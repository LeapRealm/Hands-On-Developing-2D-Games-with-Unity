using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    public float maxHitPoints;
    public float startingHitPoints;

    public abstract void ResetCharacter();
    public abstract IEnumerator DamageCharacter(int damage, float interval);
    
    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
}