using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : Interaction,ISavable
{
    [field: SerializeField] InteractionsManager.InteractionType interactionType;
    public List<Item> items = new List<Item>(20);
    public List<int> itemsCount = new List<int>(20);
    public bool isActive;
    public bool Opened;
    private void OnEnable()
    {
        InventorySystem.OnCloseInventoryEvent += HandleCloseInventory;
    }
    private void OnDisable()
    {
        InventorySystem.OnCloseInventoryEvent -= HandleCloseInventory;
    }
    private void Update()
    {
        if (InteractionsManager.currentInteraction != this)
        {
            DestroyInteractionUI();
        }
    }
    public override void ActivateInteraction()
    {
        if (isActive) return;
 
        if (!Opened)
        {
            GenerateItems();
            LoadItems();
            Opened = true;
        }
        else
        {
            LoadItems();
        }

        isActive = true;
    }
    private void HandleCloseInventory()
    {
        SearchForChange();
        InteractionsManager.chest.Close();
        isActive = false;
    }
    private void SearchForChange()
    {
        if (!isActive) return;

        for (int i = 0; i < 20; i++)
        {
            items[i] = InteractionsManager.chest.GetItemsObjects()[i].GetItem();
            itemsCount[i] = InteractionsManager.chest.GetItemsObjects()[i].GetItemCount();
        }

    }
    private void LoadItems()
    {
        if (!isActive) return;

        InteractionsManager.currentInteraction = null;
        InteractionsManager.chest.Open();
        InventorySystem.OpenInventory();
        for (int i = 0; i < 20; i++)
        {
            InteractionsManager.chest.GetItemsObjects()[i].AddItem(items[i], itemsCount[i]);
        }
    }

    public override InteractionsManager.InteractionType ReturnInteractionType()
    {
        return interactionType;
    }
    
    private void GenerateItems()
    {
        int amountOfItems = Random.Range(2,5);
        int chanceForItem;
        int random;
        for(int i = 0;i < amountOfItems; i++)
        {
            chanceForItem = (int)InventorySystem.listOfItems[Random.Range(1, InventorySystem.listOfItems.Count)].rarity;
            random = Random.Range(chanceForItem, 101);
            if(random <= chanceForItem)
            {
                i--;
            }
            else
            {
                int ItemPlace =  RandomPlaceForItem(20);
                items[ItemPlace] = InventorySystem.listOfItems[i];
                itemsCount[ItemPlace] = Random.Range(1,InventorySystem.listOfItems[i].maxSpawnedAmount);
            }
        }
    }
    
    private int RandomPlaceForItem(int lenght)
    {
        int randomPlace = Random.Range(0, lenght);
        while (items[randomPlace] != null)
        {
            randomPlace = Random.Range(0, lenght);
        }
        return randomPlace;
    }

    public object CaptureState()
    {
        if (!Opened) return null;

        int[] state = new int[40];
        for(int i = 0; i < 20; i++)
        {
            if (items[i] == null)
            {
                state[i] = 0;
            }
            else
            {
                state[i] = items[i].id;
                state[i + 20] = itemsCount[i];
            }
        }
        return state;
    }

    public void RestoreState(object state)
    {
        if (!Opened) return;

        int[] newState = (int[]) state;
        
        for(int i = 0; i < 20; i++)
        {
            if (newState[i] == 0)
            {
                items[i] = null;
            }
            else
            {
                items[i] = InventorySystem.listOfItems[newState[i] - 1];
                itemsCount[i] = newState[i + 20];
            }
        }
    }
}
