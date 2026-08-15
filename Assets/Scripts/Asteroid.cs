using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private AsteroidSize _size = AsteroidSize.Large;
    [SerializeField] private AudioClip _largeExplosionSound;
    [SerializeField] private GameObject _asteroidPrefab;

    private bool _dead;
    private static int _aliveCount;

    public AsteroidSize Size
    {
        get { return _size; }
    }

    public static int AliveCount
    {
        get { return _aliveCount; }
    }

    public static void ResetAliveCount()
    {
        _aliveCount = 0;
    }

    private void OnEnable()
    {
        _dead = false;
        _aliveCount++;
    }

    private void OnDisable()
    {
        if (_aliveCount > 0)
            _aliveCount--;
    }

    public void Damage(float damage)
    {
        if (_dead || damage <= 0f)
            return;

        DestroyAsteroid(true);
    }

    public void DestroySilent()
    {
        DestroyAsteroid(false);
    }

    private void DestroyAsteroid(bool awardScore)
    {
        if (_dead)
            return;

        _dead = true;

        if (_largeExplosionSound != null)
            AudioSource.PlayClipAtPoint(_largeExplosionSound, transform.position);

        if (awardScore)
            GameSession.AddScore(GameRules.ScoreForAsteroid(_size));

        SpawnFragments();

        PoolObject poolObject = GetComponent<PoolObject>();
        if (poolObject != null)
            poolObject.ReturnToPool();
        else
            gameObject.SetActive(false);
    }

    private void SpawnFragments()
    {
        AsteroidSize childSize;
        if (!GameRules.TryGetChildSize(_size, out childSize))
            return;

        if (_asteroidPrefab == null)
            return;

        Vector2 parentVelocity = Vector2.up;
        AsteroidMovement parentMovement = GetComponent<AsteroidMovement>();
        if (parentMovement != null && parentMovement.Velocity.sqrMagnitude > 0.01f)
            parentVelocity = parentMovement.Velocity;

        Vector2 origin = transform.position;
        float childSpeedMin = GameRules.MinSpeed(childSize);
        float childSpeedMax = GameRules.MaxSpeed(childSize);
        float baseAngle = Mathf.Atan2(parentVelocity.y, parentVelocity.x) * Mathf.Rad2Deg;

        for (int i = 0; i < GameRules.FragmentsPerSplit; i++)
        {
            float angleOffset = (i == 0 ? 1f : -1f) * Random.Range(25f, 55f);
            float angle = baseAngle + angleOffset;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector2 spawnPosition = origin + direction * 0.35f;

            GameObject fragment = PoolManager.GetObject(_asteroidPrefab.name, spawnPosition);
            if (fragment == null)
                continue;

            AsteroidMovement movement = fragment.GetComponent<AsteroidMovement>();
            if (movement != null)
            {
                float speed = Random.Range(childSpeedMin, childSpeedMax);
                movement.SetLaunch(direction, speed);
            }
        }
    }
}
