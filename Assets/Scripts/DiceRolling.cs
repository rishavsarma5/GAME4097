using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class DiceRolling : MonoBehaviour
{
    [Header("Float Components")]
    [SerializeField] float floatSpeed = 1.0f;
    [SerializeField] float floatAmplitude = 0.5f;

    [Header("Punch Info")]
    [SerializeField] GameObject rightController;
    [SerializeField] private float punchForce = 2.5f;
    [SerializeField] private float spinForce = 2f;
    [SerializeField] float stopThreshold = 0.01f;

    [Header("Dice Faces")]
    [SerializeField] private List<Transform> diceFaces;

    [Space(5)]
    [SerializeField] GameObject punchCanvas;
    [SerializeField] TextMeshProUGUI punchText;

    [SerializeField] private Rigidbody rb;
    private bool isFloating = true;
    private Transform startTransform;
    private Vector3 startPos;
    private bool isPunched = false;
    private Coroutine floatingCoroutine;

    public UnityEvent<int> OnDiceRollValue;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startTransform = this.transform;
        // set default to floating state
        SetToFloatingState();
    }

    private void OnEnable()
    {
        // reset to floating state when enabled
        //SetToFloatingState();
        //this.transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPunched && HasDiceStoppedRolling())
        {
            int diceValue = GetNumberOnDie();
            punchText.text = $"Dice roll: {diceValue}";
            punchCanvas.SetActive(true);
            SetToFloatingState();
            this.transform.Rotate(-90f, 0, 0);
            this.transform.position = startTransform.position;
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
            // Stop floating when punched
            if (floatingCoroutine != null)
            {
                StopCoroutine(floatingCoroutine);
            }

            isFloating = false;
            isPunched = true;
            rb.useGravity = true;

            punchCanvas.SetActive(false);

            Vector3 punchDir = (other.transform.position - this.transform.position).normalized;
            rb.AddForce(punchDir * punchForce, ForceMode.Impulse);

            float randX = Random.Range(0f, 1f);
            float randY = Random.Range(0f, 1f);
            float randZ = Random.Range(0f, 1f);
            rb.AddTorque(new Vector3(randX, randY, randZ) * spinForce, ForceMode.Impulse);
        }
    }

    private void SetToFloatingState()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isFloating = true;
        isPunched = false;

        startPos = this.transform.position;

        // Start the floating coroutine
        floatingCoroutine = StartCoroutine(FloatEffect());
    }

    private IEnumerator FloatEffect()
    {
        while (isFloating)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
            yield return null;
        }
    }

    private int GetNumberOnDie()
    {
        if (diceFaces == null) throw new System.Exception("Dice needs dice face transforms");

        var topFace = 0;
        var lastYPosition = diceFaces[0].position.y;

        for (int i = 0; i < diceFaces.Count; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }

        topFace++; // to make dice value 1-indexed
        OnDiceRollValue?.Invoke(topFace);
        return topFace;
    }
}
