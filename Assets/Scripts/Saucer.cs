using Asteroids.Data;
using Asteroids.Score;
using UnityEngine;

public class Saucer : MonoBehaviour
{
    [SerializeField] private bool _small;
    [SerializeField] private float _speed = 2.2f;
    [SerializeField] private float _fireInterval = 1.4f;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private GameObject _bulletPrefab;

    private Rigidbody2D _body;
    private float _fireTimer;
    private float _steerTimer;
    private bool _dead;
    private static int _aliveCount;

    public static int AliveCount
    {
        get { return _aliveCount; }
    }

    public static void ResetAliveCount()
    {
        _aliveCount = 0;
    }

    public void Configure(bool small)
    {
        _small = small;
        _speed = small ? 2.8f : 2.0f;
        _fireInterval = small ? 0.85f : 1.5f;
        transform.localScale = small ? new Vector3(0.45f, 0.22f, 1f) : new Vector3(0.75f, 0.35f, 1f);
    }

    private void OnEnable()
    {
        _dead = false;
        _aliveCount++;
        _body = GetComponent<Rigidbody2D>();
        if (_body != null)
        {
            _body.bodyType = RigidbodyType2D.Kinematic;
            _body.gravityScale = 0f;
        }
        _fireTimer = _fireInterval;
        _steerTimer = 0f;
    }

    private void OnDisable()
    {
        if (_aliveCount > 0)
            _aliveCount--;
    }

    private void Update()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver || _dead)
            return;

        Steer();
        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0f)
        {
            Fire();
            _fireTimer = _fireInterval;
        }
    }

    public void Hit()
    {
        if (_dead)
            return;

        _dead = true;
        if (_explosionSound != null)
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position);

        GameSession.AddScore(_small ? GameRules.SmallSaucerScore : GameRules.LargeSaucerScore);

        PoolObject poolObject = GetComponent<PoolObject>();
        if (poolObject != null)
            poolObject.ReturnToPool();
        else
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Asteroid asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            asteroid.DestroySilent();
            Hit();
        }
    }

    private void Steer()
    {
        if (_body == null)
            return;

        _steerTimer -= Time.deltaTime;
        if (_steerTimer <= 0f)
        {
            _steerTimer = Random.Range(0.6f, 1.4f);
            float vertical = Random.Range(-0.6f, 0.6f);
            float horizontal = Mathf.Sign(_body.velocity.x);
            if (Mathf.Abs(horizontal) < 0.1f)
                horizontal = transform.position.x < 0f ? 1f : -1f;
            _body.velocity = new Vector2(horizontal * _speed, vertical * _speed);
        }
    }

    private void Fire()
    {
        if (_bulletPrefab == null)
            return;

        Vector2 direction;
        if (_small)
            direction = AimAtPlayer();
        else
            direction = Random.insideUnitCircle.normalized;

        if (direction.sqrMagnitude < 0.01f)
            direction = Vector2.right;

        GameObject bulletObject = PoolManager.GetObject(_bulletPrefab.name, transform.position);
        if (bulletObject == null)
            return;

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Launch(transform.position, direction, false);
    }

    private Vector2 AimAtPlayer()
    {
        ShipHealth ship = ShipHealth.Instance;
        if (ship == null)
            return Random.insideUnitCircle;

        Vector2 toPlayer = (Vector2)ship.transform.position - (Vector2)transform.position;
        float inaccuracy = ScoreUI._score >= 35000 ? 8f : 22f;
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg + Random.Range(-inaccuracy, inaccuracy);
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }
}
