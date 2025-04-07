using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 20;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private int selectedSlot = -1;
    private List<InventoryItem> allItems = new List<InventoryItem>();
    private float cooldownTime = 60f;
    private float cooldownTimer = 0f;
    private bool hasSpawned = false;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        if (selectedSlot >= 0 && inventorySlots[selectedSlot].transform.childCount > 0)
        {
            InventoryItem selectedItem = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
            if (selectedItem != null)
            {
                if (!hasSpawned)
                {
                    Debug.Log("Item Selected: " + selectedItem.item.name + " | Description: " + selectedItem.item.description + " | Type: " + selectedItem.item.type);

                    hasSpawned = true;

                    cooldownTimer = cooldownTime;
                }
                else if (cooldownTimer <= 0f)
                {
                    Debug.Log("Item Selected: " + selectedItem.item.name + " | Description: " + selectedItem.item.description + " | Type: " + selectedItem.item.type);

                    cooldownTimer = cooldownTime;
                }
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        cooldownTimer = cooldownTime;
        hasSpawned = false;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
        allItems.Add(inventoryItem);
    }

    public void SortItemsByWeapon()
    {
        allItems.Sort((item1, item2) => item1.item.type.CompareTo(item2.item.type));
        RepositionItems();
        Debug.Log("Inventory sorted by Weapon (Weapon, Armor, SpecialItem)");
    }

    public void SortItemsByArmor()
    {
        allItems.Sort((item1, item2) =>
        {
            return item1.item.type == item2.item.type ?
                0 :
                (item1.item.type == ItemType.Armor ? -1 : (item1.item.type == ItemType.SpecialItem ? -1 : 1));
        });
        RepositionItems();
        Debug.Log("Inventory sorted by Armor (Armor, SpecialItem, Weapon)");
    }

    public void SortItemsBySpecialItem()
    {
        allItems.Sort((item1, item2) =>
        {
            return item1.item.type == item2.item.type ?
                0 :
                (item1.item.type == ItemType.SpecialItem ? -1 : (item1.item.type == ItemType.Weapon ? -1 : 1));
        });
        RepositionItems();
        Debug.Log("Inventory sorted by SpecialItem (SpecialItem, Weapon, Armor)");
    }

    private void RepositionItems()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            InventoryItem item = allItems[i];
            if (i < inventorySlots.Length)
            {
                item.transform.SetParent(inventorySlots[i].transform);
            }
        }
    }
}
