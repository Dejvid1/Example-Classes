using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour,ISavable
{
    private InventorySystem inventorySystem;
    public ItemObject[] itemObjects;
    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        itemObjects = GetComponentsInChildren<ItemObject>();
        gameObject.SetActive(false);
    }
    public ItemObject[] GetItemsObjects()
    {
        return itemObjects;
    }

    public object CaptureState()
    {
        int[] state = new int[40];
        for (int i = 0; i < 20; i++)
        {
            if (itemObjects[i].GetItem() == null)
            {
                state[i] = 0;
            }
            else
            {
                state[i] = itemObjects[i].GetItem().id;
                state[i + 20] = itemObjects[i].GetItemCount();
            }
        }
        return state;
    }

    public void RestoreState(object state)
    {
        int[] newState = (int[])state;
        for (int i = 0; i < 20; i++)
        {
            if (newState[i] == 0)
            {
                itemObjects[i].AddItem(null, 0);
            }
            else
            {
                itemObjects[i].AddItem(inventorySystem.listOfItems[newState[i] - 1], newState[i + 20]);
            }
        }
    }
}
