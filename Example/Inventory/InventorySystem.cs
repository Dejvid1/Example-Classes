using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [field: SerializeField] public GameObject _invetoryObject;
    [field: SerializeField] public  List<Item> listOfItems = new List<Item>();
    public Canvas canvas;
    public event Action OnCloseInventoryEvent;
    private bool _isOpen;
    public Inventory inventory { get; private set; }
    public Chest chest { get; private set; }
    private void Awake()
    {
        chest = FindObjectOfType<Chest>();
        inventory = FindObjectOfType<Inventory>();
    }
    public void OpenInventory()
    {
        _invetoryObject.gameObject.SetActive(true);
        _isOpen = true;
    }
    public void CloseInventory()
    {
        _invetoryObject.gameObject.SetActive(false);
        _isOpen = false;
        OnCloseInventoryEvent?.Invoke();
    }
    public bool IsOpen()
    {
        return _isOpen;
    }
    public void FindSlotForItem(Item item, int itemCount, ItemObject[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() != item) continue;
            
            items[i].ChangeItemCount(itemCount);
            return;
        }
        for (int i = 0; i < items.Length; i++)
        {
            
            if (!items[i].HaveItem())
            {
                items[i].AddItem(item,itemCount);
                return;
            }

            if (items[i].GetItem() != item) continue;

            items[i].AddItem(item, itemCount);
        }
    }
}
