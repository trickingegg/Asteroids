using System;

public static class GameRulesVerify
{
    private static int _failed;
    private static int _passed;

    public static int Main(string[] args)
    {
        AssertEqual(20, GameRules.ScoreForAsteroid(AsteroidSize.Large), "large score");
        AssertEqual(50, GameRules.ScoreForAsteroid(AsteroidSize.Medium), "medium score");
        AssertEqual(100, GameRules.ScoreForAsteroid(AsteroidSize.Small), "small score");

        AsteroidSize child;
        AssertTrue(GameRules.TryGetChildSize(AsteroidSize.Large, out child) && child == AsteroidSize.Medium, "large splits to medium");
        AssertTrue(GameRules.TryGetChildSize(AsteroidSize.Medium, out child) && child == AsteroidSize.Small, "medium splits to small");
        AssertTrue(!GameRules.TryGetChildSize(AsteroidSize.Small, out child), "small does not split");

        AssertEqual(4, GameRules.LargeAsteroidsForWave(0), "wave 0");
        AssertEqual(6, GameRules.LargeAsteroidsForWave(1), "wave 1");
        AssertEqual(8, GameRules.LargeAsteroidsForWave(2), "wave 2");
        AssertEqual(10, GameRules.LargeAsteroidsForWave(3), "wave 3");
        AssertEqual(10, GameRules.LargeAsteroidsForWave(20), "wave cap");
        AssertEqual(4, GameRules.LargeAsteroidsForWave(-5), "negative wave");

        AssertEqual(0, GameRules.ExtraLivesGained(0, 9999), "no extra life yet");
        AssertEqual(1, GameRules.ExtraLivesGained(9999, 10000), "cross 10k");
        AssertEqual(2, GameRules.ExtraLivesGained(0, 20000), "two extra lives");
        AssertEqual(0, GameRules.ExtraLivesGained(20000, 10000), "score drop");

        AssertWrap(-5.1f, 0f, 4.9f, 0f, "left wrap");
        AssertWrap(5.1f, 0f, -4.9f, 0f, "right wrap");
        AssertWrap(0f, -4.2f, 0f, 3.8f, "bottom wrap");
        AssertWrap(0f, 4.2f, 0f, -3.8f, "top wrap");
        AssertWrap(-5.2f, -4.3f, 4.8f, 3.7f, "corner wrap");

        AssertTrue(GameRules.MinSpeed(AsteroidSize.Medium) > GameRules.MinSpeed(AsteroidSize.Large), "medium faster than large");
        AssertTrue(GameRules.MinSpeed(AsteroidSize.Small) > GameRules.MinSpeed(AsteroidSize.Medium), "small faster than medium");

        Console.WriteLine("Passed: " + _passed + ", Failed: " + _failed);
        return _failed == 0 ? 0 : 1;
    }

    private static void AssertWrap(float x, float y, float expectedX, float expectedY, string name)
    {
        GameRules.Wrap(ref x, ref y, -5f, -4f, 5f, 4f);
        AssertTrue(Math.Abs(x - expectedX) < 0.0001f && Math.Abs(y - expectedY) < 0.0001f, name + " (" + x + "," + y + ")");
    }

    private static void AssertEqual(int expected, int actual, string name)
    {
        AssertTrue(expected == actual, name + " expected " + expected + " got " + actual);
    }

    private static void AssertTrue(bool condition, string name)
    {
        if (condition)
        {
            _passed++;
            Console.WriteLine("OK  " + name);
        }
        else
        {
            _failed++;
            Console.WriteLine("FAIL " + name);
        }
    }
}
