using UnityEngine;

public class VendingMachineStation : Station
{
    [Header("Vending Machine Settings")]
    [SerializeField] private int tipsRequired = 5;
    [SerializeField] private ItemType specialItem = ItemType.Monster;
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private GameObject itemPrefab;

    void Start()
    {
        UpdateInteractionPrompt();
    }

    protected override void Update()
    {
        base.Update();
        UpdateInteractionPrompt();
    }

    public override void OnInteract(GameObject player)
    {
        if (GameManager.Instance != null && GameManager.Instance.GetTips() >= tipsRequired)
        {
            base.OnInteract(player);
        }
        else
        {
            AudioManager.Instance?.PlaySoundOneShot("CantAfford");
        }
    }

    protected override void OnInteractionComplete()
    {
        if (GameManager.Instance != null && GameManager.Instance.GetTips() >= tipsRequired)
        {
            GameManager.Instance.AddTips(-tipsRequired);
            ProduceItem();
        }
    }

    private void ProduceItem()
    {
        if (itemPrefab != null && itemSpawnPoint != null)
        {
            GameObject item = Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
            ItemPickup pickup = item.GetComponent<ItemPickup>();

            if (pickup != null)
            {
                pickup.SetItemType(specialItem);
            }

            AudioManager.Instance?.PlaySoundOneShot("VendingMachine");
        }
    }

    private void UpdateInteractionPrompt()
    {
        int currentTips = GameManager.Instance != null ? GameManager.Instance.GetTips() : 0;
        interactionPrompt = $"Vending Machine - {tipsRequired} tips ({currentTips} available)";
    }
}
