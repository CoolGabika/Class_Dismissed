using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    // NOVÉ: Tu v Unity priradíme náš schovaný Panel
    public GameObject gameOverScreen; 

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        
        // Pre istotu sa uistíme, že obrazovka smrti je na začiatku vypnutá
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (healthSlider != null) healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("KONIEC HRY!");
        
        // ZAPNEME OBRAZOVKU SMRTI
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Zastavíme čas v hre
        Time.timeScale = 0f; 
        
        // Povolíme myš, aby si mohla kliknúť na tlačidlo Restart
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}