using UnityEngine;

public class SoalItemL7M2 : MonoBehaviour
{
    public string namaItem;  // Contoh: "Kotak", "Segitiga"

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TakeButtonL7M2.instance?.RegisterItem(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TakeButtonL7M2.instance?.UnregisterItem(this);
        }
    }

    public void AmbilItem()
    {
        RandomSoalL7M2 randomSoal = FindObjectOfType<RandomSoalL7M2>();
        if (randomSoal != null)
        {
            randomSoal.PeriksaJawaban(namaItem);
        }

        Destroy(gameObject);
    }
}
