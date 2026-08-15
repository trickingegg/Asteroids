using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    private Rigidbody2D _body;
    private bool _hasPresetLaunch;

    public Vector2 Velocity
    {
        get
        {
            if (_body == null)
                _body = GetComponent<Rigidbody2D>();
            return _body != null ? _body.velocity : Vector2.zero;
        }
    }

    public void SetLaunch(Vector2 direction, float speed)
    {
        if (_body == null)
            _body = GetComponent<Rigidbody2D>();

        _hasPresetLaunch = true;
        ApplyLaunch(direction, speed);
    }

    private void OnEnable()
    {
        _body = GetComponent<Rigidbody2D>();
        ConfigureBody();

        if (_hasPresetLaunch)
            return;

        Asteroid asteroid = GetComponent<Asteroid>();
        AsteroidSize size = asteroid != null ? asteroid.Size : AsteroidSize.Large;
        float speed = Random.Range(GameRules.MinSpeed(size), GameRules.MaxSpeed(size));
        Vector2 direction = Random.insideUnitCircle;
        if (direction.sqrMagnitude < 0.01f)
            direction = Vector2.right;
        ApplyLaunch(direction, speed);
    }

    private void OnDisable()
    {
        _hasPresetLaunch = false;
    }

    private void ConfigureBody()
    {
        if (_body == null)
            return;

        _body.gravityScale = 0f;
        _body.bodyType = RigidbodyType2D.Dynamic;
        _body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _body.angularDrag = 0f;
        _body.drag = 0f;
        _body.constraints = RigidbodyConstraints2D.None;
    }

    private void ApplyLaunch(Vector2 direction, float speed)
    {
        if (_body == null)
            return;

        Vector2 normalized = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
        _body.velocity = normalized * speed;
        _body.angularVelocity = Random.Range(-80f, 80f);
        _body.WakeUp();
    }
}
