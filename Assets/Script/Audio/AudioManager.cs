using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("----------Audio Source----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------Audio Source Gameplay----------")]
    public AudioClip button;

    [Header("----------Audio Source Gameplay----------")]
    public AudioClip backgroundMainMenu;
    public AudioClip backgroundPlayGame;
    public AudioClip Jump;
    public AudioClip Run;
    public AudioClip Take;

    [Header("----------Audio Source Gameplay----------")]
    public AudioClip Welcome;
    public AudioClip MenuDiaolog1;
    public AudioClip MenuDiaolog2;
    public AudioClip MenuDiaolog3;
    public AudioClip MenuDiaolog4;
    public AudioClip MenuDiaolog5;
    public AudioClip MenuDiaolog6;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

   private void Start()
   {
    musicSource.clip = backgroundMainMenu;
    musicSource.Play();
   }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);

    }
}
