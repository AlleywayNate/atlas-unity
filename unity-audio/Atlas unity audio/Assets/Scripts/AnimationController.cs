using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    static Animator animator;
    public CharacterController player;

    public bool isAirborne;

    private bool isRunningLogged = false;
    private bool isIdleLogged = false;
    private bool isJumpingLogged = false;
    private bool isFallingLogged = false;
    private bool isGroundedLogged = false;
    private bool isMovingLogged = false;

    private Dictionary<string, bool> wasPlaying = new Dictionary<string, bool>();
    private Dictionary<string, bool> wasFinished = new Dictionary<string, bool>();

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Move()
    {
        if (!isRunningLogged)
        {
            Debug.Log("Move animation triggered.");
            isRunningLogged = true;
            isIdleLogged = false;
        }
        animator.SetBool("isRunning", true);
        animator.SetBool("isIdle", false);
    }

    public void Idle()
    {
        if (!isIdleLogged)
        {
            Debug.Log("Idle animation triggered.");
            isIdleLogged = true;
            isRunningLogged = false;
        }
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", true);
    }

    public void Jump()
    {
        if (!isJumpingLogged)
        {
            Debug.Log("Jump animation triggered.");
            isJumpingLogged = true;
        }
        animator.SetTrigger("isJumping");
    }

    public void IsFalling()
    {
        if (!isFallingLogged)
        {
            Debug.Log("Falling animation triggered.");
            isFallingLogged = true;
            isJumpingLogged = false;
        }
        isAirborne = true;
        animator.SetTrigger("isFalling");
    }

    public void IsGrounded()
    {
        if (!isGroundedLogged)
        {
            Debug.Log("Grounded state set.");
            isGroundedLogged = true;
            isFallingLogged = false;
        }
        isAirborne = false;
        animator.ResetTrigger("isFalling");
    }

    public void LandImpact()
    {
        Debug.Log("Land impact animation triggered.");
        animator.SetTrigger("landImpact");
    }

    public void SetGrounded(bool isGrounded)
    {
        if (isGrounded != isGroundedLogged)
        {
            Debug.Log("Grounded state set to: " + isGrounded);
            isGroundedLogged = isGrounded;
        }
        animator.SetBool("isGrounded", isGrounded);
    }

    public void SetFalling(bool isFalling)
    {
        if (isFalling != isFallingLogged)
        {
            Debug.Log("Falling state set to: " + isFalling);
            isFallingLogged = isFalling;
        }
        animator.SetBool("isFalling", isFalling);
        // Explicitly handle transitions when the falling state changes
        if (isFalling)
        {
            animator.SetTrigger("isFalling");
        }
    }

    public void SetMoving(bool isMoving)
    {
        if (isMoving != isMovingLogged)
        {
            Debug.Log("Moving state set to: " + isMoving);
            isMovingLogged = isMoving;
        }
        animator.SetBool("isMoving", isMoving);
    }

    public bool IsAnimationPlaying(string animationName)
    {
        if (animator != null)
        {
            if (!wasPlaying.ContainsKey(animationName))
            {
                wasPlaying[animationName] = false;
            }

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            bool isPlaying = stateInfo.IsName(animationName);

            if (isPlaying != wasPlaying[animationName])
            {
                Debug.Log(animationName + " animation playing: " + isPlaying);
                wasPlaying[animationName] = isPlaying;
            }

            return isPlaying;
        }
        else
        {
            Debug.LogError("Animator component is not initialized.");
            return false;
        }
    }

    public bool IsAnimationFinished(string animationName)
    {
        if (animator != null)
        {
            if (!wasFinished.ContainsKey(animationName))
            {
                wasFinished[animationName] = false;
            }

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            bool isFinished = stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1f;

            if (isFinished != wasFinished[animationName])
            {
                Debug.Log(animationName + " animation finished: " + isFinished);
                wasFinished[animationName] = isFinished;
            }

            return isFinished;
        }
        else
        {
            Debug.LogError("Animator component is not initialized.");
            return false;
        }
    }
}
