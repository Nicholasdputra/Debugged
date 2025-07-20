using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableSlot : MonoBehaviour, IDropHandler
{
    RectTransform rectTransform;
    Vector3 worldPosition;
    Canvas canvas;
    Vector3 screenPos;
    Vector3 worldPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Logic to handle the drop event
        Debug.Log("Item dropped on slot");
        if (eventData.pointerDrag != null)
        {
            RectTransform draggedItemRect = eventData.pointerDrag.GetComponent<RectTransform>();
            draggedItemRect.position = rectTransform.position;

            Debug.Log("Dragged item: " + eventData.pointerDrag.name +
            " was dropped on slot: " + gameObject.name +
            " which belongs to column: " + transform.parent.name);

            // Convert UI position to world position
            screenPos = canvas.worldCamera.WorldToScreenPoint(rectTransform.position);
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

            // Instantiate the actual placeable item at the world position
            GameObject prefabToSpawn = eventData.pointerDrag.gameObject.GetComponent<PlaceableItemScript>().placeableItemPrefab;
            GameObject placeableItem = Instantiate(prefabToSpawn, worldPos, Quaternion.identity);
        }
    }
}
