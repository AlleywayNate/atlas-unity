using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    static Animator animator;
    public CharacterController player;

    public bool isAirborne;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Method to set the running animation
    public void SetRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    // Method to set the idle animation
    public void SetIdle(bool isIdle)
    {
        animator.SetBool("isIdle", isIdle);
    }

    // Method to trigger the jump animation
    public void Jump()
    {
        animator.SetTrigger("isJumping");
    }

    // Method to set the falling state
    public void SetFalling(bool isFalling)
    {
        animator.SetBool("isFalling", isFalling);
    }

    // Method to set the grounded state
    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    // Method to indicate the player is airborne
    public void IsAirborne()
    {
        isAirborne = true;
        animator.SetTrigger("isFalling");
    }

    // Method to indicate the player is grounded
    public void IsGrounded()
    {
        isAirborne = false;
        animator.ResetTrigger("isFalling");
    }


    public void SetMoving(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }

    public void LandImpact()
    {
        animator.SetTrigger("haslanded");
    }
}
