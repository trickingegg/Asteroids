using Asteroids.Data;
using Asteroids.Score;
using UnityEngine;

public class SaucerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _saucerPrefab;
    [SerializeField] private float _minDelay = 8f;
    [SerializeField] private float _maxDelay = 16f;

    private float _timer;

    private void Start()
    {
        ScheduleNext();
    }

    private void Update()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver)
            return;

        if (Saucer.AliveCount > 0)
            return;

        _timer -= Time.deltaTime;
        if (_timer > 0f)
            return;

        Spawn();
        ScheduleNext();
    }

    private void ScheduleNext()
    {
        _timer = Random.Range(_minDelay, _maxDelay);
    }

    private void Spawn()
    {
        if (_saucerPrefab == null)
            return;

        CameraSpaceData.Refresh();
        bool fromLeft = Random.value < 0.5f;
        float x = fromLeft ? CameraSpaceData.BottomLeft.x : CameraSpaceData.TopRight.x;
        float y = Random.Range(CameraSpaceData.BottomLeft.y + 1f, CameraSpaceData.TopRight.y - 1f);
        Vector2 position = new Vector2(x, y);

        GameObject saucerObject = PoolManager.GetObject(_saucerPrefab.name, position);
        if (saucerObject == null)
            return;

        Saucer saucer = saucerObject.GetComponent<Saucer>();
        if (saucer != null)
        {
            bool small = ScoreUI._score >= GameRules.SmallSaucerScoreThreshold;
            saucer.Configure(small);
        }

        AsteroidMovement movement = saucerObject.GetComponent<AsteroidMovement>();
        if (movement != null)
            movement.enabled = false;

        Rigidbody2D body = saucerObject.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            float speed = ScoreUI._score >= GameRules.SmallSaucerScoreThreshold ? 2.8f : 2.0f;
            body.velocity = new Vector2(fromLeft ? speed : -speed, 0f);
        }
    }
}
