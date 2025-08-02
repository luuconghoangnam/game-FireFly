using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move downward
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        
        // Check if enemy is off screen
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPosition.y < -0.1f)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Thêm debug để kiểm tra
        Debug.Log("Collision with: " + other.tag);
        
        if (other.CompareTag("Bullet"))
        {
            // Disable bullet
            other.gameObject.SetActive(false);
            
            // Add score
            GameManager.Instance.AddScore(1);
            
            // Disable enemy
            gameObject.SetActive(false);
        }
    }

    // Thêm vào Enemy.cs
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
