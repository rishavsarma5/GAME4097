using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DiceController : MonoBehaviour
{
    [SerializeField] private GameObject dice;
    [SerializeField] private GameObject diceTextCanvas;
    [SerializeField] private TextMeshProUGUI diceText;
    [SerializeField] [TextArea] private string diceMovementText = "Use L Joystick to move the dice to a punchable spot. Press L Trigger to confirm Dice Position.";
    [SerializeField] private float moveDiceSpeed = 2f;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private AudioClip triggerClickClip;

    public InputActionReference LC_JoystickRef;
    public InputActionReference LC_TriggerRef;

    private bool dicePositionLocked = false;
    private bool diceControllerFinished = false;

    private DiceRolling _diceRollingScript;
    private Rigidbody _diceRb;

    private void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        LC_TriggerRef.action.started += LC_TriggerCustomAction;
        TeleportDistanceManager.Instance.AddCreatedDice(this.gameObject);
        _diceRollingScript = dice.GetComponent<DiceRolling>();
        _diceRb = dice.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!LC_JoystickRef || !LC_TriggerRef)
        {
            throw new System.Exception("Input Action refs not set");
        }

        ChangeDiceBoxColliderState(false);
        diceControllerFinished = false;
        dicePositionLocked = false;
        _diceRb.useGravity = false;
        diceText.text = diceMovementText;
    }

    private void OnDestroy()
    {
        LC_TriggerRef.action.started -= LC_TriggerCustomAction;
    }

    // Update is called once per frame
    void Update()
    {
        if (diceControllerFinished) return;

        if (!dicePositionLocked) // dice position placing stage
        {
            Vector2 leftControllerValue = LC_JoystickRef.action.ReadValue<Vector2>();

            float vertValue = leftControllerValue.y;
            float horzValue = leftControllerValue.x;

            if (horzValue <= -0.5f) // joystick moving left
            {
                MoveDiceDistance(true); // move dice away
            }
            else if (horzValue >= 0.5f) // joystick moving right
            {
                MoveDiceDistance(false); // move dice closer
            }

            if (vertValue <= -0.5f) // joystick moving down
            {
                MoveDiceVertical(false); // moves dice down
            }
            else if (vertValue >= 0.5f) // joystick moving up
            {
                MoveDiceVertical(true); // moves dice up
            }

            return;
        }
        else
        {
            ChangeDiceBoxColliderState(true);
            _diceRollingScript.SetupDiceForRolling();
            diceControllerFinished = true;
        }
    }

    public void MoveDiceDistance(bool moveAway)
    {
        Vector3 forwardDirection = cameraTransform.forward;

        // Adjust the dice position either closer or farther along the forward direction based on camera
        this.transform.position += (moveAway ? 1 : -1) * moveDiceSpeed * Time.deltaTime * forwardDirection;
    }

    public void MoveDiceVertical(bool moveUp)
    {
        // Adjust the dice position upwards or downwards
        this.transform.position += (moveUp ? 1 : -1) * moveDiceSpeed * Time.deltaTime * Vector3.up;
    }

    public void LC_TriggerCustomAction(InputAction.CallbackContext context)
    {
        Debug.Log("Left Trigger pressed to lock dice position");
        if (!dicePositionLocked) // lock dice position
        {
            // lock the position so the player can adjust the dice's position
            AudioSource.PlayClipAtPoint(triggerClickClip, Camera.main.transform.position);
            dicePositionLocked = true;
        }
    }

    private void ChangeDiceBoxColliderState(bool shouldEnable)
    {
        dice.GetComponent<BoxCollider>().enabled = shouldEnable;
    }

    public void DestroyDice()
    {
        Destroy(gameObject);
    }

}
