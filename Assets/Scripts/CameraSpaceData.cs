using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Data
{
    public static class CameraSpaceData
    {
        public static Vector2 BottomLeft;
        public static Vector2 TopRight;

        public static float Width => TopRight.x - BottomLeft.x;
        public static float Height => TopRight.y - BottomLeft.y;

        public static Vector2 Center => new Vector2(BottomLeft.x + Width / 2, BottomLeft.y + Height / 2);

        static CameraSpaceData()
        {
            BottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
            TopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        }
    }
}

