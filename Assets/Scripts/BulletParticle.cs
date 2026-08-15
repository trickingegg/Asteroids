using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    private float _cooldown = 0.18f;
    private float _timer;
    [SerializeField] private AudioClip _bulletSound;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _bulletPrefab;

    private void Awake()
    {
        _timer = 0f;
        if (_particleSystem != null)
        {
            ParticleSystem.CollisionModule collision = _particleSystem.collision;
            collision.enabled = false;
        }
    }

    private void Update()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver)
            return;

        _timer -= Time.deltaTime;

        bool firePressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space);
        if (!firePressed || _timer > 0f)
            return;

        if (Bullet.PlayerBulletCount >= GameRules.MaxPlayerBullets)
            return;

        string bulletName = _bulletPrefab != null ? _bulletPrefab.name : "Bullet";

        Transform ship = transform.parent != null ? transform.parent : transform;
        Vector2 direction = ship.up;
        Vector2 position = (Vector2)ship.position + direction * 0.45f;

        GameObject bulletObject = PoolManager.GetObject(bulletName, position);
        if (bulletObject == null)
            return;

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Launch(position, direction, true);

        if (_bulletSound != null)
            AudioSource.PlayClipAtPoint(_bulletSound, position);

        if (_particleSystem != null)
            _particleSystem.Play();

        _timer = _cooldown;
    }
}
