using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private TMPro.TextMeshProUGUI controlSchemeLabel;

    private void Awake()
    {
        HideOverlay();
        RefreshControlLabel();
    }

    private void Start()
    {
        HideOverlay();
        ApplyControlSchemeToPlayer();
        RefreshControlLabel();
    }

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

        HideOverlay();
        Time.timeScale = 1f;
        Paused = false;
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
        ApplyControlSchemeToPlayer();
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

    public void Quit()
    {
        GameSession.Reset();
        SceneManager.LoadScene(MainMenu.SceneName);
    }

    private void Pause()
    {
        if (continueButton != null)
            continueButton.SetActive(true);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        RefreshControlLabel();
        Time.timeScale = 0f;
        Paused = true;
    }

    private void HideOverlay()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (continueButton != null)
            continueButton.SetActive(true);
    }

    private void RefreshControlLabel()
    {
        if (controlSchemeLabel != null)
            controlSchemeLabel.text = GameSettings.ControlSchemeLabel();
    }

    private static void ApplyControlSchemeToPlayer()
    {
        ShipMovement movement = Object.FindObjectOfType<ShipMovement>();
        if (movement != null)
            movement.ApplyControlScheme();
    }
}
