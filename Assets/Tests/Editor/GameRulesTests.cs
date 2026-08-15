using NUnit.Framework;

public class GameRulesTests
{
    [Test]
    public void ScoresMatchOriginalAsteroids()
    {
        Assert.AreEqual(20, GameRules.ScoreForAsteroid(AsteroidSize.Large));
        Assert.AreEqual(50, GameRules.ScoreForAsteroid(AsteroidSize.Medium));
        Assert.AreEqual(100, GameRules.ScoreForAsteroid(AsteroidSize.Small));
    }

    [Test]
    public void LargeSplitsIntoMedium_MediumIntoSmall_SmallDoesNotSplit()
    {
        AsteroidSize child;
        Assert.IsTrue(GameRules.TryGetChildSize(AsteroidSize.Large, out child));
        Assert.AreEqual(AsteroidSize.Medium, child);

        Assert.IsTrue(GameRules.TryGetChildSize(AsteroidSize.Medium, out child));
        Assert.AreEqual(AsteroidSize.Small, child);

        Assert.IsFalse(GameRules.TryGetChildSize(AsteroidSize.Small, out child));
    }

    [Test]
    public void WavesStartAtFourAndCapAtTen()
    {
        Assert.AreEqual(4, GameRules.LargeAsteroidsForWave(0));
        Assert.AreEqual(6, GameRules.LargeAsteroidsForWave(1));
        Assert.AreEqual(8, GameRules.LargeAsteroidsForWave(2));
        Assert.AreEqual(10, GameRules.LargeAsteroidsForWave(3));
        Assert.AreEqual(10, GameRules.LargeAsteroidsForWave(20));
        Assert.AreEqual(4, GameRules.LargeAsteroidsForWave(-5));
    }

    [Test]
    public void ExtraLifeEveryTenThousand()
    {
        Assert.AreEqual(0, GameRules.ExtraLivesGained(0, 9999));
        Assert.AreEqual(1, GameRules.ExtraLivesGained(9999, 10000));
        Assert.AreEqual(1, GameRules.ExtraLivesGained(0, 10000));
        Assert.AreEqual(2, GameRules.ExtraLivesGained(0, 20000));
        Assert.AreEqual(0, GameRules.ExtraLivesGained(10000, 19999));
        Assert.AreEqual(0, GameRules.ExtraLivesGained(20000, 10000));
    }

    [Test]
    public void WrapMovesThroughOppositeEdgeKeepingOverflow()
    {
        float x = -5.1f;
        float y = 0f;
        GameRules.Wrap(ref x, ref y, -5f, -4f, 5f, 4f);
        Assert.AreEqual(4.9f, x, 0.0001f);
        Assert.AreEqual(0f, y, 0.0001f);

        x = 5.1f;
        y = 0f;
        GameRules.Wrap(ref x, ref y, -5f, -4f, 5f, 4f);
        Assert.AreEqual(-4.9f, x, 0.0001f);

        x = 0f;
        y = -4.2f;
        GameRules.Wrap(ref x, ref y, -5f, -4f, 5f, 4f);
        Assert.AreEqual(3.8f, y, 0.0001f);

        x = 0f;
        y = 4.2f;
        GameRules.Wrap(ref x, ref y, -5f, -4f, 5f, 4f);
        Assert.AreEqual(-3.8f, y, 0.0001f);
    }

    [Test]
    public void WrapHandlesCornerLeavingBothAxes()
    {
        float x = -5.2f;
        float y = -4.3f;
        GameRules.Wrap(ref x, ref y, -5f, -4f, 5f, 4f);
        Assert.AreEqual(4.8f, x, 0.0001f);
        Assert.AreEqual(3.7f, y, 0.0001f);
    }

    [Test]
    public void SmallerAsteroidsAreFaster()
    {
        Assert.Greater(GameRules.MinSpeed(AsteroidSize.Medium), GameRules.MinSpeed(AsteroidSize.Large));
        Assert.Greater(GameRules.MinSpeed(AsteroidSize.Small), GameRules.MinSpeed(AsteroidSize.Medium));
        Assert.Greater(GameRules.MaxSpeed(AsteroidSize.Small), GameRules.MaxSpeed(AsteroidSize.Large));
    }
}
