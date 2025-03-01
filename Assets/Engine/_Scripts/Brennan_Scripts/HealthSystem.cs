using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth = 3;

    public Image[] hearts; // UI hearts
    public Sprite fullHeart;
    public Sprite emptyHeart;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UIManager.Instance.UpdateHeartsUI(currentHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    void Die()
    {
        Debug.Log("Player has fuckin Died Yo!!");
    }
}
