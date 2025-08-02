using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("General UI Elements")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePlayPanel;
    
    [Header("Gameplay UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button pauseButton;
    
    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button gameOverRestartButton;
    [SerializeField] private Button gameOverMainMenuButton;
    
    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseMainMenuButton;
    
    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
            
        // Ẩn tất cả các panel trừ main menu
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true); // Chỉ hiển thị menu chính ban đầu
    }
    
    void Start()
    {
        // Game Over Panel buttons
        if (gameOverRestartButton != null)
            gameOverRestartButton.onClick.AddListener(RestartGame);
        
        if (gameOverMainMenuButton != null)
            gameOverMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        
        // Pause Panel buttons
        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseGame);
        
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
            
        if (pauseMainMenuButton != null)
            pauseMainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    void Update()
    {
        // Kiểm tra phím Escape để pause/resume
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsGameActive())
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
    
    public void UpdateGameTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    public void ShowGameOverScreen(int score, int highScore)
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
        highScoreText.text = "High Score: " + highScore;
    }

    void RestartGame()
    {
        AudioManager.Instance.PlayButtonClickSound();
        
        // Đảm bảo thời gian game quay lại bình thường
        Time.timeScale = 1;
        isPaused = false;
        
        // Ẩn game over panel và hiển thị gameplay UI
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        
        // Bắt đầu game mới
        GameManager.Instance.StartGame();
    }

    void ReturnToMainMenu()
    {
        AudioManager.Instance.PlayButtonClickSound();
        
        // Đảm bảo thời gian game quay lại bình thường nếu đang pause
        Time.timeScale = 1;
        isPaused = false;
        
        // Ẩn tất cả panel khác và hiển thị main menu
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void PauseGame()
    {
        AudioManager.Instance.PlayButtonClickSound();
        Time.timeScale = 0;
        isPaused = true;
        pausePanel.SetActive(true);
    }

    void ResumeGame()
    {
        AudioManager.Instance.PlayButtonClickSound();
        Time.timeScale = 1;
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void ShowGameplayUI()
    {
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }
}
