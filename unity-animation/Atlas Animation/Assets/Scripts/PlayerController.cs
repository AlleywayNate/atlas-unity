using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AnimationController animationController;
    public CharacterController characterController;
    public Transform cameraTransform;

    public float speed = 5f;
    public float gravityMultiplier = 2f;
    public float jumpForce = 5f;

    private Vector3 direction;
    private bool isJumping;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing from the GameObject.");
        }
    }

    void Start()
    {
        if (animationController == null)
        {
            Debug.LogError("AnimationController reference is missing.");
        }
        if (cameraTransform == null)
        {
            Debug.LogError("CameraTransform reference is missing.");
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check for missing components
        if (cameraTransform == null || characterController == null || animationController == null)
        {
            Debug.LogError("One or more required components are missing. Please assign them in the Inspector.");
            return;
        }

        // Get input and determine movement direction
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * vertical + right * horizontal;
        moveDirection = moveDirection.normalized * speed;

        // Set running or idle animation based on movement
        if (moveDirection.magnitude >= 0.1f)
        {
            animationController.SetRunning(true);  // Set running animation
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            animationController.SetRunning(false);  // Set idle animation if not moving
        }

        direction = new Vector3(moveDirection.x, direction.y, moveDirection.z);
        direction.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        characterController.Move(direction * Time.deltaTime);

        // Handle jumping and falling states
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animationController.Jump();  // Trigger jump animation
                isJumping = true;
                direction.y = jumpForce;
            }
            else
            {
                animationController.SetGrounded(true);  // Set grounded state
                isJumping = false;
            }
        }
        else
        {
            animationController.SetGrounded(false);  // Ensure grounded state is false while in the air
            if (!isJumping)
            {
                animationController.SetFalling(true);  // Set falling animation if not jumping
            }
            else
            {
                animationController.SetFalling(false);  // Reset falling animation if jumping
            }
        }

        // Reset player position if fallen below a certain height
        if (characterController.transform.position.y < -30.0f)
        {
            characterController.enabled = false;
            transform.position = new Vector3(0, 50f, 0);
            characterController.enabled = true;
        }

        // 
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animationController.Jump(); // Trigger jump animation
                isJumping = true;
                direction.y = jumpForce;
            }
            else
            {
                if (isJumping)
                {
                    animationController.LandImpact(); // Trigger landing impact animation
                    isJumping = false;
                }
                animationController.SetGrounded(true); // Set grounded state
                animationController.SetFalling(false); // Reset falling state
            }

            // Determine if the player is moving
            bool isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
            animationController.SetMoving(isMoving);
        }
        else
        {
            animationController.SetGrounded(false); // Ensure grounded state is false while in the air
            if (!isJumping)
            {
                animationController.SetFalling(true); // Set falling animation if not jumping
            }
        }
        
    }
}
