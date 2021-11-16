using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Asteroids.Score;

public class Asteroid : MonoBehaviour
{
    private float health = 3;
    private const int _damage = 3;
   // public static Vector2 direction;
    [SerializeField] private AudioClip _largeExplosionSound;
    private const int numberOfObjects = 2;

    [SerializeField] private GameObject _asteroidPrefab;

    private void OnTriggerEnter2D(Collider2D _object)
    {
        if (_object.TryGetComponent(out ShipHealth ship))
            ship.Damage(_damage);
    }
    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(_largeExplosionSound, transform.position);
            ScoreUI._score += 20;
            GetComponent<PoolObject>().ReturnToPool();
            var directionNew = AsteroidMovementBig.directionBig;
            for (int i = 0; i < numberOfObjects; i++)
            { 
                AsteroidMovement.directionBig = directionNew;
                GameObject AstreoidMedium = PoolManager.GetObject(_asteroidPrefab.name, transform.position);
                directionNew = -directionNew;
            }
        }
    }
}