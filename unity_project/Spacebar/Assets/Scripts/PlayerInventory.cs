using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private Transform itemHoldPoint;
    [SerializeField] private int maxItems = 1;

    private ItemPickup currentItem = null;
    private GameObject heldItemVisual = null;

    public bool HasItem()
    {
        return currentItem != null;
    }

    public void PickupItem(ItemPickup item)
    {
        if (currentItem != null || item == null) return;

        currentItem = item;
        ShowHeldItem();
    }

    public void RemoveItem()
    {
        if (currentItem == null) return;

        Destroy(currentItem.gameObject);
        currentItem = null;
        HideHeldItem();
    }

    public void DropItem()
    {
        if (currentItem == null) return;

        currentItem.gameObject.SetActive(true);
        currentItem.transform.position = transform.position + transform.forward;
        currentItem = null;
        HideHeldItem();
    }

    private void ShowHeldItem()
    {
        if (currentItem == null || itemHoldPoint == null) return;

        HideHeldItem();

        GameObject visualPrefab = currentItem.GetVisualPrefab();
        if (visualPrefab != null)
        {
            heldItemVisual = Instantiate(visualPrefab, itemHoldPoint.position, itemHoldPoint.rotation, itemHoldPoint);
        }
    }

    private void HideHeldItem()
    {
        if (heldItemVisual != null)
        {
            Destroy(heldItemVisual);
            heldItemVisual = null;
        }
    }

    public ItemType GetHeldItemType()
    {
        return currentItem != null ? currentItem.GetItemType() : ItemType.CrispSandwich;
    }

    public ItemPickup GetCurrentItem() => currentItem;
}
