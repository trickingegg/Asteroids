using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 9f;
    [SerializeField] private float _lifetime = GameRules.BulletLifetime;

    private Rigidbody2D _body;
    private float _spawnTime;
    private bool _fromPlayer;
    private bool _counted;
    private static int _playerBulletCount;

    public static int PlayerBulletCount
    {
        get { return _playerBulletCount; }
    }

    public static void ResetCounters()
    {
        _playerBulletCount = 0;
    }

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 position, Vector2 direction, bool fromPlayer)
    {
        _fromPlayer = fromPlayer;
        _spawnTime = Time.time;
        transform.position = position;

        if (_body == null)
            _body = GetComponent<Rigidbody2D>();

        _body.bodyType = RigidbodyType2D.Kinematic;
        _body.gravityScale = 0f;
        Vector2 dir = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.up;
        _body.velocity = dir * _speed;
        transform.up = dir;

        if (_fromPlayer && !_counted)
        {
            _playerBulletCount++;
            _counted = true;
        }
    }

    private void OnDisable()
    {
        ReleaseCount();
    }

    private void Update()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver)
            return;

        if (Time.time - _spawnTime >= _lifetime)
            ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActiveAndEnabled)
            return;

        if (_fromPlayer)
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.Damage(1f);
                ReturnToPool();
                return;
            }

            Saucer saucer = other.GetComponent<Saucer>();
            if (saucer != null)
            {
                saucer.Hit();
                ReturnToPool();
            }

            return;
        }

        ShipHealth ship = other.GetComponent<ShipHealth>();
        if (ship != null)
        {
            ship.Damage(1f);
            ReturnToPool();
            return;
        }

        Asteroid enemyAsteroid = other.GetComponent<Asteroid>();
        if (enemyAsteroid != null)
        {
            enemyAsteroid.DestroySilent();
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        PoolObject poolObject = GetComponent<PoolObject>();
        if (poolObject != null)
            poolObject.ReturnToPool();
        else
            gameObject.SetActive(false);
    }

    private void ReleaseCount()
    {
        if (!_counted)
            return;

        _counted = false;
        if (_playerBulletCount > 0)
            _playerBulletCount--;
    }
}
