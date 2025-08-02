using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f; // Tăng giá trị này để bắn chậm hơn
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string turnLeftParam = "TurnLeft";
    [SerializeField] private string turnRightParam = "TurnRight";

    [Header("Power-ups")]
    [SerializeField] private float normalFireRate = 0.5f;
    [SerializeField] private float rapidFireRate = 0.15f;
    [SerializeField] private GameObject shieldObject;

    private bool hasDoubleDamage = false;
    private bool hasShield = false;
    
    private float nextFireTime = 0f;
    private Rigidbody2D rb;
    private float horizontalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Nếu không được gán trong Inspector, tìm Animator trên GameObject
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!GameManager.Instance.IsGameActive())
            return;
        
        // Lấy input để xử lý animation    
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Cập nhật animation dựa trên hướng di chuyển
        UpdateAnimation(horizontalInput);
            
        // Sử dụng GetKeyDown thay vì GetKey để chỉ bắn một lần khi nhấn
        // hoặc nếu bạn muốn bắn liên tục khi giữ, giữ GetKey nhưng tăng fireRate
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // fireRate = 0.5f nghĩa là 2 viên/giây
        }
    }
    
    void UpdateAnimation(float horizontalInput)
    {
        // Kích hoạt animation dựa trên hướng di chuyển
        if (horizontalInput < -0.1f)
        {
            // Di chuyển sang trái
            animator.SetBool(turnLeftParam, true);
            animator.SetBool(turnRightParam, false);
        }
        else if (horizontalInput > 0.1f)
        {
            // Di chuyển sang phải
            animator.SetBool(turnLeftParam, false);
            animator.SetBool(turnRightParam, true);
        }
        else
        {
            // Đứng yên
            animator.SetBool(turnLeftParam, false);
            animator.SetBool(turnRightParam, false);
        }
    }
    
    void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive())
            return;
            
        // Handle movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * moveSpeed;
        rb.linearVelocity = movement;
        
        // Clamp position to screen bounds
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.05f, 0.95f);
        pos.y = Mathf.Clamp(pos.y, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
    
    void Shoot()
    {
        GameObject bullet = ObjectPooler.Instance.GetPooledObject("Bullet");
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            
            // Sử dụng hasDoubleDamage
            if (hasDoubleDamage)
            {
                // Tăng sát thương của đạn
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                    bulletScript.SetDamageMultiplier(2);
            }
            
            bullet.SetActive(true);
            AudioManager.Instance.PlayShootSound();
        }
    }
    
    public void ActivatePowerUp(PowerUpType type, float duration)
    {
        switch(type)
        {
            case PowerUpType.RapidFire:
                fireRate = rapidFireRate;
                Invoke("ResetFireRate", duration);
                break;
                
            case PowerUpType.DoubleDamage:
                hasDoubleDamage = true;
                Invoke("ResetDamage", duration);
                break;
                
            case PowerUpType.Shield:
                hasShield = true;
                shieldObject.SetActive(true);
                Invoke("ResetShield", duration);
                break;
        }
    }

    private void ResetFireRate()
    {
        fireRate = normalFireRate;
    }

    private void ResetDamage()
    {
        hasDoubleDamage = false;
    }

    private void ResetShield()
    {
        hasShield = false;
        shieldObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Obstacle")) && !hasShield)
        {
            GameManager.Instance.GameOver();
            AudioManager.Instance.PlayGameOverSound();
            gameObject.SetActive(false);
        }
        else if ((other.CompareTag("Enemy") || other.CompareTag("Obstacle")) && hasShield)
        {
            // Shield bảo vệ khỏi va chạm đầu tiên
            ResetShield();
        }
    }
}
