using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public const string SceneName = "MainMenu";

    [SerializeField] private TMPro.TextMeshProUGUI controlSchemeLabel;

    private void Awake()
    {
        Time.timeScale = 1f;
        PauseMenu.Paused = false;
        RefreshControlLabel();
    }

    public void NewGame()
    {
        GameSession.Reset();
        SceneManager.LoadScene(GameSession.GameSceneName);
    }

    public void ToggleControls()
    {
        GameSettings.ToggleMouseAim();
        RefreshControlLabel();
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    private void RefreshControlLabel()
    {
        if (controlSchemeLabel != null)
            controlSchemeLabel.text = GameSettings.ControlSchemeLabel();
    }
}
