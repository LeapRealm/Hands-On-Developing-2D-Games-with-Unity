using UnityEngine;

public class Player : Character
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CanBePickedUp"))
        {
            var itemInfo = other.gameObject.GetComponent<Consumable>().item;

            if (itemInfo != null)
            {
                print("Hit: " + itemInfo.objectName);

                switch (itemInfo.itemType)
                {
                    case Item.ItemType.COIN:
                        break;
                    
                    case Item.ItemType.HEALTH:
                        AdjustHealthPoints(itemInfo.quantity);
                        break;
                }
                
                other.gameObject.SetActive(false);
            }
        }
    }

    private void AdjustHealthPoints(int amount)
    {
        healthPoints += amount;
        print("Adjusted healthPoints by: " + amount + ". New value: " + healthPoints);
    }
}