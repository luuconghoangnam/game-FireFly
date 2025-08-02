using UnityEngine;

public enum PowerUpType
{
    RapidFire,
    DoubleDamage,
    Shield
}

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType type;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float duration = 5f;
    
    void Update()
    {
        // Di chuy?n xu?ng
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        
        // Ki?m tra n?u ra kh?i màn hình
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPosition.y < -0.1f)
        {
            gameObject.SetActive(false);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Kích ho?t power-up
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ActivatePowerUp(type, duration);
                gameObject.SetActive(false);
            }
        }
    }
}