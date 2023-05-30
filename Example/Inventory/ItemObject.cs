using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class ItemObject : MonoBehaviour,IDragHandler,IDropHandler,IInitializePotentialDragHandler,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{   
    private Item _item = null;
    public int _itemCount = 0;
    private Image[] _images;
    private Text _text;
    private RectTransform _position;
    private Vector3 _startPosition;
    public List<ItemObject> itemObjectsToSwap = new List<ItemObject>();
    private InputReader inputReader;
    private InventorySystem inventorySystem;
   
    private void Awake()
    {
        inventorySystem = FindObjectOfType<InventorySystem>();
        inputReader = FindObjectOfType<InputReader>();
        _images = GetComponentsInChildren<Image>();
        _text = GetComponentInChildren<Text>();
        _position = GetComponent<RectTransform>();
        _startPosition = _position.anchoredPosition;
    }
    private void OnEnable()
    {
        inventorySystem.OnCloseInventoryEvent += DisabaleOutline;
    }
    private void OnDestroy()
    {
        inventorySystem.OnCloseInventoryEvent -= DisabaleOutline;
    }
    private void Start()
    {
        itemObjectsToSwap.Clear();
    }
    public Item GetItem()
    {
        return _item;
    }
    public void AddItem(Item item, int count)
    {
        if(item == null)
        {
            ResetItemObject();
        }
        else
        {
            _item = item;
            _itemCount = count;
            _images[0].enabled = true;
            _images[0].sprite = _item.icon;
            _text.text = _itemCount.ToString();
        }
      
    }
    public bool HaveItem()
    {
        return _item != null;
    }
    public void ChangeItemCount(int count)
    {
        _itemCount += count;
        _text.text = _itemCount.ToString();
    }
    public int GetItemCount()
    {
        return _itemCount;
    }
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        itemObjectsToSwap.Clear();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_position, eventData.position, eventData.pressEventCamera, out var mausePosition))
        {
            _position.position = mausePosition;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        SearchForClosestItemObjectToSwap(itemObjectsToSwap,out ItemObject itemObjectToSwap);
        SwapItemObjects(itemObjectToSwap);
        _position.anchoredPosition = _startPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Draggable") return;
        
            if(!itemObjectsToSwap.Contains(collision.GetComponent<ItemObject>()))
            {
                itemObjectsToSwap.Add(collision.GetComponent<ItemObject>());
            }
    }
    private void SearchForClosestItemObjectToSwap(List<ItemObject> itemObjects,out ItemObject itemObject)
    {
        itemObject = null;
        if (itemObjects.Count == 0) return;

        float distance = float.PositiveInfinity;
        for (int i = 0; i < itemObjects.Count; i++)
        {
           if(distance > Vector3.Distance(transform.position, itemObjects[i].transform.position) && Vector3.Distance(transform.position, itemObjects[i].transform.position) < 50)
            { 
                distance = Vector3.Distance(transform.position, itemObjects[i].transform.position);
                itemObject = itemObjects[i];
            }
        }
        itemObjectsToSwap.Clear();

    }
    private void SwapItemObjects(ItemObject itemObjectToSwap)
    {
        if (itemObjectToSwap == null) return;

            if(itemObjectToSwap.GetItem() == _item)
            {
                itemObjectToSwap.ChangeItemCount(_itemCount);
                ResetItemObject();
            }
            else
            {
                Item tempItem = null;
                int tempCount = 0;
                tempItem = itemObjectToSwap.GetItem();
                tempCount = itemObjectToSwap.GetItemCount();
                itemObjectToSwap.AddItem(_item, _itemCount);
                AddItem(tempItem, tempCount);
                itemObjectToSwap._position.anchoredPosition = itemObjectToSwap._startPosition;
            }
    }
    public void ResetItemObject()
    {
        _item = null;
        _images[0].enabled = false;
        _images[0].sprite = null;
        _text.text = " ";
        _itemCount = 0;
        _images[1].enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (!inputReader.IsShiftPressed) return;
        if (!inventorySystem.chest.IsActive) return;

        if (GetComponentInParent<Inventory>() != null)
        {
            inventorySystem.FindSlotForItem(_item, _itemCount, inventorySystem.chest.GetItemsObjects());
        }
        else
        {
            inventorySystem.FindSlotForItem(_item, _itemCount, inventorySystem.inventory.GetItemsObjects());
        }
        ResetItemObject(); 
    }
    public void DisabaleOutline()
    {
        _images[1].enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _images[1].enabled = true;
        _images[1].color = RarityColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _images[1].enabled = false;
    }
    private Color RarityColor()
    {
        if(_item?.rarity == Item.Rarity.epic)
        {
            return Color.magenta;
        }
        if (_item?.rarity == Item.Rarity.rare)
        {
            return Color.green;
        }
        if (_item?.rarity == Item.Rarity.legendary)
        {
            return Color.yellow;
        }
        if(_item?.rarity == Item.Rarity.common)
        {
            return Color.blue;
        }
        return Color.white;
    }
}
