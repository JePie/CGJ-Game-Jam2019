using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            player.Die();
        }
    }
}