using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float minPitch = -90f;
    [SerializeField] private float maxPitch = 90f;

    private static Vector3 angularVelocity;
    public static Vector3 AngularVelocity => angularVelocity;
    
    private Camera cam;
    private float pitch = 0f;
    void OnEnable()
    {
        Input.GetInstance().Player.Enable();
    }

    void OnDisable()
    {
        Input.GetInstance().Player.Disable();
    }

    void OnDestroy()
    {
        Input.GetInstance().Dispose();
    }

    void Start()
    {
        cam = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 lookInput = Input.GetInstance().Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        playerBody.Rotate(Vector3.up * mouseX);
        angularVelocity = Vector3.up * mouseX / Time.deltaTime;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}