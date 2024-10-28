using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DiceRolling : MonoBehaviour
{
    [Header("Float Components")]
    [SerializeField] float floatSpeed = 1.0f;
    [SerializeField] float floatAmplitude = 0.5f;

    [SerializeField] GameObject rightController;
    [SerializeField] private float punchForce = 2.5f;

    [SerializeField] GameObject punchText;

    [SerializeField] float stopThreshold = 0.01f;

    private Rigidbody rb;
    private bool isFloating = true;
    private Transform startTransform;
    private Vector3 startPos;
    private bool hasStopped = false;
    private bool isPunched = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startTransform = this.transform;
        startPos = this.transform.position;

        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFloating)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            this.transform.position = new Vector3(startPos.x, newY, startPos.z);
        }

        if (!hasStopped && HasDiceStoppedRolling())
        {
            hasStopped = true;
            // determine side that's up for value
            rb.useGravity = false;
            transform.position = startTransform.position;
            transform.rotation = startTransform.rotation;
            isFloating = true;
            punchText.SetActive(true);
            isPunched = false;
        } else if (hasStopped && !HasDiceStoppedRolling())
        {
            hasStopped = false;
        }
    }

    private bool HasDiceStoppedRolling()
    {
        return rb.velocity.magnitude < stopThreshold && rb.angularVelocity.magnitude < stopThreshold;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("got to on trigger enter");
        if (!isPunched && other.CompareTag("PlayerHand"))
        {
            isFloating = false;
            rb.useGravity = true;
            punchText.SetActive(false);
            isPunched = true;

            Vector3 punchDir = (this.transform.position - other.transform.position).normalized;
            
            if (rightController.TryGetComponent(out Rigidbody controllerRb))
            {
                Debug.Log("using right controller's velocity");
                Vector3 controllerVelocity = controllerRb.velocity;
                rb.AddForce(controllerVelocity * punchForce, ForceMode.Impulse);
            } else
            {
                Debug.Log("using basic velocity");
                rb.AddForce(punchDir * punchForce, ForceMode.Impulse);
            }
            
            //rb.AddForce(punchDir * punchForce, ForceMode.Impulse);
        }
    }


}
