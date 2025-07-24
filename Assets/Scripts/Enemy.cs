using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public static float movementSpeedMultiplier = 0.5f;
    public bool canMove;
    public int health;
    public float speed;
    public Rigidbody2D rb;
    public int row;

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
        AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.enemyDeathSFXClip);
        GameManagerScript.Instance.spawnManager.enemyInRow[row]--;
        GameManagerScript.Instance.spawnManager.enemiesLeft--;
        Destroy(gameObject);
    }
}