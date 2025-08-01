using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string turnLeftParam = "TurnLeft";
    [SerializeField] private string turnRightParam = "TurnRight";
    
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
            
        // Handle shooting input
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
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
            bullet.SetActive(true);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);
        }
    }
}
