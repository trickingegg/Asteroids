using Asteroids.Data;
using UnityEngine;

public class AsteroidSpawnBig : MonoBehaviour
{
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private float _nextWaveDelay = 1.5f;

    private int _waveIndex;
    private float _waveCooldown;
    private bool _waitingForNextWave;

    private void Awake()
    {
        Physics2D.gravity = Vector2.zero;
        GameSession.Reset();
    }

    private void Start()
    {
        SpawnWave();
    }

    private void Update()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver)
            return;

        if (Asteroid.AliveCount > 0 || Saucer.AliveCount > 0)
        {
            _waitingForNextWave = false;
            return;
        }

        if (!_waitingForNextWave)
        {
            _waitingForNextWave = true;
            _waveCooldown = _nextWaveDelay;
            return;
        }

        _waveCooldown -= Time.deltaTime;
        if (_waveCooldown > 0f)
            return;

        _waveIndex++;
        SpawnWave();
        _waitingForNextWave = false;
    }

    private void SpawnWave()
    {
        if (_asteroidPrefab == null)
            return;

        CameraSpaceData.Refresh();
        int count = GameRules.LargeAsteroidsForWave(_waveIndex);
        for (int i = 0; i < count; i++)
        {
            Vector2 position = RandomEdgePosition();
            PoolManager.GetObject(_asteroidPrefab.name, position);
        }
    }

    private static Vector2 RandomEdgePosition()
    {
        float minX = CameraSpaceData.BottomLeft.x;
        float maxX = CameraSpaceData.TopRight.x;
        float minY = CameraSpaceData.BottomLeft.y;
        float maxY = CameraSpaceData.TopRight.y;
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0:
                return new Vector2(minX, Random.Range(minY, maxY));
            case 1:
                return new Vector2(maxX, Random.Range(minY, maxY));
            case 2:
                return new Vector2(Random.Range(minX, maxX), minY);
            default:
                return new Vector2(Random.Range(minX, maxX), maxY);
        }
    }
}
