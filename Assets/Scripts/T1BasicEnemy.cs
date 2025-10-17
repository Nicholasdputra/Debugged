using UnityEngine;

public class T1BasicEnemy : Enemy
{
    void Start()
    {
        canMove = true;
        health = maxHealth;
        speed = movementSpeedMultiplier * 1.0f; // Adjusted speed for T1BasicEnemy
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckIfCanMove();
    }
}