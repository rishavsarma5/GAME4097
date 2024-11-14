using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class DiceRolling : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [Header("Float Components")]
    [SerializeField] float floatSpeed = 1.0f;
    [SerializeField] float floatAmplitude = 0.5f;

    [Header("Punch Info")]
    [SerializeField] GameObject rightController;
    [SerializeField] private float punchForce = 2.5f;
    [SerializeField] private float spinForce = 2f;
    [SerializeField] float stopThreshold = 0.01f;
    [SerializeField] private bool isPunched = false;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float showDiceResultWaitTime = 2.0f;

    [Header("Dice Faces")]
    [SerializeField] private List<Transform> diceFaces;

    [Space(5)]
    [SerializeField] GameObject punchCanvas;
    [SerializeField] TextMeshProUGUI punchText;
    [SerializeField] [TextArea] string punchTextDescription = "Punch me for movement rolling!";

    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider boxCollider;
    private bool isFloating = true;
    private Vector3 startPos;
    [SerializeField] private int dicePlayerIndex = -1;

    private Coroutine floatingCoroutine;

    public UnityEvent<int, int> OnDiceRollValue;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        if (dicePlayerIndex == -1)
        {
            Debug.LogError("Create unique index variable for dicePlayerIndex");
        }
        punchText.text = punchTextDescription;

    }

    private void OnEnable()
    {
        // set default to floating state
        SetToFloatingState();
    }

    private void OnDestroy()
    {
        if (TeleportDistanceManager.Instance != null)
        {
            OnDiceRollValue.RemoveListener(TeleportDistanceManager.Instance.CreateTeleportDistanceBox);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPunched && HasDiceStoppedRolling() && IsDiceGrounded())
        {
            StartCoroutine(WaitForDiceToSettle());
        }
    }

    private IEnumerator WaitForDiceToSettle()
    {
        yield return new WaitForSeconds(0.5f);

        if (IsDiceGrounded())
        {
            Debug.Log("got to dice settled state");
            int diceValue = GetNumberOnDie();
            punchText.text = $"Dice roll: {diceValue}";
            Debug.Log($"Dice rolled a {diceValue}");
            punchCanvas.SetActive(true);
            this.transform.position = startPos;
            boxCollider.enabled = false;
            // Rotate the dice so that the top face points toward the player
            AlignTopFaceTowardPlayer(diceValue);
            ResetDiceState();
            yield return new WaitForSeconds(showDiceResultWaitTime);
            OnDiceRollValue?.Invoke(dicePlayerIndex, diceValue);
            boxCollider.enabled = true;
        }
        else
        {
            Debug.Log("Dice is not grounded yet.");
        }
    }

    private void AlignTopFaceTowardPlayer(int topFaceIndex)
    {
        Transform topFace = diceFaces[topFaceIndex - 1]; // Adjust for 0-based index

        Vector3 playerDirection = (playerTransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(playerDirection, -topFace.forward);

        // Rotate the dice so that the top face points toward the player
        transform.rotation = targetRotation;
    }

    private bool HasDiceStoppedRolling()
    {
        return rb.velocity.magnitude < stopThreshold && rb.angularVelocity.magnitude < stopThreshold;
    }

    private bool IsDiceGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position - new Vector3(0, 0.1f, 0), Vector3.down, out hit, groundCheckDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!isPunched && (other.CompareTag("PlayerHand") || other.gameObject.layer == LayerMask.NameToLayer("Player")))
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

            Vector3 punchDir = (this.transform.position - other.transform.position).normalized;
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

    private void ResetDiceState()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isFloating = true;
        isPunched = false;

        startPos = this.transform.position;
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
        return topFace;
    }

    public int GetDicePlayerIndex()
    {
        return dicePlayerIndex;
    }

    public void ResetText()
    {
        punchText.text = punchTextDescription;
    }

    public void SetupDiceForController()
    {
        rb.useGravity = false;
    }
}
