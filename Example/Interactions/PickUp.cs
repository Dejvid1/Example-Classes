using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : Interaction,ISavable
{
    [field:SerializeField] InteractionsManager.InteractionType interactionType;
    [SerializeField]  Item item;
    [SerializeField] int itemCount;
    private bool _isRaised;
    private void Update()
    {
        if (InteractionsManager.currentInteraction == this) return;
        
        DestroyInteractionUI();
        
    }

    public override void ActivateInteraction()
    {
        gameObject.SetActive(false);
        _isRaised = true;
        InteractionsManager.currentInteraction = null;
        InventorySystem.FindSlotForItem(item,itemCount,InteractionsManager.inventory.GetItemsObjects());
    }

    public override InteractionsManager.InteractionType ReturnInteractionType()
    {
        return interactionType;
    }

    public object CaptureState()
    {
        return _isRaised;
    }

    public void RestoreState(object state)
    {
        gameObject.SetActive(!(bool)state);

    }
}
