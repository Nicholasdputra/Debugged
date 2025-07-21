using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    int health = 300;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Destroy the enemy on collision
            health -= other.gameObject.GetComponent<Enemy>().health; // Reduce health when an enemy collides
            Debug.Log("Barrier Health: " + health);
            if (health <= 0)
            {
                Destroy(gameObject); // Destroy barrier when health is zero
                Debug.Log("Barrier Destroyed");
            }

            // Add visual effects or sounds here
            Vector2 visualEffectPosition = other.transform.position;
            visualEffectPosition.y = gameObject.transform.position.x;
            // Instantiate(visualEffectPrefab, visualEffectPosition, Quaternion.identity);
        }
    }
}
