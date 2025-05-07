using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    TrashcanController trashcanController;
    public int expOnDrop = 1; // You can set this per item in the Inspector

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        trashcanController = FindObjectOfType<TrashcanController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (eventData.pointerEnter == null)
        {
            Debug.LogWarning("PointerEnter is null. Dropping outside any slot.");
        }

        InventorySlot dropSlot = eventData.pointerEnter?.GetComponent<InventorySlot>();
        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<InventorySlot>();
            }
        }

        if (originalParent == null)
        {
            Debug.LogError("OriginalParent is null. Ensure OnBeginDrag is called before OnEndDrag.");
            return;
        }

        InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();

        if (dropSlot != null)
        {
            Debug.Log("Dropped on slot: " + dropSlot.name + " OriginalSlot: " + (originalSlot?.currentItem?.name ?? "None"));

            // Swap or move item
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            // Gain experience
            Experience_Manager expManager = FindObjectOfType<Experience_Manager>();
            if (expManager != null && dropSlot.name.Contains("TrashCanSlot"))
            {
                expManager.GainExperience(expOnDrop);
                StartCoroutine(DestroyAfterDelay(1f)); // Fixed coroutine call
            }
        }
        else
        {
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            Debug.Log("Dropped outside any slot. Reverting.");
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
