using Asteroids.Score;
using UnityEngine;

public static class GameSession
{
    public static bool IsGameOver { get; private set; }

    public static void Reset()
    {
        IsGameOver = false;
        ScoreUI._score = 0;
        HealthUI.health = GameRules.StartingLives;
        Asteroid.ResetAliveCount();
        Bullet.ResetCounters();
        Saucer.ResetAliveCount();
        Time.timeScale = 1f;
        PauseMenu.Paused = false;
    }

    public static void AddScore(int points)
    {
        if (points <= 0)
            return;

        int scoreBefore = ScoreUI._score;
        ScoreUI._score += points;
        int extraLives = GameRules.ExtraLivesGained(scoreBefore, ScoreUI._score);
        if (extraLives > 0)
            HealthUI.health += extraLives;
    }

    public static void SetGameOver()
    {
        IsGameOver = true;
    }
}
