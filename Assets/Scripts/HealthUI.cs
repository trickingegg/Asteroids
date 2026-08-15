using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static int health = GameRules.StartingLives;
    public Image Heart0;
    public Image Heart1;
    public Image Heart2;
    public Image Heart3;

    private void Update()
    {
        SetEnabled(Heart0, health >= 1);
        SetEnabled(Heart1, health >= 2);
        SetEnabled(Heart2, health >= 3);
        SetEnabled(Heart3, health >= 4);
    }

    private static void SetEnabled(Image image, bool enabled)
    {
        if (image != null)
            image.enabled = enabled;
    }
}
