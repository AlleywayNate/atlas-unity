using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public AnimationController animationController;
    public CharacterController characterController;
    public Transform cameraTransform; // Renamed to avoid conflicts

    public float speed = 5f;
    public float gravityMul = 2f;
    public float jumpForce = 5f;
    public Vector3 startPosition = new Vector3(0, 50f, 0); // Starting position

    private Vector3 direction;
    private bool isJumping = false;
    private bool isImpactPlaying = false;
    private bool isGettingUpPlaying = false;
    private bool isImpactPlayingLogged = false;

    private bool isFalling = false;
    private float fallDelay = 0.2f;
    private Coroutine fallDelayCoroutine;

    void Awake()
    {
        // Initialize CharacterController
        characterController = GetComponent<CharacterController>();

        // Check if CharacterController component is missing
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing from the GameObject.");
        }
    }

    void Start()
    {
        // Check if Animator or CameraTransform is missing
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
        // Check if the animations are playing
        isImpactPlaying = animationController.IsAnimationPlaying("Falling Flat Impact");
        isGettingUpPlaying = animationController.IsAnimationPlaying("Getting Up");

        // Only log if there's a change in state
        if (isImpactPlaying || isGettingUpPlaying)
        {
            if (!isImpactPlayingLogged)
            {
                Debug.Log("Movement disabled due to impact or getting up animation.");
                isImpactPlayingLogged = true;
            }
            // Disable movement logic
            return;
        }
        else
        {
            isImpactPlayingLogged = false;
        }

        // Check for null references before proceeding
        if (cameraTransform == null || characterController == null || animationController == null)
        {
            Debug.LogError("One or more required components are missing. Please assign them in the Inspector.");
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Determine movement direction based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f; // Flatten the forward vector
        right.y = 0f; // Flatten the right vector

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * vertical + right * horizontal;
        moveDirection = moveDirection.normalized * speed;

        // Check if player is moving or idle and set animation accordingly
        if (moveDirection.magnitude >= 0.1f)
        {
            animationController.Move();
            // Rotate the player to face the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            animationController.Idle();
        }

        // Apply movement direction and gravity
        direction = new Vector3(moveDirection.x, direction.y, moveDirection.z);
        direction.y += Physics.gravity.y * gravityMul * Time.deltaTime;
        characterController.Move(direction * Time.deltaTime);

        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Start the Jump animation immediately
                animationController.Jump();
                isJumping = true;
                direction.y = jumpForce;
                // Stop any falling animation if jumping
                if (isFalling) 
                {
                    animationController.SetFalling(false);
                    isFalling = false;
                }
            }
            else
            {
                if (isJumping)
                {
                    // Check if the player is landing
                    animationController.LandImpact();
                    isJumping = false;
                }
                animationController.SetGrounded(true); // Set grounded state
                bool isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
                animationController.SetMoving(isMoving); // Set moving state
            }
        }
        else
        {
            animationController.SetGrounded(false); // Ensure grounded state is false while in the air
            if (!isJumping && !isFalling)
            {
                fallDelayCoroutine = StartCoroutine(FallDelayCoroutine()); // Start falling animation after delay
            }
        }

        // Fall check and respawn
        if (characterController.transform.position.y < -30.0f)
        {
            ResetPlayerPosition(); // Call method to reset position and start falling animation
        }
    }

    private IEnumerator FallDelayCoroutine()
    {
        Debug.Log("Fall delay started.");
        yield return new WaitForSeconds(fallDelay);
        animationController.SetFalling(true); // Start falling animation after delay
        isFalling = true;
        Debug.Log("Fall delay ended, falling animation started.");
    }

    private void ResetPlayerPosition()
    {
        // Reset the player’s position
        characterController.enabled = false;
        transform.position = startPosition;
        characterController.enabled = true;

        // Trigger the falling animation
        animationController.SetFalling(true);
        isFalling = true;
    }
}
