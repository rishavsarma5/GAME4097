using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FloatingTextController : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset = new Vector3(0, -0.05f, 0.2f);

    [SerializeField] InputActionReference closeTextRef;
    public TextMeshProUGUI textField;

    private AudioSource _audioSource;

    private void Awake()
    {
        closeTextRef.action.started += CloseFloatingText;
    }

    private void OnDestroy()
    {
        closeTextRef.action.started -= CloseFloatingText;
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = cameraTransform.position + cameraTransform.TransformDirection(offset);
        transform.LookAt(cameraTransform);
    }

    public void CloseFloatingText(InputAction.CallbackContext context)
    {
        _audioSource.Play();
        Destroy(this.gameObject, 0.5f);
    }
}
