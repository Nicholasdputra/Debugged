using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public abstract class Tower : MonoBehaviour
{
    [Header("References To Other Scripts")]
    public HubScript mainHub;

    [Header("UI Elements")]
    GameObject button;
    public GameObject buttonPrefab;

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
    public Coroutine attackCoroutine;
    public int damage;
    public float range;
    protected float attackCooldown;
    public float attackCost;

    void SwitchStates()
    {
        if (state == 0 && RequirementCheck() && canSwitch)
        {
            mainHub.currentLoad += 1; // Increment load when switching on
            mainHub.currentCharge -= switchOnCost; // Deduct charge for switching on
            state = 1; // Switch On
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Turn Off";
            StartCoroutine(SwitchCooldownCoroutine());
            Debug.Log("Tower is now ON");
        }
        else if (state == 1 && canSwitch)
        {
            mainHub.currentLoad -= 1; // Decrement load when switching off
            state = 0; // Switch Off
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Turn On";
            StartCoroutine(SwitchCooldownCoroutine());  
            Debug.Log("Tower is now OFF");
        }
        else
        {
            Debug.Log("Not enough charge to switch states");
        }
    }
    
    public bool RequirementCheck()
    {
        if (mainHub.currentLoad < mainHub.maxLoad && mainHub.currentCharge >= switchOnCost)
        {
            Debug.Log("Requirements fulfilled for switching on the tower.");
            return true;
        }
        else if (mainHub.currentLoad >= mainHub.maxLoad)
        {
            Debug.Log("Cannot switch on tower: Load limit reached.");
            return false;
        }
        else if (mainHub.currentCharge < switchOnCost)
        {
            Debug.Log("Cannot switch on tower: Not enough charge.");
            return false;
        }
        else
        {
            Debug.Log("Unknown error in RequirementCheck.");
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
            }
        }
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
        Debug.Log("Switch cooldown complete, can switch again");
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
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Turn On";
        // Debug.Log("UI Button Spawned for Tower at " + transform.position);

        button.SetActive(false); // Initially hide the button
    }

    public void OnMouseEnter()
    {
        Debug.Log("Pointer entered tower: " + gameObject.name);
        if (button != null)
        {
            button.SetActive(true);
        }
    }

    public void OnMouseExit()
    {
        Debug.Log("Pointer exited tower: " + gameObject.name);
        if (button != null)
        {
            button.SetActive(false);
        }
    }
}
