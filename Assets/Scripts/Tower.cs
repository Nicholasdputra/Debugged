using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Tower : MonoBehaviour
{
    [Header("References To Other Scripts")]
    public HubScript mainHub;

    [Header("UI Elements")]
    public GameObject buttonPrefab;

    [Header("Switching On/Off Settings")]
    public int state;
    // 0 = Off, 1 = On
    public float switchCooldown;
    public float switchOnCost;

    [Header("Passive Drain Settings")]
    public Coroutine drainCoroutine;
    public int passiveDrain;
    public float drainCooldown;

    [Header("Attack Settings")]
    public Coroutine attackCoroutine;
    public int damage;
    public float range;
    public float attackCooldown;
    public float attackCost;

    void SwitchStates()
    {
        if (state == 0 && mainHub.currentCharge >= switchOnCost)
        {
            state = 1; // Switch On
            Debug.Log("Tower is now ON");
        }
        else if (state == 1)
        {
            state = 0; // Switch Off
            Debug.Log("Tower is now OFF");
        }
        else
        {
            Debug.Log("Not enough charge to switch states");
        }
    }

    void CheckForCharge()
    {
        if (mainHub.currentCharge <= 0)
        {
            if(state == 1)
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
                Debug.Log("Attacking with " + gameObject.name + ", deducting " + attackCost + " charge");
                mainHub.currentCharge -= attackCost;
                Debug.Log("Current Charge After Attack: " + mainHub.currentCharge);
                Attack();
                yield return new WaitForSeconds(attackCooldown);
            }
            else
            {
                Debug.Log("Not enough charge to attack");
                yield return null;
            }
        }
        attackCoroutine = null;
    }

    public IEnumerator DrainCoroutine()
    {
        while (state == 1)
        {
            Debug.Log("Passive drain of " + passiveDrain + " from " + gameObject.name);
            mainHub.currentCharge -= passiveDrain;
            Debug.Log("Current Charge: " + mainHub.currentCharge);
            yield return new WaitForSeconds(drainCooldown);
        }
    }

    protected abstract void Attack();

    //I want a function that can spawn a UI buttons, for switching on and off the tower
    public void SpawnUIButtons()
    {   
        Canvas canvas = GameObject.Find("OverlayCanvas").GetComponent<Canvas>();
        Vector3 screenPos = canvas.worldCamera.WorldToScreenPoint(transform.position);
        Vector3 worldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, canvas.planeDistance)); 
        
        GameObject button = Instantiate(buttonPrefab, canvas.transform);
        button.GetComponent<RectTransform>().position = worldPos;

        //add listeners or set properties for the button as needed
        button.GetComponent<Button>().onClick.AddListener(SwapState);
        Debug.Log("UI Button Spawned for Tower at " + transform.position);
    }

    public void SwapState()
    {
        if (state == 0)
        {
            Debug.Log("Tower is now ON");
            state = 1; // Switch On
            Debug.Log("Deducted " + switchOnCost + " for turning on " + gameObject.name);
            mainHub.currentCharge -= switchOnCost; // Deduct charge for switching on
            Debug.Log("Current Charge: " + mainHub.currentCharge);
        }
        else if (state == 1)
        {
            state = 0; // Switch Off
            Debug.Log("Tower is now OFF");
            Debug.Log("Current Charge: " + mainHub.currentCharge);
        }
    }
}
