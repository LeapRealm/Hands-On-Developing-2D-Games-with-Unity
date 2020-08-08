﻿using UnityEngine;
using System.Collections;

public class Player : Character
{
    public HitPoints hitPoints;
    
    public HealthBar healthBarPrefab;
    private HealthBar healthBar;

    public Inventory inventoryPrefab;
    private Inventory inventory;

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

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            
            hitPoints.value -= damage;

            if (hitPoints.value <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    public override void KillCharacter()
    {
        base.KillCharacter();
        
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }

    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);
        
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
        
        hitPoints.value = startingHitPoints;
    }

    private void OnEnable()
    {
        ResetCharacter();
    }
}