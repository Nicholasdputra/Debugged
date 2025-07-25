using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    public SpawnManagerScript spawnManager;
    public Sprite originalSprite;
    public Sprite damagedSprite;
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            canMove = false;
            Die();
        }
        else
        {
            Debug.Log("Enemy: " + gameObject.name + " took " + damage + " damage. Remaining health: " + health);
            StartCoroutine(FlashWhite());
        }
    }

    private IEnumerator FlashWhite()
    {
        // Debug.Log("Enemy: " + gameObject.name + " is flashing white.");
        //pause animator
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = damagedSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = originalSprite;
        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    // Method to handle enemy death
    public void Die()
    {
        Debug.Log("Enemy: " + gameObject.name + " has died.");
        AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.enemyDeathSFXClip);
        spawnManager.enemyInRow[row]--;
        spawnManager.enemiesLeft--;
        Destroy(gameObject);
    }
}