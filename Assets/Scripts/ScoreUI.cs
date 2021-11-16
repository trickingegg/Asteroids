using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Score
{
    public class ScoreUI : MonoBehaviour
    {
        public GameObject _player;
        public Text score;
        public static int _score;

        public void Update()
        {
            score.text = _score.ToString();
        }
    }
}
