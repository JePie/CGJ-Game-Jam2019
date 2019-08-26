using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    const float jumpStrength = 10f;
    const float fallSpeedMultiplier = 2f;
    const float maxFallSpeed = -20f;                //used to clamp fall speed in case this object falls from too high

    protected override void Update()
    {
        base.Update();
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        //update horizontal movement
        currentVelocity.x = Input.GetAxisRaw("Horizontal");
        
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
        }
        else
        {
            //increase fall speed over time; make sure y-velocity doesn't go beyond <maxFallSpeed>
            currentVelocity.y = Mathf.Max(currentVelocity.y + Physics.gravity.y * Time.deltaTime * fallSpeedMultiplier, maxFallSpeed);
        }
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

    void FixedUpdate()
    {
        HandleMovementInput();
        rb2d.velocity = currentVelocity;
    }
}