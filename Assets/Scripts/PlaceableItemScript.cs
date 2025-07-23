using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceableItemScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public GameObject placeableItemPrefab;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = transform.parent.Find("BaseCanvas").GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found. Please ensure there is a Canvas in the scene.");
        }
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Logic to handle the beginning of a drag operation
        Debug.Log("Drag started");
        canvasGroup.alpha = 0.6f; // Make the item semi-transparent while dragging
        canvasGroup.blocksRaycasts = false; // Allow raycasts to pass through while dragging

    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("Dragging item");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Logic to handle the end of a drag operation
        Debug.Log("Drag ended");
        canvasGroup.alpha = 1f; // Restore full opacity
        canvasGroup.blocksRaycasts = true; // Restore raycast blocking

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Logic to handle pointer down event
        // Debug.Log("Pointer down on item");
    }
}
