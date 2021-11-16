using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float _maxSpeed = 3;
    public float _rotSpeed = 200;
    public float _boost = 1;
    public float offset = 0;

    private Rigidbody2D _body;
    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        /*var mousePosition = Input.mousePosition;
        //mousePosition.z = transform.position.z - Camera.main.transform.position.z; // это только для перспективной камеры необходимо
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //положение мыши из экранных в мировые координаты
        var angle = Vector2.Angle(Vector2.right, mousePosition - transform.position);//угол между вектором от объекта к мыше и осью х
        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePosition.y ? angle : -angle);//немного магии на последок*/
        /*
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, (rotateZ * _rotSpeed) + offset);*/

        var _movement = Input.GetAxis("Vertical");
        var _rotation = Input.GetAxis("Horizontal");
        var movement = new Vector2 (0, _movement);
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            _body.AddRelativeForce(movement * _boost * Time.deltaTime, ForceMode2D.Impulse);
        _body.velocity = Vector2.ClampMagnitude(_body.velocity, _maxSpeed);
        /*or we can use non physic movement
        movement = Vector2.ClampMagnitude(movement, _maxSpeed);
        transform.Translate(movement * _boost * Time.fixedDeltaTime);
        */
        Vector3 rotation = new Vector3(0, 0, -_rotation * _rotSpeed * Time.deltaTime);
        transform.Rotate(rotation);
    }
}
