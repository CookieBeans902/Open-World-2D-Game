using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; set; }
    [Header("-----------Audio Source-------------")]
    public AudioSource music;
    public AudioSource SFX;
    [Header("-----------Audio Clips(Ambient)--------------")]
    public AudioClip mainMenuMusic;
    public AudioClip gameBackgroundMusic;
    [Header("-----------Audio Clips(SFX)--------------")]
    public AudioClip attackSword;
    public AudioClip attackProjectile;
    public AudioClip walk;
    public AudioClip death;
    public AudioClip playerInteract;
    public AudioClip skillSound;
    public AudioClip buttonClick;
    public AudioClip levelUp;
    public AudioClip EquipSound;
    public AudioClip questButtonOpen;
    public AudioClip buttonHover;
    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        music.clip = mainMenuMusic;
        music.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }
    public void InGameMusic()
    {
        music.clip = gameBackgroundMusic;
        music.Play();
    }
    public void ButtonSFX(AudioClip clip)
    {
        if (!SFX.isPlaying)
        {
            SFX.PlayOneShot(clip);
        }
    }
}
