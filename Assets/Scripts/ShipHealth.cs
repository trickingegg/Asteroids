using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    private const int _damage = 10;
    private float _health = 1;
    private float _godTime = 3.0f;
    private float _blinks = 4;

    public void Damage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            StartCoroutine(God());
            HealthUI.health--;
            //Destroy(gameObject);
            Debug.Log("Ship destroyed");
        }
    }

    IEnumerator God()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        for (int i = 0; i < _godTime * _blinks; i++)
            yield return StartCoroutine(Blinking(GetComponent<SpriteRenderer>()));
        GetComponent<PolygonCollider2D>().enabled = true;
    }
    IEnumerator Blinking(SpriteRenderer sprite)
    {
        if (sprite.enabled)
            sprite.enabled = false;
        else
            sprite.enabled = true;
        yield return new WaitForSeconds(1 / _blinks);
    }

    private void OnTriggerEnter2D(Collider2D _object)
    {
        if (_object.TryGetComponent(out Asteroid asteroid))
            asteroid.Damage(_damage);
    }
}
