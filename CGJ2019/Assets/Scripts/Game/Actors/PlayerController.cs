using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    Animator anim;

    const float jumpStrength = 10f;
    const float fallSpeedMultiplier = 2f;
    const float maxFallSpeed = -20f;                //used to clamp fall speed in case this object falls from too high

    bool canDash = true;
    const float dashDuration = 0.25f;
    const float dashCooldown = dashDuration * 2;
    const float dashSpeed = 400f;
    IEnumerator dashCoroutine, dashCooldownCoroutine;

    float lastHorizontalInput;                       //technically a Vector1, used for dashing without a horizontal input

    enum PlayerState
    {
        Idle,
        Walk,
        Jump,
        Dash,
        Death
    }
    PlayerState playerState;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        HandleMovementInput();
        UpdateAnimator();
    }

    void HandleMovementInput()
    {
        //update horizontal movement
        if (dashCoroutine == null)
        {
            currentVelocity.x = Input.GetAxisRaw("Horizontal");

            if (currentVelocity.x > 0) { sr.flipX = false; }
            else if (currentVelocity.x < 0) { sr.flipX = true; }
        }

        //update last non-zero horizontal input
        if (currentVelocity.x != 0f)
        {
            lastHorizontalInput = currentVelocity.x;
        }

        //update vertical movement
        if (IsGrounded())
        {
            //reset y-velocity to 0
            currentVelocity.y = 0;

            //allow dashing again
            canDash = true;

            //allow jumping only when grounded
            if (Input.GetButtonDown("Jump"))
            {
                currentVelocity.y += jumpStrength;
            }

            if (dashCoroutine == null)
            {
                playerState = currentVelocity == Vector2.zero ? PlayerState.Idle : PlayerState.Walk;
            }
        }
        else
        {
            playerState = PlayerState.Jump;

            if (dashCoroutine == null)
            {
                //increase fall speed over time; make sure y-velocity doesn't go beyond <maxFallSpeed>
                currentVelocity.y = Mathf.Max(currentVelocity.y + Physics.gravity.y * Time.deltaTime * fallSpeedMultiplier, maxFallSpeed);
            }
            //reset y-velocity to 0 when dashing (resume falling once dash has ended)
            else
            {
                currentVelocity.y = 0;
            }
        }

        //handle dash input
        if (Input.GetButtonDown("Dash") && dashCooldownCoroutine == null && canDash)
        {
            playerState = PlayerState.Dash;
            dashCoroutine = Dash();
            StartCoroutine(dashCoroutine);
        }
    }

    //actual functionality handled in FixedUpdate()
    IEnumerator Dash()
    {
        //set rigidbody type to Dynamic so that force can be applied to it
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        canDash = false;
        yield return new WaitForSeconds(dashDuration);

        //reset rigidbody type and <playerState>
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        playerState = PlayerState.Idle;

        //start dash cooldown
        dashCooldownCoroutine = CooldownDash();
        StartCoroutine(dashCooldownCoroutine);
        dashCoroutine = null;
    }

    //while this is running, player cannot dash
    IEnumerator CooldownDash()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashCooldownCoroutine = null;
    }

    //returns true if there is an object in the Ground layer underneath this object; otherwise false
    bool IsGrounded()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = Vector2.down;
        float marginOfError = 0.01f;            //can be adjusted
        float rayDistance = GetComponent<Collider2D>().bounds.extents.y + marginOfError;
        int groundLayer = 1 << LayerMask.NameToLayer("Ground");

        RaycastHit2D result = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayer);
        return result;
    }

    //sync "playerState" parameter in Animator controller to <playerState> in script
    void UpdateAnimator()
    {
        anim.SetInteger("playerState", (int)playerState);
    }

    void FixedUpdate()
    {
        //movement
        rb2d.velocity = currentVelocity;

        //dashing
        if (dashCoroutine != null)
        {
            Vector2 dashDirection = lastHorizontalInput == 0 ? Vector2.right : (Vector2.right * lastHorizontalInput).normalized;
            rb2d.AddForce(dashDirection * dashSpeed);
        }
    }
}