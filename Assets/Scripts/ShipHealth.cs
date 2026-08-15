using System.Collections;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    public static ShipHealth Instance { get; private set; }

    private float _health = 1f;
    private float _godTime = 3.0f;
    private float _blinks = 4;
    private bool _invulnerable;
    private bool _destroyed;

    private SpriteRenderer _sprite;
    private Collider2D _collider;
    private Rigidbody2D _body;

    private void Awake()
    {
        Instance = this;
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Instance = this;
    }

    public bool IsInvulnerable
    {
        get { return _invulnerable || _destroyed; }
    }

    public void Damage(float damage)
    {
        if (_invulnerable || _destroyed || PauseMenu.Paused || GameSession.IsGameOver)
            return;

        _health -= damage;
        if (_health > 0f)
            return;

        HealthUI.health--;
        if (HealthUI.health <= 0)
        {
            _destroyed = true;
            gameObject.SetActive(false);
            GameSession.SetGameOver();
            PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
            if (pauseMenu != null)
                pauseMenu.ShowGameOver();
            return;
        }

        StartCoroutine(Respawn());
    }

    public IEnumerator GrantInvulnerability(float duration)
    {
        _invulnerable = true;
        if (_collider != null)
            _collider.enabled = false;

        float elapsed = 0f;
        float blinkStep = 1f / _blinks;
        while (elapsed < duration)
        {
            if (_sprite != null)
                _sprite.enabled = !_sprite.enabled;
            yield return new WaitForSeconds(blinkStep);
            elapsed += blinkStep;
        }

        if (_sprite != null)
            _sprite.enabled = true;
        if (_collider != null)
            _collider.enabled = true;
        _invulnerable = false;
        _health = 1f;
    }

    private IEnumerator Respawn()
    {
        if (_body != null)
        {
            _body.velocity = Vector2.zero;
            _body.angularVelocity = 0f;
            _body.position = Vector2.zero;
        }
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _health = 1f;
        yield return GrantInvulnerability(_godTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_invulnerable || _destroyed)
            return;

        Asteroid asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            asteroid.Damage(1f);
            Damage(1f);
            return;
        }

        Saucer saucer = other.GetComponent<Saucer>();
        if (saucer != null)
        {
            saucer.Hit();
            Damage(1f);
        }
    }
}
