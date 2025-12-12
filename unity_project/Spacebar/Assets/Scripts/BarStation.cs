using UnityEngine;

public class BarStation : Station
{
    [Header("Bar Settings")]
    [SerializeField] private ItemType itemToProduce = ItemType.Pint;
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private GameObject itemPrefab;

    protected override void OnInteractionComplete()
    {
        ProduceItem();
    }

    private void ProduceItem()
    {
        if (itemPrefab != null && itemSpawnPoint != null)
        {
            GameObject item = Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
            ItemPickup pickup = item.GetComponent<ItemPickup>();

            if (pickup != null)
            {
                pickup.SetItemType(itemToProduce);
            }

            AudioManager.Instance?.PlaySoundOneShot("ProduceItem");
        }
    }

    public void SetItemType(ItemType type)
    {
        itemToProduce = type;
        UpdateInteractionPrompt();
    }

    private void UpdateInteractionPrompt()
    {
        interactionPrompt = $"Hold E to make {itemToProduce}";
    }
}
