using UnityEngine;

public static class GameSettings
{
    private const string MouseAimKey = "Asteroids.UseMouseAim";
    private static bool _loaded;
    private static bool _useMouseAim;

    public static bool UseMouseAim
    {
        get
        {
            EnsureLoaded();
            return _useMouseAim;
        }
        set
        {
            _useMouseAim = value;
            _loaded = true;
            PlayerPrefs.SetInt(MouseAimKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static void ToggleMouseAim()
    {
        UseMouseAim = !UseMouseAim;
    }

    public static string ControlSchemeLabel()
    {
        return UseMouseAim ? "мышь" : "клавиатура";
    }

    private static void EnsureLoaded()
    {
        if (_loaded)
            return;

        _loaded = true;
        _useMouseAim = PlayerPrefs.GetInt(MouseAimKey, 0) == 1;
    }
}
