using UnityEngine;

namespace Asteroids.Data
{
    public static class CameraSpaceData
    {
        public static Vector2 BottomLeft;
        public static Vector2 TopRight;

        public static float Width
        {
            get { return TopRight.x - BottomLeft.x; }
        }

        public static float Height
        {
            get { return TopRight.y - BottomLeft.y; }
        }

        public static Vector2 Center
        {
            get { return new Vector2(BottomLeft.x + Width * 0.5f, BottomLeft.y + Height * 0.5f); }
        }

        public static void Refresh()
        {
            Camera camera = Camera.main;
            if (camera == null)
                return;

            float distance = Mathf.Abs(camera.transform.position.z);
            BottomLeft = camera.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
            TopRight = camera.ViewportToWorldPoint(new Vector3(1f, 1f, distance));
        }
    }
}
