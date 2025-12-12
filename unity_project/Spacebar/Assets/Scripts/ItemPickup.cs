using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [Header("Item Settings")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private string itemName;
    [SerializeField] private GameObject visualPrefab;

    [Header("Pickup Settings")]
    [SerializeField] private bool canBePickedUp = true;

    private bool isBeingLookedAt = false;

    void Start()
    {
        if (string.IsNullOrEmpty(itemName))
        {
            itemName = itemType.ToString();
        }
    }

    public void OnLookEnter()
    {
        isBeingLookedAt = true;
        InteractionPromptUI.ShowPrompt(GetInteractionPrompt());
    }

    public void OnLookExit()
    {
        isBeingLookedAt = false;
        InteractionPromptUI.HidePrompt();
    }

    public void OnInteract(GameObject player)
    {
        if (!canBePickedUp) return;

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory != null && !inventory.HasItem())
        {
            inventory.PickupItem(this);
            AudioManager.Instance?.PlaySoundOneShot("PickupItem");
            gameObject.SetActive(false);
        }
    }

    public void OnInteractEnd(GameObject player)
    {
    }

    public string GetInteractionPrompt()
    {
        return $"Press E to pickup {itemName}";
    }

    public void SetItemType(ItemType type)
    {
        itemType = type;
        itemName = type.ToString();
    }

    public ItemType GetItemType() => itemType;
    public string GetItemName() => itemName;
    public GameObject GetVisualPrefab() => visualPrefab;
}
