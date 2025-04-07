using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Item Hovered: " + item.name + " | Description: " + item.description + " | Type: " + item.type);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited from: " + item.name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            switch (item.type)
            {
                case ItemType.Weapon:
                    Debug.Log("Player attacks using " + item.name);
                    break;
                case ItemType.Armor:
                    Debug.Log("Player has put on " + item.name);
                    if (count > 1)
                    {
                        count--;
                        RefreshCount();
                    }
                    else
                    {
                        DestroyItem();
                    }
                    break;
                case ItemType.SpecialItem:
                    Debug.Log("Player drinks " + item.name);
                    if (count > 1)
                    {
                        count--;
                        RefreshCount();
                    }
                    else
                    {
                        DestroyItem();
                    }
                    break;
                default:
                    Debug.LogWarning("Unknown item type: " + item.name);
                    break;
            }
        }
    }

    private void DestroyItem()
    {
        Destroy(gameObject);
    }
}