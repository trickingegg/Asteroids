using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Data;

public class MoveOutsideBorders : MonoBehaviour
{
    private Vector2 TopRight => CameraSpaceData.TopRight;
    private Vector2 BottomLeft => CameraSpaceData.BottomLeft;
    private Vector2 ObjectPos => transform.position;

    public void FixedUpdate()
    {
        TryMoveOutside();
    }

    private void TryMoveOutside()
    {
        if (ObjectPos.x < BottomLeft.x)
        {
            var newPos = new Vector2(TopRight.x - BottomLeft.x - ObjectPos.x, ObjectPos.y);
            transform.position =newPos;
        }
        else if (ObjectPos.y < BottomLeft.y)
        {
            var newPos = new Vector2(ObjectPos.x, TopRight.y - BottomLeft.y - ObjectPos.y);
            transform.position = newPos;
        }
        if (ObjectPos.x > TopRight.x)
        {
            var newPos = new Vector2(BottomLeft.x + ObjectPos.x - TopRight.x, ObjectPos.y);
            transform.position = newPos;
        }
        if (ObjectPos.y > TopRight.y)
        {
            var newPos = new Vector2(ObjectPos.x, BottomLeft.y + ObjectPos.y - TopRight.y);
            transform.position = newPos;
        }
    }

}
