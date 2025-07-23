using UnityEngine;

public class BridgeOpen : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject bridge;           // ✅ Jembatan yang akan diaktifkan
    public GameObject infoUI;           // ✅ UI informasi (aktif jika player sentuh box)

    [Header("Tag")]
    public string triggerAreaTag = "BridgeTrigger"; // ✅ Tag untuk area pemicu bridge
    public string playerTag = "Player";             // ✅ Tag untuk player

    private bool isPlayerTouchingBox = false;

    void Start()
    {
        if (bridge != null) bridge.SetActive(false);
        if (infoUI != null) infoUI.SetActive(false);
    }

    // ✅ UI informasi → aktif saat player sentuh box
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            isPlayerTouchingBox = true;
            if (infoUI != null) infoUI.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            isPlayerTouchingBox = false;
            if (infoUI != null) infoUI.SetActive(false);
        }
    }

    // ✅ Aktifkan bridge jika box menyentuh trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerAreaTag))
        {
            if (bridge != null)
            {
                bridge.SetActive(true);
                Debug.Log("✅ Box menyentuh trigger, Bridge aktif");
            }
        }
    }

    // ✅ Nonaktifkan bridge jika box keluar dari trigger area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(triggerAreaTag))
        {
            if (bridge != null)
            {
                bridge.SetActive(false);
                Debug.Log("❌ Box keluar trigger, Bridge nonaktif");
            }
        }
    }
}

