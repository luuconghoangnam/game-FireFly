using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    void Update()
    {
        // Di chuyển thẳng xuống (giống Enemy)
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Kiểm tra nếu đi ra khỏi màn hình
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPosition.y < -0.1f)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Vô hiệu hóa đạn
            other.gameObject.SetActive(false);

            // Cộng điểm
            GameManager.Instance.AddScore(1);

            // Vô hiệu hóa obstacle
            gameObject.SetActive(false);
        }
    }
}