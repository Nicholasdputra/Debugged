using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTrapScript : Tower
{
    public GameObject teslaFieldPrefab;
    public int teslaFieldExpansionSpeed = 1;

    void Start()
    {
        mainHub = transform.parent.Find("Battery").GetComponent<HubScript>();
        
        attackCoroutine = null;
        drainCoroutine = null;

        state = 0; // Initial state
        switchCooldown = 3f;
        switchOnCost = 20;

        passiveDrain = 4;
        drainCooldown = 5f;

        damage = 20;
        range = 3f;
        attackCooldown = 1f;
        attackCost = 3;
        
        // Initialize the tower
        SpawnUIButtons();
    }

    void Update()
    {
        CheckForCharge();
        if (state == 1)
        {
            if (attackCoroutine == null)
            {
                Debug.Log("Starting attack coroutine for:" + gameObject.name);
                attackCoroutine = StartCoroutine(AttackCoroutine());
            }
            if (drainCoroutine == null)
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
        // Debug.Log("Attacking with Basic Tower: " + gameObject.name);
        // Implement attack logic here
        GameObject proj = Instantiate(teslaFieldPrefab, transform.position, Quaternion.identity);
        proj.layer = 2;

        mainHub.currentCharge -= attackCost; // Deduct charge for attack
        // Debug.Log("Current Charge After Attack: " + mainHub.currentCharge);
    }
}
