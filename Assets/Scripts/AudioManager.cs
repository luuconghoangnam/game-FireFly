using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip buttonClickSound;
    
    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    
    private AudioSource sfxSource;
    private AudioSource musicSource;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        // Tạo audio sources
        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        
        // Cài đặt music source
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
    }
    
    void Start()
    {
        PlayBackgroundMusic();
    }
    
    public void PlayShootSound() => PlaySFX(shootSound);
    public void PlayExplosionSound() => PlaySFX(explosionSound);
    public void PlayGameOverSound() => PlaySFX(gameOverSound);
    public void PlayButtonClickSound() => PlaySFX(buttonClickSound);
    
    public void PlayBackgroundMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }
    
    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }
    
    private void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}