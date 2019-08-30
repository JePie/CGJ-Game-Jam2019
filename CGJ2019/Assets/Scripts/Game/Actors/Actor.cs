using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    protected SpriteRenderer sr;

    [SerializeField] protected float moveSpeed;
    protected Vector2 currentVelocity;

    protected virtual void Awake()
    {
        //init. components
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
}