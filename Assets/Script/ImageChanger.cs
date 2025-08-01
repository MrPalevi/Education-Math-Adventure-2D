using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    [Header("Target Image yang akan diubah (misal: profile image di banyak tempat)")]
    public Image[] imageTargets;

    [Header("Panel Pilihan Gambar")]
    public GameObject panelPotoImage;

    [Header("Tombol untuk Membuka Panel")]
    public Button editImageProfileButton;

    [Header("Daftar Sprite Pilihan dan Tombolnya")]
    public Sprite[] availableSprites;
    public Button[] spriteButtons;

    private const string prefsKey = "SelectedProfileImageIndex";

    void Start()
    {
        SetupEditButton();
        SetupSpriteButtons();
        LoadSavedProfileImage();
    }

    void SetupEditButton()
    {
        if (editImageProfileButton != null)
        {
            editImageProfileButton.onClick.AddListener(() =>
            {
                if (panelPotoImage != null)
                    panelPotoImage.SetActive(true);
            });
        }
    }

    void SetupSpriteButtons()
    {
        for (int i = 0; i < spriteButtons.Length; i++)
        {
            int index = i;
            if (spriteButtons[i] != null && i < availableSprites.Length)
            {
                // Set tampilan sprite button
                Image buttonImage = spriteButtons[i].GetComponent<Image>();
                if (buttonImage != null)
                    buttonImage.sprite = availableSprites[i];

                spriteButtons[i].onClick.AddListener(() =>
                {
                    ChangeProfileImage(index);
                    if (panelPotoImage != null)
                        panelPotoImage.SetActive(false);
                });
            }
        }
    }

    void LoadSavedProfileImage()
    {
        int savedIndex = PlayerPrefs.GetInt(prefsKey, 0);

        if (availableSprites != null && savedIndex >= 0 && savedIndex < availableSprites.Length)
        {
            Sprite chosenSprite = availableSprites[savedIndex];
            foreach (var img in imageTargets)
            {
                if (img != null)
                    img.sprite = chosenSprite;
            }
        }
    }

    void ChangeProfileImage(int index)
    {
        if (index >= 0 && index < availableSprites.Length)
        {
            Sprite selected = availableSprites[index];
            foreach (var img in imageTargets)
            {
                if (img != null)
                    img.sprite = selected;
            }

            PlayerPrefs.SetInt(prefsKey, index);
            PlayerPrefs.Save();
        }
    }
}