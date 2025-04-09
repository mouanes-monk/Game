using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBox : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Vector3 originalPosition;
    private Collider boxCollider;
    private Rigidbody rb;

    private float dragZ = 0f; // Keep z consistent

    void Start()
    {
        boxCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        // If Rigidbody exists, disable it for dragging
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Save original Z plane once
        dragZ = transform.position.z;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;

        // Temporarily disable collider to prevent physics interference
        if (boxCollider != null) boxCollider.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.WorldToScreenPoint(transform.position).z));
        transform.position = new Vector3(mousePosition.x, mousePosition.y, dragZ); // lock to original Z
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject slot = GetClosestSlot();
        if (slot != null)
        {
            transform.SetParent(slot.transform);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }

        // Re-enable collider
        if (boxCollider != null) boxCollider.enabled = true;
    }

    private GameObject GetClosestSlot()
    {
        GameObject closestSlot = null;
        float minDistance = float.MaxValue;
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Slot");

        foreach (GameObject slot in slots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance < minDistance && distance < 1.5f)
            {
                minDistance = distance;
                closestSlot = slot;
            }
        }

        return closestSlot;
    }
}
