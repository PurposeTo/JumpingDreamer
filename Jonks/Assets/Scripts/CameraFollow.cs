using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private GameObject centre;

    private Vector2 toCentreVector;

    private Camera thisCamera;
    private float cameraDefaultOrthographicSize;

    private readonly float sizeChangeMultiplier = 1f / 1.1f;
    private readonly float sensitiveToClose = 2f; // Чувствительность к приближению камеры
    private readonly float sensitiveToDistance = 3f; // Чувствительность к отдалению камеры

    private void Start()
    {
        player = GameManager.Instance.Player;
        centre = GameManager.Instance.CentreObject;
        thisCamera = gameObject.GetComponent<Camera>();
        cameraDefaultOrthographicSize = thisCamera.orthographicSize;

        SetCameraRightValues();
    }


    private void LateUpdate()
    {
        SetCameraRightValues();
    }


    private void SetCameraRightValues()
    {
        toCentreVector = (centre.transform.position - player.transform.position);

        transform.position = GetTheRightCameraPosition();
        transform.rotation = GameLogic.GetOrthoRotation(transform.position, centre.transform.position);
        thisCamera.orthographicSize = GetTheRightOrthographicSize();
    }


    private Vector3 GetTheRightCameraPosition()
    {
        return new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }


    private float GetTheRightOrthographicSize()
    {
        
        float expectedOrthographicSize = (toCentreVector.magnitude - Centre.CentreRadius) * sizeChangeMultiplier;

        expectedOrthographicSize += cameraDefaultOrthographicSize;


        float sensetive;
        // Если необходимо отдалить
        if (thisCamera.orthographicSize < expectedOrthographicSize)
        {
            sensetive = sensitiveToDistance; // Отдаляться должна быстрее
        }
        else // Если необходимо приблизить
        {
            sensetive = sensitiveToClose;
        }

        expectedOrthographicSize = Mathf.Lerp(thisCamera.orthographicSize, expectedOrthographicSize, sensetive * Time.deltaTime);

        return expectedOrthographicSize;
    }
}
