using UnityEngine;

public class DeleteProjectilesScript : MonoBehaviour
{
    void Start()
    {
        Debug.Log("DeleteProjectilesScript started on: " + gameObject.name);
        
        // Check collider setup
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Debug.Log("Collider found: " + col.GetType() + ", IsTrigger: " + col.isTrigger);
        }
        else
        {
            Debug.LogError("No Collider2D found on " + gameObject.name);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Projectile destroyed");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Something staying in trigger: " + collision.gameObject.name);
    }
}