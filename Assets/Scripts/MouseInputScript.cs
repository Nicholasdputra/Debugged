using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MouseInputScript : MonoBehaviour
{
    private List<Tower> currentlyHoveredTowers = new List<Tower>();
    private List<Tower> previouslyHoveredTowers = new List<Tower>();
    [SerializeField] LayerMask excludedLayers;

    void Start()
    {
        // Create a mask that excludes "Enemy" and "Projectile" layers
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int projectileLayer = LayerMask.NameToLayer("Projectile");
        
        // Create the exclusion mask (all layers EXCEPT Enemy and Projectile)
        excludedLayers = ~((1 << enemyLayer) | (1 << projectileLayer));
    }

    void Update()
    {
        HandleTowerHovering();
        if(Input.GetMouseButtonDown(0))
        {
            foreach (Tower tower in currentlyHoveredTowers)
            {
                tower.SwitchStates();
            }
        }
    }

    void HandleTowerHovering()
    {
        currentlyHoveredTowers.Clear();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePos, excludedLayers);

        foreach (Collider2D hitCollider in hitColliders)
        {
            Tower tower = hitCollider.GetComponent<Tower>();
            if (tower != null)
            {
                currentlyHoveredTowers.Add(tower);

                if (!previouslyHoveredTowers.Contains(tower))
                {
                    // Mouse just entered this tower
                    tower.OnTowerMouseEnter();
                }
            }
        }

        foreach (Tower tower in previouslyHoveredTowers)
        {
            if (!currentlyHoveredTowers.Contains(tower))
            {
                tower.OnTowerMouseExit();
            }
        }

        previouslyHoveredTowers.Clear();
        previouslyHoveredTowers.AddRange(currentlyHoveredTowers);
    }
}