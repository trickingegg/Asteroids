using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static int health = 4;
    // Start is called before the first frame update
    public Image Heart0;
    public Image Heart1;
    public Image Heart2;
    public Image Heart3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (health)
        {
            case 4:
                Heart3.enabled = true;
                Heart2.enabled = true;
                Heart1.enabled = true;
                Heart0.enabled = true;
                break;
            case 3:
                Heart3.enabled = false;

                break;

            case 2:
                Heart2.enabled = false;

                break;

            case 1:
                Heart1.enabled = false;
                break;
            case 0:
                Heart0.enabled = false;
                break;
        }
    }
}
