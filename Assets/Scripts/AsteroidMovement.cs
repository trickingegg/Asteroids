using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    private Rigidbody2D _body;
    public static Vector2 directionBig;
    private Vector2 direction;
    private float _speed;
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _speed = Random.Range(0.3f, 1.0f);
        direction = Quaternion.AngleAxis(-45, Vector3.forward) * directionBig;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _body.AddRelativeForce(direction * Time.fixedDeltaTime, ForceMode2D.Impulse);
        _body.velocity = Vector2.ClampMagnitude(_body.velocity, _speed);
        Debug.Log("dir = " + direction);
    }
}
