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
    GameObject button;
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

    void SwitchStates()
    {
        if (state == 0 && RequirementCheck() && canSwitch)
        {
            DelayFirstAttackCoroutine = StartCoroutine(StartAttacking());
            animator.enabled = true; // Enable the animator component
            state = 1; // Switch On
            animator.SetInteger("state", state); // Set the animator state to "On" 
            mainHub.currentLoad += 1; // Increment load when switching on
            mainHub.currentCharge -= switchOnCost; // Deduct charge for switching on
            // button.GetComponentInChildren<TextMeshProUGUI>().text = "Turn Off";
            StartCoroutine(SwitchCooldownCoroutine());
            // Debug.Log("Tower is now ON");
        }
        else if (state == 1 && canSwitch)
        {
            animator.enabled = true; // Enable the animator component
            mainHub.currentLoad -= 1; // Decrement load when switching off
            state = 0; // Switch Off
            animator.SetInteger("state", state); // Set the animator state to "Off"
            StartCoroutine(TurnOffAnimatorCoroutine());
            // button.GetComponentInChildren<TextMeshProUGUI>().text = "Turn On";
            StartCoroutine(SwitchCooldownCoroutine());
            // Debug.Log("Tower is now OFF");
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
                // canAttack = false; // Disable attack
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
                // Debug.Log("Attacking with " + gameObject.name + ", deducting " + attackCost + " charge");
                mainHub.currentCharge -= attackCost;
                // Debug.Log("Current Charge After Attack: " + mainHub.currentCharge);
                Attack();
                yield return new WaitForSeconds(attackCooldown);
            }
            else if (mainHub.currentCharge >= attackCost)
            {
                // Debug.Log("Cannot attack right now, waiting for cooldown");
            }
            else
            {
                // Debug.Log("Not enough charge to attack");
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
        // Debug.Log("Switch cooldown complete, can switch again");
    }


    public IEnumerator DrainCoroutine()
    {
        while (state == 1)
        {
            // Debug.Log("Passive drain of " + passiveDrain + " from " + gameObject.name);
            mainHub.currentCharge -= passiveDrain;
            // Debug.Log("Current Charge: " + mainHub.currentCharge);
            yield return new WaitForSeconds(drainCooldown);
        }
    }

    protected abstract void Attack();


    public void SpawnUIButtons()
    {
        Canvas canvas = transform.parent.Find("OverlayCanvas").GetComponent<Canvas>();
        Vector3 screenPos = canvas.worldCamera.WorldToScreenPoint(transform.position);
        Vector3 worldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, canvas.planeDistance));
        

        button = Instantiate(buttonPrefab, canvas.transform);

        button.GetComponent<RectTransform>().position = worldPos;

        button.GetComponent<Button>().onClick.AddListener(SwitchStates);
        // button.GetComponentInChildren<TextMeshProUGUI>().text = "Turn On";
        // Debug.Log("UI Button Spawned for Tower at " + transform.position);

        button.SetActive(false); // Initially hide the button
    }

    public void OnMouseEnter()
    {
        // Debug.Log("Pointer entered tower: " + gameObject.name);
        if (button != null)
        {
            button.SetActive(true);
            spriteRenderer.sprite = (state == 1) ? hoverOnSprite : hoverOffSprite;
        }
    }

    public void OnMouseOver()
    {
        if (button != null)
        {
            button.SetActive(true);
            spriteRenderer.sprite = (state == 1) ? hoverOnSprite : hoverOffSprite;
        }
    }

    public void OnMouseExit()
    {
        // Debug.Log("Pointer exited tower: " + gameObject.name);
        if (button != null)
        {
            button.SetActive(false);
            spriteRenderer.sprite = (state == 1) ? baseOnSprite : baseOffSprite;
        }
    }
}
