using UnityEngine;

public class BasicProjectileScript : MonoBehaviour
{
    public int damage;
    public int speed;
    public int lane;

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
            Debug.Log("Hit an enemy!");
            // Deal damage to the enemy
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            
            // Destroy the projectile after hitting an enemy
            Destroy(gameObject);
        }
    }
}
