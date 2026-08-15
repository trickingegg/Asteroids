using Asteroids.Data;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    private float _cooldown = 0.18f;
    private float _timer;
    [SerializeField] private AudioClip _bulletSound;
    [SerializeField] private ParticleSystem _particleSystem;

    private ParticleSystem.Particle[] _wrapBuffer = new ParticleSystem.Particle[GameRules.MaxPlayerBullets];

    private void Awake()
    {
        _timer = 0f;
        ConfigureParticleWeapon();
    }

    private void ConfigureParticleWeapon()
    {
        if (_particleSystem == null)
            return;

        ParticleSystem.MainModule main = _particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = GameRules.MaxPlayerBullets;
        main.startLifetime = GameRules.BulletLifetime;
        main.startSpeed = 8f;
        main.playOnAwake = false;
        main.loop = false;
        main.useUnscaledTime = false;

        ParticleSystem.EmissionModule emission = _particleSystem.emission;
        emission.rateOverTime = 0f;
        emission.rateOverDistance = 0f;

        ParticleSystem.CollisionModule collision = _particleSystem.collision;
        collision.enabled = true;
        collision.type = ParticleSystemCollisionType.World;
        collision.mode = ParticleSystemCollisionMode.Collision2D;
        collision.sendCollisionMessages = true;
        collision.enableDynamicColliders = true;
        collision.radiusScale = 0.6f;
        collision.lifetimeLoss = 1f;
        collision.bounce = 0f;
        collision.dampen = 0f;
        collision.colliderForce = 0f;
        collision.maxCollisionShapes = 256;
        collision.quality = ParticleSystemCollisionQuality.High;
        collision.collidesWith = Physics2D.DefaultRaycastLayers;
    }

    private void Update()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver)
            return;

        _timer -= Time.deltaTime;

        bool firePressed = Input.GetKey(KeyCode.Space);
        if (!firePressed || _timer > 0f)
            return;

        if (_particleSystem == null)
            return;

        if (_particleSystem.particleCount >= GameRules.MaxPlayerBullets)
            return;

        _particleSystem.Emit(1);

        if (_bulletSound != null)
            AudioSource.PlayClipAtPoint(_bulletSound, transform.position);

        _timer = _cooldown;
    }

    private void LateUpdate()
    {
        WrapParticles();
    }

    private void WrapParticles()
    {
        if (_particleSystem == null || _particleSystem.particleCount == 0)
            return;

        CameraSpaceData.Refresh();
        int count = _particleSystem.GetParticles(_wrapBuffer);
        bool changed = false;
        for (int i = 0; i < count; i++)
        {
            Vector3 position = _wrapBuffer[i].position;
            float x = position.x;
            float y = position.y;
            GameRules.Wrap(ref x, ref y, CameraSpaceData.BottomLeft.x, CameraSpaceData.BottomLeft.y,
                CameraSpaceData.TopRight.x, CameraSpaceData.TopRight.y);
            if (!Mathf.Approximately(x, position.x) || !Mathf.Approximately(y, position.y))
            {
                position.x = x;
                position.y = y;
                _wrapBuffer[i].position = position;
                changed = true;
            }
        }

        if (changed)
            _particleSystem.SetParticles(_wrapBuffer, count);
    }

    private void OnParticleCollision(GameObject other)
    {
        Asteroid asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            asteroid.Damage(1f);
            return;
        }

        Saucer saucer = other.GetComponent<Saucer>();
        if (saucer != null)
            saucer.Hit();
    }
}
