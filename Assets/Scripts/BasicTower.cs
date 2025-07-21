using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
    public GameObject projectilePrefab;
    public int projectileSpeed = 2;

    void Start()
    {
        mainHub = GameObject.Find("Battery").GetComponent<HubScript>();
        
        attackCoroutine = null;
        drainCoroutine = null;

        state = 0; // Initial state
        switchCooldown = 2f;
        switchOnCost = 2;

        passiveDrain = 1;
        drainCooldown = 1f;

        damage = 1;
        range = 5f;
        attackCooldown = 2.5f;
        attackCost = 1;
        
        // Initialize the tower
        SpawnUIButtons();
    }
    
    void Update()
    {
        if (state == 1)
        {
            if (attackCoroutine == null)
            { 
                Debug.Log("Starting attack coroutine for:" + gameObject.name);
                attackCoroutine = StartCoroutine(AttackCoroutine());            
            }
            if(drainCoroutine == null)
            {
                Debug.Log("Starting drain coroutine for:" + gameObject.name);
                drainCoroutine = StartCoroutine(DrainCoroutine());
            }
        }
        else if (state == 0)
        {
            if (attackCoroutine != null)
            {
                Debug.Log("Stopping attack coroutine for:" + gameObject.name);
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            if (drainCoroutine != null)
            {
                Debug.Log("Stopping drain coroutine for:" + gameObject.name);
                StopCoroutine(drainCoroutine);
                drainCoroutine = null;
            }
        }
    }

    protected override void Attack()
    {
        Debug.Log("Attacking with Basic Tower: " + gameObject.name);
        // Implement attack logic here
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        mainHub.currentCharge -= attackCost; // Deduct charge for attack
        Debug.Log("Current Charge After Attack: " + mainHub.currentCharge);

        BasicProjectileScript projectileScript = proj.GetComponent<BasicProjectileScript>();
        if (projectileScript != null)
        {
            projectileScript.damage = damage;
            projectileScript.speed = projectileSpeed; // Set speed or any other properties as needed
        }
    }
}
