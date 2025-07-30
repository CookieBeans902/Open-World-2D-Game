using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioManager Instance { get; set; }
    [Header("-----------Audio Source-------------")]
    public AudioSource music;
    public AudioSource SFX;
    [Header("-----------Audio Clips--------------")]
    public AudioClip backgroundMusic;
    public AudioClip attackSword;
    public AudioClip attackProjectile;
    public AudioClip walk;
    public AudioClip death;
    public AudioClip interact;
    public AudioClip skillSound;
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
}
