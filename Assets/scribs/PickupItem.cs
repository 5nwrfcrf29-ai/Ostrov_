using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("Item Info")]
    public string itemName = "Wood";
    public int itemID = 1;
    public int quantity = 1;

    public string GetInteractionPrompt()
    {
        return $"Press [E] to pick up {itemName}";
    }

    public void Interact(PlayerMove player)
    {
        // Look for an inventory component on the player object
        if (player.TryGetComponent<PlayerInventory>(out PlayerInventory inventory))
        {
            bool added = inventory.AddItem(itemID, quantity);

            if (added)
            {
                // Destroy the item in the world once picked up
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("No PlayerInventory found on the player!");
        }
    }
}