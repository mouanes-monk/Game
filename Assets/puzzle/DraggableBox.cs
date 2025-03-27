using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBox : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Vector3 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Save original parent and position
        originalParent = transform.parent;
        originalPosition = transform.position;

        // Unparent the object to keep world position stable
        transform.SetParent(null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move box with the mouse (in world space)
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, originalPosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject slot = GetClosestSlot();
        if (slot != null)
        {
            // Snap to the slot
            transform.SetParent(slot.transform);
            transform.localPosition = Vector3.zero;
        }
        else
        {
            // Return to original position if no valid slot found
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }
    }

    private GameObject GetClosestSlot()
    {
        GameObject closestSlot = null;
        float minDistance = float.MaxValue;

        // Find all slots in the scene
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Slot");

        foreach (GameObject slot in slots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance < minDistance && distance < 1.5f) // Adjust range as needed
            {
                minDistance = distance;
                closestSlot = slot;
            }
        }

        return closestSlot;
    }
}
