using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Tipe Item")]
    public string itemType = ""; 
    // ✅ Isi di Inspector: "Pencil", "Pen", "Book", dll.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ItemCollector.instance.ShowCollectButton(this.gameObject, itemType);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ItemCollector.instance.HideCollectButton();
        }
    }
}
