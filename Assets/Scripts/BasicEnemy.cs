using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    void Start()
    {
        canMove = true;
        health = 10;
        speed = 0.5f;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckIfCanMove();
        CheckIfDead();
    }
}
