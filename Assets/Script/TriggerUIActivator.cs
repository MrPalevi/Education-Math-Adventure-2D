using UnityEngine;

public class TriggerUIActivator : MonoBehaviour
{
    [Header("UI yang akan ditampilkan")]
    public GameObject infoUI; // Assign panel UI dari inspector (misalnya canvas child atau image)
    public string playerTag = "Player";             // ✅ Tag untuk player

    private bool isPlayerTouchingBox = false;
        void Start()
    {
        if (infoUI != null) infoUI.SetActive(false);
    }
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
}
