using UnityEngine;
using UnityEngine.SceneManagement; // Dôležité pre reštartovanie scény

public class MenuController : MonoBehaviour
{
    // Táto funkcia znova načíta aktuálnu scénu od začiatku
    public void RestartGame()
    {
        // Pred načítaním musíme vrátiť čas na normal (1f), lebo GameManager ho pri výhre/prehre stopol na 0
        Time.timeScale = 1f; 
        
        // Znovu načíta scénu, ktorá je momentálne zapnutá
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("RESTART");
    }

    // Táto funkcia úplne vypne hru
    public void QuitGame()
    {
        Debug.Log("KONIEC");
        Application.Quit();
    }
}