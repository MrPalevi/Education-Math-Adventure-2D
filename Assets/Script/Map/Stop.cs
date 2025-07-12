using System.Collections;
using UnityEngine;

public class Stop : MonoBehaviour
{
    [Header("UI yang Akan Ditampilkan")]
    public GameObject uiToShow;

    [Header("Durasi Muncul UI")]
    public float displayDuration = 2f;

    private Coroutine currentCoroutine;

    private void Start()
    {
        if (uiToShow != null)
        {
            uiToShow.SetActive(false); // Pastikan UI tersembunyi saat awal
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && uiToShow != null)
        {
            // Jika sebelumnya ada coroutine aktif, hentikan
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Mulai ulang coroutine untuk menampilkan UI
            currentCoroutine = StartCoroutine(ShowUI());
        }
    }

    IEnumerator ShowUI()
    {
        uiToShow.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        uiToShow.SetActive(false);
        currentCoroutine = null;
    }
}
