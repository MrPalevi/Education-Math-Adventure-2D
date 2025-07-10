using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool isGameOver;
    public static Vector2 lastCheckPointPos = new Vector2(-5, 0);
    public GameObject gameOverScreen;
    public GameObject panelPause;

    private void Awake()
    {
        isGameOver = false;
        GameObject.FindGameObjectWithTag("Player").transform.position = lastCheckPointPos;
    }

    void Update()
    {
        if (isGameOver)
        {
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
        panelPause.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        panelPause.SetActive(false);
    }

}
