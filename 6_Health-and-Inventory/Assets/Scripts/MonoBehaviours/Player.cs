using UnityEngine;

public class Player : Character
{
    public HealthBar healthBarPrefab;
    private HealthBar healthBar;

    public Inventory inventoryPrefab;
    private Inventory inventory;

    private void Start()
    {
        inventory = Instantiate(inventoryPrefab);
        
        hitPoints.value = startingHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CanBePickedUp"))
        {
            var itemInfo = other.gameObject.GetComponent<Consumable>().item;

            if (itemInfo != null)
            {
                var shouldDisappear = false;

                switch (itemInfo.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(itemInfo);
                        break;
                    
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(itemInfo.quantity);
                        break;
                }

                if (shouldDisappear)
                {
                    other.gameObject.SetActive(false);
                }
            }
        }
    }

    private bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value += amount;
            print("Adjusted HP by: " + amount + ". New value: " + hitPoints.value);

            return true;
        }

        return false;
    }
}