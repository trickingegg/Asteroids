using Asteroids.Data;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float _maxSpeed = 3;
    public float _rotSpeed = 200;
    public float _boost = 1;
    public float offset = 0;

    private Rigidbody2D _body;
    private float _hyperspaceCooldown;
    private const float HyperspaceCooldownTime = 1.5f;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        if (_body != null)
        {
            _body.gravityScale = 0f;
            _body.drag = 0f;
        }
        ApplyControlScheme();
    }

    private void Update()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver)
            return;

        _hyperspaceCooldown -= Time.deltaTime;
        if (GameSettings.UseMouseAim)
            AimAtMouse();
        else
            Rotate();
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.H))
        {
            TryHyperspace();
        }
    }

    private void FixedUpdate()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver)
            return;

        if (_body == null)
            return;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            _body.AddRelativeForce(Vector2.up * (_boost * 8f), ForceMode2D.Force);

        _body.velocity = Vector2.ClampMagnitude(_body.velocity, _maxSpeed);
    }

    public void ApplyControlScheme()
    {
        ShipMovementMouse mouseMovement = GetComponent<ShipMovementMouse>();
        if (mouseMovement != null)
            mouseMovement.enabled = false;
    }

    private void Rotate()
    {
        float rotation = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(rotation) < 0.01f)
            return;

        transform.Rotate(0f, 0f, -rotation * _rotSpeed * Time.deltaTime);
    }

    private void AimAtMouse()
    {
        Camera camera = Camera.main;
        if (camera == null)
            return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(camera.transform.position.z);
        Vector3 mouseWorld = camera.ScreenToWorldPoint(mouseScreen);
        Vector2 diff = (Vector2)mouseWorld - (Vector2)transform.position;
        if (diff.sqrMagnitude < 0.0001f)
            return;

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + offset;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void TryHyperspace()
    {
        if (_hyperspaceCooldown > 0f)
            return;

        ShipHealth health = GetComponent<ShipHealth>();
        if (health != null && health.IsInvulnerable)
            return;

        _hyperspaceCooldown = HyperspaceCooldownTime;
        CameraSpaceData.Refresh();

        float marginX = CameraSpaceData.Width * 0.12f;
        float marginY = CameraSpaceData.Height * 0.12f;
        float x = Random.Range(CameraSpaceData.BottomLeft.x + marginX, CameraSpaceData.TopRight.x - marginX);
        float y = Random.Range(CameraSpaceData.BottomLeft.y + marginY, CameraSpaceData.TopRight.y - marginY);
        Vector2 destination = new Vector2(x, y);

        if (Random.value < GameRules.HyperspaceFailChance)
        {
            if (_body != null)
            {
                _body.velocity = Vector2.zero;
                _body.position = destination;
            }
            else
            {
                transform.position = destination;
            }

            if (health != null)
                health.Damage(1f);
            return;
        }

        if (_body != null)
        {
            _body.velocity = Vector2.zero;
            _body.position = destination;
        }
        else
        {
            transform.position = destination;
        }

        if (health != null)
            health.StartCoroutine(health.GrantInvulnerability(1.2f));
    }
}
