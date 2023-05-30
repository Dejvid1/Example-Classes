using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private ItemObject[] itemObjects;
    public bool IsActive { get; private set; }
    private void Awake()
    {
        itemObjects = GetComponentsInChildren<ItemObject>();
        gameObject.SetActive(false);
    }
    public ItemObject[] GetItemsObjects()
    {
        return itemObjects;
    }
    public void Open()
    {
        IsActive = true;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }
}
