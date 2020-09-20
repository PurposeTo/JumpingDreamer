using System.Linq;
using UnityEngine;

public class CameraSizeChecker : MonoBehaviour
{
    private Camera thisCamera;


    private void Awake()
    {
        thisCamera = gameObject.GetComponent<Camera>();

        if (thisCamera.orthographic) GetOrthographicCameraArea();
        else Debug.LogError("Can't check camera area, if it is not Orthographic camera!");
    }

    //FixMe: не работает!
    // Todo IsInTheCameraArea не учитывает ширину и высоту объекта...
    public bool IsInTheCameraArea(Vector2 position)
    {
        Vector2[] cameraCornerDots = GetOrthographicCameraArea();

        Vector2 minXY = new Vector2(
            x: Mathf.Min(cameraCornerDots.Select(dot => dot.x).ToArray()),
            y: Mathf.Min(cameraCornerDots.Select(dot => dot.y).ToArray()));
        Vector2 maxXY = new Vector2(
            x: Mathf.Max(cameraCornerDots.Select(dot => dot.x).ToArray()),
            y: Mathf.Max(cameraCornerDots.Select(dot => dot.y).ToArray()));

        return position.x > minXY.x && position.x < maxXY.x
            && position.y > minXY.y && position.y < maxXY.y;
    }


    // OrthographicSize is a constant and means height
    private Vector2[] GetOrthographicCameraArea()
    {
        float screenRatio = thisCamera.aspect;
        float orthographicHeight = thisCamera.orthographicSize;
        float orthographicWidth = thisCamera.orthographicSize * screenRatio;


        return new Vector2[] {
            new Vector2(
                x: transform.position.x + orthographicWidth,
                y: transform.position.y + orthographicHeight),
            new Vector2(
                x: transform.position.x - orthographicWidth,
                y: transform.position.y + orthographicHeight),
            new Vector2(
                x: transform.position.x + orthographicWidth,
                y: transform.position.y - orthographicHeight),
            new Vector2(
                x: transform.position.x - orthographicWidth,
                y: transform.position.y - orthographicHeight)
        };
    }
}
