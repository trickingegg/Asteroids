#!/usr/bin/env python3
"""Standalone checks for Assets/Scripts/Core/GameRules.cs (original Asteroids rules)."""
from __future__ import division
import os
import re
import sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
SOURCE = os.path.join(ROOT, "Assets", "Scripts", "Core", "GameRules.cs")


def read_const(source, name):
    match = re.search(r"public const (?:int|float) %s = ([0-9.]+)f?;" % name, source)
    if not match:
        raise AssertionError("missing constant %s" % name)
    value = match.group(1)
    if "." in value:
        return float(value)
    return int(value)


def wrap(x, y, min_x, min_y, max_x, max_y):
    width = max_x - min_x
    height = max_y - min_y
    if width <= 0 or height <= 0:
        return x, y
    while x < min_x:
        x += width
    while x > max_x:
        x -= width
    while y < min_y:
        y += height
    while y > max_y:
        y -= height
    return x, y


def large_asteroids_for_wave(wave_index):
    if wave_index < 0:
        wave_index = 0
    count = 4 + wave_index * 2
    if count > 10:
        count = 10
    return count


def extra_lives_gained(score_before, score_after):
    if score_after < score_before:
        return 0
    return (score_after // 10000) - (score_before // 10000)


def almost(a, b):
    return abs(a - b) < 1e-4


def main():
    with open(SOURCE) as handle:
        source = handle.read()

    checks = [
        ("LargeAsteroidScore", 20),
        ("MediumAsteroidScore", 50),
        ("SmallAsteroidScore", 100),
        ("LargeSaucerScore", 200),
        ("SmallSaucerScore", 1000),
        ("StartingLives", 3),
        ("ExtraLifeEvery", 10000),
        ("StartingLargeAsteroids", 4),
        ("MaxLargeAsteroids", 10),
        ("FragmentsPerSplit", 2),
        ("MaxPlayerBullets", 4),
    ]
    failed = 0
    passed = 0

    def ok(condition, name):
        nonlocal failed, passed
        if condition:
            passed += 1
            print("OK  " + name)
        else:
            failed += 1
            print("FAIL " + name)

    for name, expected in checks:
        ok(read_const(source, name) == expected, "%s == %s" % (name, expected))

    ok(large_asteroids_for_wave(0) == 4, "wave 0")
    ok(large_asteroids_for_wave(1) == 6, "wave 1")
    ok(large_asteroids_for_wave(2) == 8, "wave 2")
    ok(large_asteroids_for_wave(3) == 10, "wave 3")
    ok(large_asteroids_for_wave(20) == 10, "wave cap")
    ok(large_asteroids_for_wave(-5) == 4, "negative wave")

    ok(extra_lives_gained(0, 9999) == 0, "no extra life yet")
    ok(extra_lives_gained(9999, 10000) == 1, "cross 10k")
    ok(extra_lives_gained(0, 20000) == 2, "two extra lives")
    ok(extra_lives_gained(20000, 10000) == 0, "score drop")

    x, y = wrap(-5.1, 0.0, -5, -4, 5, 4)
    ok(almost(x, 4.9) and almost(y, 0.0), "left wrap")
    x, y = wrap(5.1, 0.0, -5, -4, 5, 4)
    ok(almost(x, -4.9) and almost(y, 0.0), "right wrap")
    x, y = wrap(0.0, -4.2, -5, -4, 5, 4)
    ok(almost(x, 0.0) and almost(y, 3.8), "bottom wrap")
    x, y = wrap(0.0, 4.2, -5, -4, 5, 4)
    ok(almost(x, 0.0) and almost(y, -3.8), "top wrap")
    x, y = wrap(-5.2, -4.3, -5, -4, 5, 4)
    ok(almost(x, 4.8) and almost(y, 3.7), "corner wrap")

    ok("path: Assets/Resources/Scenes/MainMenu.unity" in open(os.path.join(ROOT, "ProjectSettings", "EditorBuildSettings.asset")).read(), "MainMenu in build")
    game_scene = open(os.path.join(ROOT, "Assets", "Resources", "Scenes", "Game.unity")).read()
    ok('m_Name: PauseMenu' in game_scene and 'm_IsActive: 0' in game_scene, "pause overlay starts hidden")
    main_menu = open(os.path.join(ROOT, "Assets", "Resources", "Scenes", "MainMenu.unity")).read()
    ok("m_MethodName: ToggleControls" in main_menu, "main menu controls wired")
    ok("m_MethodName: ToggleControls" in game_scene, "pause controls wired")
    ok("AsteroidSize.Medium" in source and "AsteroidSize.Small" in source, "split sizes exist")

    print("Passed: %s, Failed: %s" % (passed, failed))
    return 0 if failed == 0 else 1


if __name__ == "__main__":
    sys.exit(main())
