using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceController : MonoBehaviour
{
    [SerializeField] private GameObject dice;
    [SerializeField] private GameObject diceTextCanvas;
    [SerializeField] private float moveDiceSpeed = 2f;

    [SerializeField] private Transform cameraTransform;

    public InputActionReference LC_JoystickRef;
    public InputActionReference LC_PrimaryButtonRef;
    public InputActionReference LC_TriggerRef;

    private bool diceLocationLocked = false;
    private bool dicePositionLocked = false;
    private bool diceControllerFinished = false;

    private DiceRolling _diceRollingScript;
    private Rigidbody _diceRb;


    private void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _diceRollingScript = dice.GetComponent<DiceRolling>();
        _diceRb = dice.GetComponent<Rigidbody>();
        _diceRollingScript.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!LC_JoystickRef || !LC_PrimaryButtonRef || !LC_TriggerRef)
        {
            throw new System.Exception("Input Action refs not set");
        }
    }

    private void OnEnable()
    {
        LC_PrimaryButtonRef.action.started += LC_PrimaryCustomAction;
        LC_TriggerRef.action.started += LC_TriggerCustomAction;
        DisableDiceBoxCollider();
        diceControllerFinished = false;
        diceLocationLocked = false;
        dicePositionLocked = false;
        _diceRb.useGravity = false;
    }

    private void OnDisable()
    {
        LC_PrimaryButtonRef.action.started -= LC_PrimaryCustomAction;
        LC_TriggerRef.action.started -= LC_TriggerCustomAction;
        _diceRollingScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (diceControllerFinished) return;

        if (!diceLocationLocked) // dice location placing stage
        {
            // Move dice horizontally based on the camera's left-right direction
            Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * 1.5f;
            targetPosition += cameraTransform.right * Mathf.Sin(Time.time * moveDiceSpeed) * 0.5f;
            this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
            this.transform.LookAt(cameraTransform);
            //transform.LookAt(cameraTransform);
            return;
        }
        else if (!dicePositionLocked) // dice position placing stage
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
        } else
        {
            EnableDiceBoxCollider();
            _diceRollingScript.enabled = true;
            diceControllerFinished = true;
        }
    }

    public void MoveDiceDistance(bool moveAway)
    {
        Vector3 forwardDirection = cameraTransform.forward;

        // Adjust the dice position either closer or farther along the forward direction
        this.transform.position += (moveAway ? 1 : -1) * moveDiceSpeed * Time.deltaTime * forwardDirection;
    }

    public void MoveDiceVertical(bool moveUp)
    {
        this.transform.position += (moveUp ? 1 : -1) * moveDiceSpeed * Time.deltaTime * Vector3.up;
    }

    public void LC_PrimaryCustomAction(InputAction.CallbackContext context)
    {
        // unlock dice location
        if (diceLocationLocked)
        {
            diceLocationLocked = false;
        }
    }

    public void LC_TriggerCustomAction(InputAction.CallbackContext context)
    {
        Debug.Log("Left Trigger pressed");
        if (!diceLocationLocked) // lock dice location
        {
            diceLocationLocked = true;
            FloatingTextSpawner.Instance.SpawnFloatingTextWithTimedDestroy("Use L Stick to Move Dice Up/Down, Closer/Far and L Trigger to Confirm when ready.", 5f);
        }
        else if (!dicePositionLocked) // lock dice position
        {
            dicePositionLocked = true;
        }
    }

    private void DisableDiceBoxCollider()
    {
        dice.GetComponent<BoxCollider>().enabled = false;
    }

    private void EnableDiceBoxCollider()
    {
        dice.GetComponent<BoxCollider>().enabled = true;
    }

}
