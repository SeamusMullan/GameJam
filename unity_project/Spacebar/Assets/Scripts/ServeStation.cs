using UnityEngine;

public class ServeStation : Station
{
    [Header("Serve Settings")]
    [SerializeField] private Transform serveCheckPoint;
    [SerializeField] private float serveRange = 2f;
    [SerializeField] private LayerMask customerLayer;

    protected override void OnInteractionComplete()
    {
        ServeNearestCustomer();
    }

    public override void OnInteract(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (inventory != null && inventory.HasItem())
        {
            base.OnInteract(player);
        }
        else
        {
            AudioManager.Instance?.PlaySoundOneShot("NoItem");
        }
    }

    private void ServeNearestCustomer()
    {
        Collider[] customers = Physics.OverlapSphere(serveCheckPoint.position, serveRange, customerLayer);

        if (customers.Length > 0)
        {
            Customer nearestCustomer = customers[0].GetComponent<Customer>();

            if (nearestCustomer != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    PlayerInventory inventory = player.GetComponent<PlayerInventory>();
                    if (inventory != null && inventory.HasItem())
                    {
                        ItemType heldItem = inventory.GetHeldItemType();
                        nearestCustomer.ServeItem(heldItem);
                        inventory.RemoveItem();
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (serveCheckPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(serveCheckPoint.position, serveRange);
        }
    }
}
