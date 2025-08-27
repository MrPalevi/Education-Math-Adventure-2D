using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        // Cek apakah PlayerPrefs untuk volume sudah ada
        if (!PlayerPrefs.HasKey("musicVolume") || !PlayerPrefs.HasKey("SFXVolume"))
        {
            // Jika belum ada, set ke default 1
            PlayerPrefs.SetFloat("musicVolume", 1f);
            PlayerPrefs.SetFloat("SFXVolume", 1f);
            PlayerPrefs.Save(); // Simpan perubahan
        }

        // Load nilai dari PlayerPrefs dan update mixer & slider
        LoadVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat("musicVolume");
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume");

        musicSlider.value = musicVol;
        SFXSlider.value = sfxVol;

        myMixer.SetFloat("music", Mathf.Log10(musicVol) * 20);
        myMixer.SetFloat("SFX", Mathf.Log10(sfxVol) * 20);
    }
}
