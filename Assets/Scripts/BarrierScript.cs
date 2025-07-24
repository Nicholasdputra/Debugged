using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    int health = 9999;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManagerScript.Instance.spawnManager.enemiesLeft--;
            GameManagerScript.Instance.spawnManager.enemyInRow[other.GetComponent<Enemy>().row]--;
            
            AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.enemyHitBarrierSFXClip);
            Destroy(other.gameObject); // Destroy the enemy on collision
            health--; // Reduce health when an enemy collides

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
