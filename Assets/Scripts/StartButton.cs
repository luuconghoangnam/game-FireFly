using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(StartGame);
    }
    
    void StartGame()
    {
        // Thêm âm thanh nếu có
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClickSound();
        
        // Hiển thị UI gameplay và ẩn menu
        UIManager.Instance.ShowGameplayUI();
        
        // Bắt đầu game
        GameManager.Instance.StartGame();
        
        // Không cần ẩn nút nữa vì đã ẩn cả panel
        // gameObject.SetActive(false);
    }
}
