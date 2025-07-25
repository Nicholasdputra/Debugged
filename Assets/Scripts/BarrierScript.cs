using System.Collections;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] barrierSprites; // Array of sprites for the barrier
    int health = 3;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Set the initial sprite for the barrier
        spriteRenderer.sprite = barrierSprites[health];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(health > 0)
            {
                SpawnManagerScript spawnManager = other.GetComponent<Enemy>().spawnManager;
                spawnManager.enemiesLeft--;
                spawnManager.enemyInRow[other.GetComponent<Enemy>().row]--;

                AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.enemyHitBarrierSFXClip);
                Destroy(other.gameObject); // Destroy the enemy on collision

                StartCoroutine(ChangeSprite(health));
                health--; // Reduce health when an enemy collides

                // Add visual effects or sounds here
                Vector2 visualEffectPosition = other.transform.position;
                visualEffectPosition.y = gameObject.transform.position.x;
                // Instantiate(visualEffectPrefab, visualEffectPosition, Quaternion.identity);
            }
            
        }
    }

    public IEnumerator ChangeSprite(int index)
    {
        spriteRenderer.sprite = barrierSprites[index - 1];
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = barrierSprites[index];
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = barrierSprites[index - 1];
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = barrierSprites[index];
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = barrierSprites[index - 1];
    }
}
