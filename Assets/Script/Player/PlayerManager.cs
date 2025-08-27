using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static Vector3 lastCheckPointPos = new Vector3(-5f, 0f, 0f);
    public static bool isGameOver;

    public GameObject gameOverScreen;
    public GameObject panelPause;
    public PlayerPositionSaver positionSaver;

    [Header("Restart Position")]
    public Transform spawnPoint; // drag object spawn point dari Inspector

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

    public void RestartTimeOver()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLv1()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L1M1");
        PlayerPrefs.DeleteKey("L1M2");
        PlayerPrefs.DeleteKey("L1M3");
        PlayerPrefs.DeleteKey("L1M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLv2()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv2");
        PlayerPrefs.DeleteKey("L2M1");
        PlayerPrefs.DeleteKey("L2M2");
        PlayerPrefs.DeleteKey("L2M3");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLv3()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L3M1");
        PlayerPrefs.DeleteKey("L3M2");
        PlayerPrefs.DeleteKey("L3M3");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLv4()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L4M1");
        PlayerPrefs.DeleteKey("L4M2");
        PlayerPrefs.DeleteKey("L4M3");
        PlayerPrefs.DeleteKey("L4M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLv5()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L5M1");
        PlayerPrefs.DeleteKey("L5M2");
        PlayerPrefs.DeleteKey("L5M3");
        PlayerPrefs.DeleteKey("L5M4");
        PlayerPrefs.DeleteKey("L5M5");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLv6()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L6M1");
        PlayerPrefs.DeleteKey("L6M2");
        PlayerPrefs.DeleteKey("L6M3");
        PlayerPrefs.DeleteKey("L6M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLv7()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L7M1");
        PlayerPrefs.DeleteKey("L7M2");
        PlayerPrefs.DeleteKey("L7M3");
        PlayerPrefs.DeleteKey("L7M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLv8()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L8M1");
        PlayerPrefs.DeleteKey("L8M2");
        PlayerPrefs.DeleteKey("L8M3");
        PlayerPrefs.DeleteKey("L8M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

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

    public void KembaliKeMainMenu()
    {
        positionSaver.SavePlayerPosition(); // Simpan posisi sebelum keluar
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        PlayerPrefs.Save();
    }

        public void MainMenuLv1()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L1M1");
        PlayerPrefs.DeleteKey("L1M2");
        PlayerPrefs.DeleteKey("L1M3");
        PlayerPrefs.DeleteKey("L1M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene("MainMenu");
    }

        public void MainMenuLv2()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv2");
        PlayerPrefs.DeleteKey("L2M1");
        PlayerPrefs.DeleteKey("L2M2");
        PlayerPrefs.DeleteKey("L2M3");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void MainMenuLv3()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L3M1");
        PlayerPrefs.DeleteKey("L3M2");
        PlayerPrefs.DeleteKey("L3M3");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void MainMenuLv4()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L4M1");
        PlayerPrefs.DeleteKey("L4M2");
        PlayerPrefs.DeleteKey("L4M3");
        PlayerPrefs.DeleteKey("L4M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void MainMenuLv5()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L5M1");
        PlayerPrefs.DeleteKey("L5M2");
        PlayerPrefs.DeleteKey("L5M3");
        PlayerPrefs.DeleteKey("L5M4");
        PlayerPrefs.DeleteKey("L5M5");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void MainMenuLv6()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L6M1");
        PlayerPrefs.DeleteKey("L6M2");
        PlayerPrefs.DeleteKey("L6M3");
        PlayerPrefs.DeleteKey("L6M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void MainMenuLv7()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L7M1");
        PlayerPrefs.DeleteKey("L7M2");
        PlayerPrefs.DeleteKey("L7M3");
        PlayerPrefs.DeleteKey("L7M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void MainMenuLv8()
    {
        Time.timeScale = 1f;
        positionSaver.ClearSavedPosition();
        CheckPoint.ResetCheckpointInCurrentScene();
        PlayerPrefs.DeleteKey("TotalFinalScoreLv1");
        PlayerPrefs.DeleteKey("L8M1");
        PlayerPrefs.DeleteKey("L8M2");
        PlayerPrefs.DeleteKey("L8M3");
        PlayerPrefs.DeleteKey("L8M4");
        PlayerPrefs.Save();

        if (spawnPoint != null)
        {
            PlayerManager.lastCheckPointPos = spawnPoint.position;
            Debug.Log($"Posisi awal di-set ke {spawnPoint.position}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint belum di-assign di Inspector!");
        }

        SceneManager.LoadScene("MainMenu");
    }
}
