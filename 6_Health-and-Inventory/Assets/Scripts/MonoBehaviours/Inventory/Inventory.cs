using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public const int numSlots = 5;
    
    private Image[] itemImages = new Image[numSlots];
    private Item[] items = new Item[numSlots];
    private GameObject[] slots = new GameObject[numSlots];

    private void Start()
    {
        CreateSlots();
    }

    private void CreateSlots()
    {
        if (slotPrefab != null)
        {
            for (var i = 0; i < numSlots; i++)
            {
                var newSlot = Instantiate(slotPrefab);
                
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(transform.GetChild(0));
                
                slots[i] = newSlot;
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        for (var i = 0; i < items.Length; i++)
        {
            // 기존 슬롯에 추가한다.
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable)
            {
                items[i].quantity++;

                var slotScript = slots[i].GetComponent<Slot>();
                var quantityText = slotScript.qtyText;

                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();

                return true;
            }

            // 아이템을 빈 슬롯에 추가한다.
            // 아이템을 복사해서 추가하므로
            // 스크립팅 가능한 오브젝트의 원본은 바뀌지 않는다.
            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;

                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;

                return true;
            }
        }
        
        return false;
    }
}