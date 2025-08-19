using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        HealthComponent healthComponent = other.GetComponentInParent<HealthComponent>();
        if (healthComponent != null) 
        healthComponent.TakeDamage(9999);
        
    }
}
