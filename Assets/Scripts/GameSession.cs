using Asteroids.Score;
using UnityEngine;

public static class GameSession
{
    public const string AsteroidLayerName = "Asteroid";

    public static bool IsGameOver { get; private set; }

    public static int AsteroidLayer
    {
        get { return LayerMask.NameToLayer(AsteroidLayerName); }
    }

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
        ConfigurePhysics();
    }

    public static void ConfigurePhysics()
    {
        Physics2D.gravity = Vector2.zero;
        int asteroidLayer = AsteroidLayer;
        if (asteroidLayer >= 0)
            Physics2D.IgnoreLayerCollision(asteroidLayer, asteroidLayer, true);
    }

    public static void ApplyAsteroidCollisionSetup(GameObject asteroid)
    {
        if (asteroid == null)
            return;

        int asteroidLayer = AsteroidLayer;
        if (asteroidLayer >= 0)
            asteroid.layer = asteroidLayer;

        Collider2D collider = asteroid.GetComponent<Collider2D>();
        if (collider != null)
            collider.isTrigger = false;
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
