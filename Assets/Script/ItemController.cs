using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [Header("Item Settings")]
    public string namaItem = "Nama Item";
    [TextArea(2, 4)] public string deskripsiItem = "Deskripsi item ini.";

    public Button buttonTakeUI; // Ini tetap ditarik dari Inspector (tombol tetap satu, tapi hanya aktif per item)
    
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
        // Tombol aktif hanya jika player masih di dalam area dan item belum diambil
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
        }
    }

    void OnTakeButtonClicked()
    {
        if (sudahDiambil || !playerInRange) return;

        sudahDiambil = true;

        // Sembunyikan tombol
        if (buttonTakeUI != null)
            buttonTakeUI.gameObject.SetActive(false);

        // Tampilkan info ke panel
        if (itemManager != null)
            itemManager.ShowItemInfo(namaItem, deskripsiItem);

        // Nonaktifkan item ini
        gameObject.SetActive(false);

        Debug.Log($"Item diambil: {namaItem}");
    }
}
