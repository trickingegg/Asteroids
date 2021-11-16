using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Asteroids.Score;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;
    public static bool Controlls = true;
    [SerializeReference] private GameObject pauseMenuUI;
    [SerializeReference] private GameObject Controlls1;
    [SerializeReference] private GameObject Controlls2;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void NewGame()
    {
        HealthUI.health = 4;
        ScoreUI._score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        Paused = false;
    }

    private void ControllsChange()
    {
        if (Controlls)
        {
            Controlls1.SetActive(true);
            Controlls2.SetActive(false);
            Controlls = false;
        }
        else
        {
            Controlls1.SetActive(false);
            Controlls2.SetActive(true);
            Controlls = true;
        }
            
        Time.timeScale = 0f;
        Paused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
