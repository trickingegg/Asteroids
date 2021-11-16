using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEngine : MonoBehaviour
{
    [SerializeField] private AudioClip _engineSound;
    [SerializeField] private ParticleSystem _particleSystem;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            AudioSource.PlayClipAtPoint(_engineSound, transform.position);
            _particleSystem.Play();
        }

        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            _particleSystem.Stop();
    }
}
