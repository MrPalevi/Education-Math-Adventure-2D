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
    public AudioClip Jump;
    public AudioClip Run;
    public AudioClip Take;
    public AudioClip Cekpoint;
    public AudioClip Hit;
    public AudioClip TakeCoin;

    [Header("----------Audio Source Main Menu----------")]
    public AudioClip backgroundMainMenu;
    public AudioClip Welcome;
    public AudioClip button;



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
