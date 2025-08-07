using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage. Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
    public void Initialize(float HP)
    {
        currentHealth = HP;
        maxHealth = HP;
    }
}
