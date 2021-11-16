using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonds : MonoBehaviour
{
    public Transform _object;
    public Transform _receiver;
    private Camera _camera;

    private bool _objectIsOverlapping = false;
    void Start()
    {
        _camera = GetComponent<Camera>();
        Vector2 vec = _camera.ViewportToWorldPoint(new Vector2(1.5f, 1.5f));
        gameObject.GetComponent<BoxCollider2D>().size = vec;
    }

    void Update()
    {
        if (_objectIsOverlapping)
        {
            Vector2 portalToObject = _object.position - transform.position;
            float dotProduct = Vector2.Dot(transform.up, portalToObject);

            if (dotProduct < 0f)
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, _receiver.rotation);
                rotationDiff += 180;
                _object.Rotate(Vector2.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0, 0, rotationDiff) * portalToObject;
                _object.position = _receiver.position + positionOffset;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         _objectIsOverlapping = true;

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        _objectIsOverlapping = false;
    }

}
