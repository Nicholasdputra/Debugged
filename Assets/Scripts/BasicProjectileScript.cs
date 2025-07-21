using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectileScript : MonoBehaviour
{
    public int damage;
    public int speed;

    // Update is called once per frame
    void Update()
    {
        //move right
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit an enemy!");
            // Deal damage to the enemy
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.health -= damage;
            }
            
            // Destroy the projectile after hitting an enemy
            Destroy(gameObject);
        }
    }
}
