using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Data;
using Random = UnityEngine.Random;

public class AsteroidSpawnMiddle : MonoBehaviour
{
    private const int numberOfObjects = 2;

    [SerializeField] private GameObject _asteroidPrefab;

    private void Update()
    {
        Spawner();
    }
    public void Spawner()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            var posX = Random.Range(0.0f, 1.0f);
            var posY = Random.Range(0.0f, 1.0f);
            Vector2 vec = Camera.main.ViewportToWorldPoint(new Vector2(posX, posY));
            //Instantiate(_asteroidPrefab, vec, Quaternion.identity);
            GameObject AstreoidMedium = PoolManager.GetObject(_asteroidPrefab.name, vec);
        }
    }
}
