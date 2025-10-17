using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    public SpawnManagerScript spawnManager;
    public Sprite originalSprite;
    public Sprite hitSprite;
    public Sprite[] damagedSprites;
    public static float movementSpeedMultiplier = 0.5f;
    public bool canMove;
    [SerializeField] protected int maxHealth; 
    public int health;
    public float speed;
    public Rigidbody2D rb;
    public int row;

    // Method to handle enemy movement
    public void Move()
    {
        //Move Left 
        if (canMove)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic && rb.velocity.x <= 0.1)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    // Method to check if the enemy can move
    public void CheckIfCanMove()
    {
        if (canMove)
        {
            Move();
        }
    }

    // Method to handle taking damage and knocking them back
    public void TakeDamage(int damage, float knockbackForce)
    {
        health -= damage;
        if (health <= 0)
        {
            canMove = false;
            Die();
        }
        else
        {
            canMove = false;
            if(rb.bodyType == RigidbodyType2D.Kinematic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            rb.velocity = Vector2.zero;
            // Debug.Log("Current Velocity after reset: " + rb.velocity);
            rb.AddForce(Vector2.right * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(FlashWhite());
        }
    }

    // Overloaded method to handle taking damage without knockback
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
            // Debug.Log("Enemy: " + gameObject.name + " took " + damage + " damage. Remaining health: " + health);
            StartCoroutine(FlashWhite());
        }
    }

    // Coroutine to flash the enemy white when hit
    private IEnumerator FlashWhite()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = hitSprite;
        yield return new WaitForSeconds(0.1f);

        if (health <= maxHealth * 0.33f && damagedSprites.Length >= 2)
        {
            spriteRenderer.sprite = damagedSprites[1];
        }
        else if (health <= maxHealth * 0.66f && damagedSprites.Length >= 1)
        {
            spriteRenderer.sprite = damagedSprites[0];
        }
        else
        {
            spriteRenderer.sprite = originalSprite;
        }

        if (animator != null)
        {
            animator.enabled = true;
        }
        if (canMove == false)
        {
            canMove = true;
        }
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            yield return new WaitForSeconds(0.3f);
            rb.velocity = Vector2.zero; // Stop any residual movement after knockback
            rb.bodyType = RigidbodyType2D.Kinematic; // Switch back to kinematic for normal movement
        }
    }

    // Method to handle enemy death
    public void Die()
    {
        // Debug.Log("Enemy: " + gameObject.name + " has died.");
        AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.enemyDeathSFXClip);
        spawnManager.enemyInRow[row]--;
        spawnManager.enemiesLeft--;
        Destroy(gameObject);
    }
}