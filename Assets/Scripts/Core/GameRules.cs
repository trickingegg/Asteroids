public enum AsteroidSize
{
    Large = 0,
    Medium = 1,
    Small = 2
}

/// <summary>
/// Constants and pure rules of Atari Asteroids (1979).
/// No Unity types so the logic can be unit-tested outside the editor.
/// </summary>
public static class GameRules
{
    public const int LargeAsteroidScore = 20;
    public const int MediumAsteroidScore = 50;
    public const int SmallAsteroidScore = 100;
    public const int LargeSaucerScore = 200;
    public const int SmallSaucerScore = 1000;

    public const int StartingLives = 3;
    public const int ExtraLifeEvery = 10000;
    public const int MaxDisplayedLives = 4;

    public const int StartingLargeAsteroids = 4;
    public const int LargeAsteroidsPerWaveStep = 2;
    public const int MaxLargeAsteroids = 10;
    public const int FragmentsPerSplit = 2;

    public const int MaxPlayerBullets = 4;
    public const float BulletLifetime = 1.2f;

    public const float HyperspaceFailChance = 0.25f;
    public const int SmallSaucerScoreThreshold = 40000;

    public static int ScoreForAsteroid(AsteroidSize size)
    {
        switch (size)
        {
            case AsteroidSize.Large:
                return LargeAsteroidScore;
            case AsteroidSize.Medium:
                return MediumAsteroidScore;
            case AsteroidSize.Small:
                return SmallAsteroidScore;
            default:
                return 0;
        }
    }

    public static bool TryGetChildSize(AsteroidSize size, out AsteroidSize childSize)
    {
        if (size == AsteroidSize.Large)
        {
            childSize = AsteroidSize.Medium;
            return true;
        }

        if (size == AsteroidSize.Medium)
        {
            childSize = AsteroidSize.Small;
            return true;
        }

        childSize = size;
        return false;
    }

    public static int LargeAsteroidsForWave(int waveIndex)
    {
        if (waveIndex < 0)
            waveIndex = 0;

        int count = StartingLargeAsteroids + waveIndex * LargeAsteroidsPerWaveStep;
        if (count > MaxLargeAsteroids)
            count = MaxLargeAsteroids;
        return count;
    }

    public static int ExtraLivesGained(int scoreBefore, int scoreAfter)
    {
        if (scoreAfter < scoreBefore)
            return 0;
        return (scoreAfter / ExtraLifeEvery) - (scoreBefore / ExtraLifeEvery);
    }

    public static float MinSpeed(AsteroidSize size)
    {
        switch (size)
        {
            case AsteroidSize.Large:
                return 0.6f;
            case AsteroidSize.Medium:
                return 1.1f;
            case AsteroidSize.Small:
                return 1.6f;
            default:
                return 0.6f;
        }
    }

    public static float MaxSpeed(AsteroidSize size)
    {
        switch (size)
        {
            case AsteroidSize.Large:
                return 1.4f;
            case AsteroidSize.Medium:
                return 2.2f;
            case AsteroidSize.Small:
                return 3.2f;
            default:
                return 1.4f;
        }
    }

    /// <summary>
    /// Toroidal wrap used by the original: leaving one edge continues from the opposite edge
    /// with the same overflow, so motion stays continuous.
    /// </summary>
    public static void Wrap(ref float x, ref float y, float minX, float minY, float maxX, float maxY)
    {
        float width = maxX - minX;
        float height = maxY - minY;
        if (width <= 0f || height <= 0f)
            return;

        while (x < minX)
            x += width;
        while (x > maxX)
            x -= width;
        while (y < minY)
            y += height;
        while (y > maxY)
            y -= height;
    }
}
