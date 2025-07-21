using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool canMove;
    public int health;
    public float speed;
    public Rigidbody2D rb;

    // Method to handle enemy movement
    public void Move()
    {
        //Move Left 
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void CheckIfCanMove()
    {
        if (canMove)
        {
            Move();
        }
    }

    public void CheckIfDead()
    {
        if (health <= 0)
        {
            canMove = false;
            Die();
        }
    }

    // Method to handle enemy death
    public void Die()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}