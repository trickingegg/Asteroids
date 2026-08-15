using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;
    public static bool Controlls = true;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject Controlls1;
    [SerializeField] private GameObject Controlls2;
    [SerializeField] private GameObject continueButton;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (GameSession.IsGameOver)
            return;

        if (Paused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        if (GameSession.IsGameOver)
            return;

        if (continueButton != null)
            continueButton.SetActive(true);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    private void Pause()
    {
        if (continueButton != null)
            continueButton.SetActive(true);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void ShowGameOver()
    {
        if (continueButton != null)
            continueButton.SetActive(false);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void NewGame()
    {
        GameSession.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        Paused = false;
    }

    private void ControllsChange()
    {
        if (Controlls)
        {
            if (Controlls1 != null)
                Controlls1.SetActive(true);
            if (Controlls2 != null)
                Controlls2.SetActive(false);
            Controlls = false;
        }
        else
        {
            if (Controlls1 != null)
                Controlls1.SetActive(false);
            if (Controlls2 != null)
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
