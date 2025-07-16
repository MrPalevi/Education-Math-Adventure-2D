using UnityEngine;

public class CallPanelUI : MonoBehaviour
{
    [Header("Referensi ke ResultManager")]
    public ResultManager resultManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (resultManager != null)
            {
                resultManager.ShowResult(); // Memanggil UI hasil
                Debug.Log("Level Complete: Menampilkan Panel Result");
            }
            else
            {
                Debug.LogWarning("ResultManager belum di-assign pada CallPanelUI.");
            }
        }
    }
}
