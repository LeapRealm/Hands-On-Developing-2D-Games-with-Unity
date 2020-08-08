using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int damageInflicted;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other is BoxCollider2D)
        {
            var enemy = other.GetComponent<Enemy>();

            StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
            
            gameObject.SetActive(false);
        }
    }
}