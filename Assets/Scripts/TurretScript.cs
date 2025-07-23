using UnityEngine;

public class TurretScript : Tower
{
    public GameObject projectilePrefab;
    int projectileSpeed = 6;

    void Start()
    {
        mainHub = transform.parent.Find("Battery").GetComponent<HubScript>();
        
        attackCoroutine = null;
        drainCoroutine = null;

        state = 0; // Initial state
        switchCooldown = 2f;
        switchOnCost = 15;

        passiveDrain = 3;
        drainCooldown = 5f;

        damage = 50;
        range = 999f;
        attackCooldown = 2.5f;
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
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.layer = 2;

        mainHub.currentCharge -= attackCost; // Deduct charge for attack
        // Debug.Log("Current Charge After Attack: " + mainHub.currentCharge);

        BasicProjectileScript projectileScript = proj.GetComponent<BasicProjectileScript>();
        if (projectileScript != null)
        {
            projectileScript.damage = damage;
            projectileScript.speed = projectileSpeed; // Set speed or any other properties as needed
        }
    }
}
