using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static Vector3 lastCheckPointPos = new Vector3(-5f, 0f, 0f);
    public static bool isGameOver;

    public GameObject gameOverScreen;
    public GameObject panelPause;

    private void Awake()
    {
        isGameOver = false;
    }

    void Start()
    {
        Time.timeScale = 1f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 spawnPos = lastCheckPointPos;
        if (spawnPos.x < 0) spawnPos.x = 0f;

        if (spawnPos != Vector3.zero)
        {
            player.transform.position = spawnPos;
        }
        else
        {
            Debug.Log("Mulai dari posisi default karena belum ada checkpoint.");
        }
    }


    void Update()
    {
        if (isGameOver)
        {
            if (gameOverScreen != null)
                gameOverScreen.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        if (panelPause != null)
            panelPause.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (panelPause != null)
            panelPause.SetActive(false);
    }

    public void LoadSceneName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
