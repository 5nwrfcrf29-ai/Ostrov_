using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Simple dictionary: ItemID -> Quantity
    public Dictionary<int, int> inventory = new Dictionary<int, int>();

    public bool AddItem(int itemID, int amount)
    {
        if (inventory.ContainsKey(itemID))
        {
            inventory[itemID] += amount;
        }
        else
        {
            inventory[itemID] = amount;
        }

        Debug.Log($"Picked up item ID: {itemID}. Total in inventory: {inventory[itemID]}");
        return true;
    }
}