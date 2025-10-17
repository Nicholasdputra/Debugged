using UnityEngine;

public class T2BasicEnemy : Enemy
{
    void Start()
    {
        canMove = true;
        health = maxHealth;
        speed = movementSpeedMultiplier * 1.0f;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckIfCanMove();
    }
}
