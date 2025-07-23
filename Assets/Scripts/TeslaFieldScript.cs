using System.Collections;
using UnityEngine;

public class TeslaFieldScript : MonoBehaviour
{
    int damage;

    void Start()
    {
        StartCoroutine(ExpandField());
    }

    ///Slowly expands itself from  a scale of 1 1 1 to 3.75 3 1 in 3 seconds
    IEnumerator ExpandField()
    {
        Vector3 targetScale = new Vector3(3.75f, 3.05f, 1f);
        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale; // Ensure final scale is set

        Destroy(gameObject, 0.1f); // Destroy the field after 5 seconds
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Apply damage or effects to the enemy
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.health -= damage; // Example damage value
                Debug.Log("Enemy hit by Tesla Field: " + enemy.name + " for " + damage + " damage.");
            }
        }
    }
}
