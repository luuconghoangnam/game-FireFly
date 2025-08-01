using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move upward
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        // Check if bullet is off screen
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPosition.y > 1.1f)
        {
            gameObject.SetActive(false);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Tạo hiệu ứng nổ khi đạn va chạm với enemy hoặc obstacle
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            CreateExplosion();
            gameObject.SetActive(false);
        }
    }
    
    void CreateExplosion()
    {
        // Lấy một explosion từ object pool
        GameObject explosion = ObjectPooler.Instance.GetPooledObject("Explosion");
        if (explosion != null)
        {
            // Đặt explosion tại vị trí của đạn
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
        }
    }
}
