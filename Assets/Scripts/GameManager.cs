using UnityEngine;
using TMPro; // Veľmi dôležité pre ovládanie TextMeshPro cez kód!

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int KillsToWin = 10;        
    public GameObject victoryScreen;   
    public TextMeshProUGUI scoreText;  // SEM pretiahneme náš KillsText z Hierarchy

    private int _currentKills = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (victoryScreen != null) victoryScreen.SetActive(false);
        Time.timeScale = 1f;
        
        // Aktualizujeme text hneď na začiatku hry
        UpdateScoreUI();
    }

    public void AddKill()
    {
        _currentKills++;
        
        // Zakaždým, keď pribudne kill, prepíšeme text na obrazovke
        UpdateScoreUI();

        if (_currentKills >= KillsToWin)
        {
            WinGame();
        }
    }

    // Pomocná funkcia, ktorá prepíše text v UI
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Uspaní učitelia: " + _currentKills + " / " + KillsToWin;
        }
    }

    void WinGame()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
        }
        Time.timeScale = 0f; 
    }
}