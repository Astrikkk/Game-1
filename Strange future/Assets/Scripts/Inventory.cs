using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject itemPrefab;
    public int slotCount = 10;

    private List<Item> items = new List<Item>();
    private List<GameObject> slots = new List<GameObject>();
    private Text itemInfoText;

    void Start()
    {
        // Створення слотів інвентаря
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);
            slots.Add(slot);
        }

        // Знаходження компоненти Text для відображення інформації про предмет
        itemInfoText = GameObject.Find("Item Info Text").GetComponent<Text>();
    }

    public bool AddItem(Item item)
    {
        // Додавання предмету до першого вільного слоту
        for (int i = 0; i < slots.Count; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            if (!slot.IsOccupied())
            {
                GameObject itemObj = Instantiate(itemPrefab, slot.transform);
                itemObj.GetComponent<Image>().sprite = item.sprite;
                slot.SetItem(item, itemObj);
                items.Add(item);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        // Видалення предмету зі списку інвентаря та його слоту
        items.Remove(item);
        for (int i = 0; i < slots.Count; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            if (slot.GetItem() == item)
            {
                Destroy(slot.GetItemObject());
                slot.SetItem(null, null);
                break;
            }
        }
    }

    public void ShowItemInfo(Item item)
    {
        // Відображення інформації про вибраний предмет
        itemInfoText.text = item.name + "\n\n" + item.description;
    }
}

public class Item
{
    public string name;
    public string description;
    public Sprite sprite;
}

public class Slot : MonoBehaviour
{
    private Item item;
    private GameObject itemObj;

    public bool IsOccupied()
    {
        return item != null;
    }

    public Item GetItem()
    {
        return item;
    }

    public GameObject GetItemObject()
    {
        return itemObj;
    }

    public void SetItem(Item newItem, GameObject newItemObj)
    {
        item = newItem;
        itemObj = newItemObj;
    }
}
