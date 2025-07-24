using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FirstTimeLoading : MonoBehaviour
{
    public Slider progressBar;             // Referensi ke Slider (Progress Bar)
    public TextMeshProUGUI progressText;   // Referensi teks progress (opsional)
    public float minLoadingTime = 2f;      // Waktu minimum loading dalam detik
    public string nextSceneName = "MainMenu"; // Nama scene berikutnya (Main Menu)

    private void Start()
    {
        StartCoroutine(LoadSceneAsync(nextSceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Mulai memuat scene secara asynchronous
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            // Perbarui progress berdasarkan waktu minimum dan progress Unity
            float progress = Mathf.Clamp01((elapsedTime / minLoadingTime) * 0.5f + (operation.progress / 0.9f) * 0.5f);
            progressBar.value = progress;

            // Perbarui teks progress
            if (progressText != null)
            {
                progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            }

            elapsedTime += Time.deltaTime;

            // Jika progress Unity mencapai 90% dan waktu minimum selesai
            if (operation.progress >= 0.9f && elapsedTime >= minLoadingTime)
            {
                progressBar.value = 1f;
                progressText.text = "100%";
                yield return new WaitForSeconds(0.5f); // Memberikan waktu jeda sebelum perpindahan
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
