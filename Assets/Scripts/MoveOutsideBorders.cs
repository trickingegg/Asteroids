using Asteroids.Data;
using UnityEngine;

public class MoveOutsideBorders : MonoBehaviour
{
    private Rigidbody2D _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        TryMoveOutside();
    }

    private void TryMoveOutside()
    {
        CameraSpaceData.Refresh();
        Vector2 position = _body != null ? _body.position : (Vector2)transform.position;
        float x = position.x;
        float y = position.y;
        GameRules.Wrap(ref x, ref y, CameraSpaceData.BottomLeft.x, CameraSpaceData.BottomLeft.y,
            CameraSpaceData.TopRight.x, CameraSpaceData.TopRight.y);

        if (Mathf.Approximately(x, position.x) && Mathf.Approximately(y, position.y))
            return;

        Vector2 wrapped = new Vector2(x, y);
        if (_body != null)
            _body.position = wrapped;
        else
            transform.position = wrapped;
    }
}
