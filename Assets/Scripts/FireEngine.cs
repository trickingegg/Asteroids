using UnityEngine;

public class FireEngine : MonoBehaviour
{
    [SerializeField] private AudioClip _engineSound;
    [SerializeField] private ParticleSystem _particleSystem;

    private void Update()
    {
        if (PauseMenu.Paused || GameSession.IsGameOver)
        {
            if (_particleSystem != null && _particleSystem.isPlaying)
                _particleSystem.Stop();
            return;
        }

        bool thrusting = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        if (thrusting)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (_engineSound != null)
                    AudioSource.PlayClipAtPoint(_engineSound, transform.position);
            }

            if (_particleSystem != null && !_particleSystem.isPlaying)
                _particleSystem.Play();
        }
        else if (_particleSystem != null && _particleSystem.isPlaying)
        {
            _particleSystem.Stop();
        }
    }
}
