using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotAmount;
    public GameObject[] itemPrefabs;

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
        // for (int i = 0; i < slotAmount; i++)
        // {
        //     Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        //     if(i < itemPrefabs.Length)
        //     {
        //         GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //         item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //         slot.currentItem = item;
        //     }
        // }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Inventory is full");
        return false;
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> inventoryData = new List<InventorySaveData>();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                
                inventoryData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
                
            }
        }
        return inventoryData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < slotAmount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotAmount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}
