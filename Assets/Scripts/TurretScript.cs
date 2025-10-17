using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine;

public class TurretScript : Tower
{
    public GameObject projectilePrefab;
    public GameObject muzzle;
    public int lane;
    int projectileSpeed = 6;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        FindLane();
        muzzle = transform.GetChild(0).gameObject;
        mainHub = transform.parent.Find("Battery").GetComponent<HubScript>();

        // attackCoroutine = null;
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
    }

    void Update()
    {
        CheckForCharge();
        if (state == 1)
        {
            if (drainCoroutine == null)
            {
                // Debug.Log("Starting drain coroutine for:" + gameObject.name);
                drainCoroutine = StartCoroutine(DrainCoroutine());
            }
        }
        else if (state == 0)
        {
            if (attackCoroutine != null)
            {
                // Debug.Log("Stopping attack coroutine for:" + gameObject.name);
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            if (drainCoroutine != null)
            {
                // Debug.Log("Stopping drain coroutine for:" + gameObject.name);
                StopCoroutine(drainCoroutine);
                drainCoroutine = null;
            }
        }
    }

    protected void FindLane()
    {
        string objectName = gameObject.name; // e.g., "turret 1"
        Match match = Regex.Match(objectName, @"\d+"); // Finds first sequence of digits
        if (match.Success)
        {
            lane = int.Parse(match.Value);
        }
    }

    protected override void Attack()
    {
        // Debug.Log("Attacking with Basic Tower: " + gameObject.name);
        // Implement attack logic here
        if (animator.enabled == false)
        {
            animator.enabled = true; // Enable the animator to play the attack animation
        }
        animator.SetTrigger("Attack");

        if (turnOffAnimatorCoroutine != null)
        {
            StopCoroutine(turnOffAnimatorCoroutine); // Stop any existing coroutine
            turnOffAnimatorCoroutine = null; // Clear reference
        }
        turnOffAnimatorCoroutine = StartCoroutine(TurnOffAnimatorCoroutine());

        StartCoroutine(SpawnBullet());
    }

    public IEnumerator SpawnBullet()
    {
        yield return new WaitForSeconds(0.4f); // Wait for the attack animation to play
        AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.turretFireSFXClip);
        GameObject proj = Instantiate(projectilePrefab, muzzle.transform.position, Quaternion.identity);

        mainHub.currentCharge -= attackCost; // Deduct charge for attack
        // Debug.Log("Current Charge After Attack: " + mainHub.currentCharge);

        BasicProjectileScript projectileScript = proj.GetComponent<BasicProjectileScript>();
        if (projectileScript != null)
        {
            projectileScript.lane = lane; // Set the lane for the projectile
            projectileScript.damage = damage;
            projectileScript.speed = projectileSpeed; // Set speed or any other properties as needed
            projectileScript.knockbackForce = projectileSpeed / 3f; // Set knockback force
        }
    }
}
