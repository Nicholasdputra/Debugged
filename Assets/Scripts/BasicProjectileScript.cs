using UnityEngine;

public class BasicProjectileScript : MonoBehaviour
{
    public float knockbackForce;
    public int damage;
    public int speed;
    public int lane;

    void Start()
    {
        Debug.Log("Projectile started on: " + gameObject.name);
        
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

    // Update is called once per frame
    void Update()
    {
        //move right
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && other.GetComponent<Enemy>().row + 1 == lane)
        {
            AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.turretProjectileHitSFXClip);

            // Deal damage to the enemy
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, knockbackForce);
            }

            // Destroy the projectile after hitting an enemy
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Something staying in trigger: " + collision.gameObject.name);
    }
}
