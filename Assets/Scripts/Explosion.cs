using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // Reset animation
        if (animator != null)
            animator.Play(0);

        // Tự động vô hiệu hóa sau khi animation kết thúc
        Invoke("Disable", lifetime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}