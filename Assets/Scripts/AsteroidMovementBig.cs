using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovementBig : MonoBehaviour
{
    private Rigidbody2D _body;
    private Vector2 direction;
    public static Vector2 directionBig;
    private float _speed;
    //public static Quaternion mas;
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _speed = Random.Range(0.3f, 1.0f);
        direction = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        //AsteroidMovement.directionBig = direction;
        directionBig = direction;
        Debug.Log("dirBig = " + direction);

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
    }
}