using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public abstract class Tower : MonoBehaviour
{
    [Header("References To Other Objects")]
    public HubScript mainHub;
    public GameObject buttonPrefab;
    public Animator animator;

    [Header("Switching On/Off Settings")]
    public int state;
    // 0 = Off, 1 = On
    public float switchCooldown;
    public bool canSwitch = true;
    public float switchOnCost;

    [Header("Passive Drain Settings")]
    public Coroutine drainCoroutine;
    public int passiveDrain;
    public float drainCooldown;

    [Header("Attack Settings")]
    public Coroutine DelayFirstAttackCoroutine;
    public Coroutine attackCoroutine;
    public int damage;
    public float range;
    protected float attackCooldown;
    public float attackCost;

    public Coroutine turnOffAnimatorCoroutine; // Add this field
    public SpriteRenderer spriteRenderer;
    public Sprite baseOnSprite;
    public Sprite baseOffSprite;
    public Sprite hoverOnSprite;
    public Sprite hoverOffSprite;

    public void SwitchStates()
    {
        if (state == 0 && RequirementCheck() && canSwitch)
        {
            DelayFirstAttackCoroutine = StartCoroutine(StartAttacking());
            animator.enabled = true; // Enable the animator component
            state = 1; // Switch On
            animator.SetInteger("state", state); // Set the animator state to "On" 
            mainHub.currentLoad += 1; // Increment load when switching on
            mainHub.currentCharge -= switchOnCost; // Deduct charge for switching on
            StartCoroutine(SwitchCooldownCoroutine());
        }
        else if (state == 1 && canSwitch)
        {
            animator.enabled = true; // Enable the animator component
            mainHub.currentLoad -= 1; // Decrement load when switching off
            state = 0; // Switch Off
            animator.SetInteger("state", state); // Set the animator state to "Off"
            StartCoroutine(TurnOffAnimatorCoroutine());
            StartCoroutine(SwitchCooldownCoroutine());
        }
        else
        {
            // Debug.Log("Not enough charge to switch states");
        }
    }

    public IEnumerator TurnOffAnimatorCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        //set the animator component to inactive after the switch animation
        animator.enabled = false;
        spriteRenderer.sprite = (state == 1) ? baseOnSprite : baseOffSprite;
        turnOffAnimatorCoroutine = null; // Clear reference
    }
    
    public bool RequirementCheck()
    {
        if (mainHub.currentLoad < mainHub.maxLoad && mainHub.currentCharge >= switchOnCost)
        {
            // Debug.Log("Requirements fulfilled for switching on the tower.");
            return true;
        }
        else if (mainHub.currentLoad >= mainHub.maxLoad)
        {
            // Debug.Log("Cannot switch on tower: Load limit reached.");
            return false;
        }
        else if (mainHub.currentCharge < switchOnCost)
        {
            // Debug.Log("Cannot switch on tower: Not enough charge.");
            return false;
        }
        else
        {
            // Debug.Log("Unknown error in RequirementCheck.");
            // This should not happen, but just in case
            return false;
        }
    }

    public void CheckForCharge()
    {
        if (mainHub.currentCharge <= 0)
        {
            if (state == 1)
            {
                Debug.Log("Not enough charge to keep the tower ON, switching OFF");
                state = 0; // Switch Off
                animator.enabled = true; // Enable the animator component
                animator.SetInteger("state", state);
                StartCoroutine(TurnOffAnimatorCoroutine());
            }
        }
    }

    public IEnumerator StartAttacking()
    {
        yield return new WaitForSeconds(0.5f); // Wait for the initial delay
        attackCoroutine = StartCoroutine(AttackCoroutine());
        DelayFirstAttackCoroutine = null; // Clear reference
    }

    public IEnumerator AttackCoroutine()
    {
        while (state == 1)
        {
            if (mainHub.currentCharge >= attackCost)
            {
                mainHub.currentCharge -= attackCost;
                Attack();
                yield return new WaitForSeconds(attackCooldown);
            }
            else if (mainHub.currentCharge >= attackCost)
            {

            }
            else
            {
                yield return null;
            }
        }
        attackCoroutine = null;
    }

    public IEnumerator SwitchCooldownCoroutine()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchCooldown);
        canSwitch = true;
    }


    public IEnumerator DrainCoroutine()
    {
        while (state == 1)
        {
            mainHub.currentCharge -= passiveDrain;
            yield return new WaitForSeconds(drainCooldown);
        }
    }

    protected abstract void Attack();

    public void OnTowerMouseEnter()
    {
        spriteRenderer.sprite = (state == 1) ? hoverOnSprite : hoverOffSprite;

    }
    
    public void OnTowerMouseExit()
    {
        spriteRenderer.sprite = (state == 1) ? baseOnSprite : baseOffSprite;
    }
}