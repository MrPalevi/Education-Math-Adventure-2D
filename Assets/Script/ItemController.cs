using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [Header("Item Settings")]
    public string namaItem = "Nama Item";
    [TextArea(2, 4)] public string deskripsiItem = "Deskripsi item ini.";

    public Button buttonTakeUI;

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip takeSound;     // Suara saat item diambil

    private bool sudahDiambil = false;
    private ItemManager itemManager;
    private bool playerInRange = false;

    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();

        if (buttonTakeUI != null)
        {
            buttonTakeUI.gameObject.SetActive(false);
            buttonTakeUI.onClick.AddListener(OnTakeButtonClicked);
        }
    }

    void Update()
    {
        if (buttonTakeUI != null)
            buttonTakeUI.gameObject.SetActive(playerInRange && !sudahDiambil);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !sudahDiambil)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (!sudahDiambil && buttonTakeUI != null)
                buttonTakeUI.gameObject.SetActive(false);
        }
    }

    void OnTakeButtonClicked()
    {
        if (sudahDiambil || !playerInRange) return;

        sudahDiambil = true;

        // Mainkan audio jika tersedia
        if (audioSource != null && takeSound != null)
        {
            audioSource.PlayOneShot(takeSound);
        }

        // Sembunyikan tombol
        if (buttonTakeUI != null)
            buttonTakeUI.gameObject.SetActive(false);

        // Kirim ke item manager
        if (itemManager != null)
            itemManager.ShowItemInfo(namaItem, deskripsiItem);

        // Nonaktifkan item (diberi delay agar suara bisa terdengar dulu)
        StartCoroutine(DisableAfterSound());

        Debug.Log($"Item diambil: {namaItem}");
    }

    System.Collections.IEnumerator DisableAfterSound()
    {
        // Tunggu suara selesai kalau ada, lalu disable
        if (takeSound != null)
            yield return new WaitForSeconds(takeSound.length);
        
        gameObject.SetActive(false);
    }
}
