using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    private const int _damage = 1;

    private float _cooldown = 0.333f;
    private float _timer;
    [SerializeField] private AudioClip _bulletSound;
    [SerializeField] private ParticleSystem _particleSystem;

    //private readonly List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();
    private void Awake()
    {
        _timer = _cooldown;
    }
    private void Update()
    {
        _timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && _timer <= 0.0f)
        {
            AudioSource.PlayClipAtPoint(_bulletSound, transform.position);
            _particleSystem.Play();
            _timer = _cooldown;
        }
            
    }
    private void OnParticleCollision(GameObject other)
    {
        //var events = _particleSystem.GetCollisionEvents(other, _collisionEvents);

        if (other.TryGetComponent(out Asteroid asteroid))
            asteroid.Damage(_damage);
    }
}
