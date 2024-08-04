using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float mouseSensitivity = 100f;
    public float distanceFromPlayer = 5f; // Distance from the player
    public float heightOffset = 2f; // Height offset from the player
    public float minVerticalAngle = -35f; // Minimum vertical angle
    public float maxVerticalAngle = 60f; // Maximum vertical angle

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
    }

    void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate the new rotation
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle); // Clamp the vertical rotation

        // Calculate the rotation and position of the camera
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Vector3 position = player.position - rotation * Vector3.forward * distanceFromPlayer + Vector3.up * heightOffset;

        // Apply the rotation and position to the camera
        transform.rotation = rotation;
        transform.position = position;
    }
}
