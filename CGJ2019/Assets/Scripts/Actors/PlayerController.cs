﻿using System.Collections;
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
    const float dashSpeed = 400f;
    IEnumerator dashCoroutine;
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

            //allow jumping only when grounded
            if (Input.GetButtonDown("Jump"))
            {
                currentVelocity.y += jumpStrength;
            }

            //allow dashing again
            canDash = true;
        }
        else
        {
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
        if (Input.GetButtonDown("Dash") && dashCoroutine == null)
        {
            dashCoroutine = Dash();
            StartCoroutine(dashCoroutine);
        }
    }

    //actual functionality handled in FixedUpdate()
    IEnumerator Dash()
    {
        canDash = false;
        yield return new WaitForSeconds(dashDuration);
        dashCoroutine = null;
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

    void UpdateAnimator()
    {
        anim.SetInteger("playerState", (int)playerState);
    }

    void FixedUpdate()
    {
<<<<<<< HEAD
        HandleMovementInput();
=======
        //movement
>>>>>>> origin/nic's-branch
        rb2d.velocity = currentVelocity;

        //dashing
        if (dashCoroutine != null)
        {
            Vector2 dashDirection = lastHorizontalInput == 0 ? Vector2.right : (Vector2.right * lastHorizontalInput).normalized;
            rb2d.AddForce(dashDirection * dashSpeed);
        }
    }
}