using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider SFXslider;
    void Start()
    {
        if (!PlayerPrefs.HasKey("music"))
        {
            LoadAudio();
        }
        else
        {
            SetVolumes();
        }
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("music", volume);
    }
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        mixer.SetFloat("master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("master", volume);
    }
    public void SetSFXVolume()
    {
        float volume = SFXslider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }
    public void LoadAudio()
    {
        musicSlider.value = PlayerPrefs.GetFloat("music");
        masterSlider.value = PlayerPrefs.GetFloat("master");
        SFXslider.value = PlayerPrefs.GetFloat("SFX");
        SetVolumes();
    }
    public void SetVolumes()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }
}