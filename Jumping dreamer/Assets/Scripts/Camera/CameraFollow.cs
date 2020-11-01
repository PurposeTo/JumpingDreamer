using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    private PlayerPresenter playerPresenter;
    private GameObject centre;

    private Camera thisCamera;
    private float cameraDefaultOrthographicSize;

    private readonly float sizeChangeMultiplier = 1f / 1.1f;
    private readonly float sensitiveToClose = 2f; // Чувствительность к приближению камеры
    private readonly float sensitiveToDistance = 3.5f; // Чувствительность к отдалению камеры

    private void Start()
    {
        playerPresenter = ImportantGameObjectsHolder.Instance.PlayerPresenter;
        player = playerPresenter.gameObject;
        centre = ImportantGameObjectsHolder.Instance.Centre.gameObject;
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
        SetCameraRightPosition();
        SetCameraOrtographicSize();
    }


    private void SetCameraRightPosition()
    {
        transform.position = GetTheRightCameraPosition();
        transform.rotation = GameLogic.GetOrthoRotation(transform.position, centre.transform.position);
    }


    private void SetCameraOrtographicSize()
    {
        float expectedOrthographicSize = GetOrthographicSizeFromDistanceToCentre();
        float orthographicSize = GetOrthographicSizeWithGravitySensitive(expectedOrthographicSize);
        thisCamera.orthographicSize = SmoothOrthographicSize(orthographicSize);
    }


    private Vector3 GetTheRightCameraPosition()
    {
        return new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }


    private float GetOrthographicSizeFromDistanceToCentre()
    {
        Vector2 toCentreVector = (centre.transform.position - player.transform.position);
        float expectedOrthographicSize = (toCentreVector.magnitude - Centre.CentreRadius) * sizeChangeMultiplier;
        expectedOrthographicSize += cameraDefaultOrthographicSize;
        return expectedOrthographicSize;
    }


    private float SmoothOrthographicSize(float expectedOrthographicSize)
    {
        float sensetive = thisCamera.orthographicSize < expectedOrthographicSize
            ? sensitiveToDistance // Если необходимо отдалить
            : sensitiveToClose; // Если необходимо приблизить

        return Mathf.Lerp(thisCamera.orthographicSize, expectedOrthographicSize, sensetive * Time.deltaTime);
    }


    private float GetOrthographicSizeWithGravitySensitive(float expectedOrthographicSize)
    {
        float sensetive = Mathf.Clamp(playerPresenter.GravitySensitive.GetGravitySensitive(), 0.2f, float.MaxValue);
        float lerpedValue = Mathf.Lerp(cameraDefaultOrthographicSize, expectedOrthographicSize, sensetive);
        return GameLogic.ClampValueByAnotherValue(lerpedValue, expectedOrthographicSize, 2f);
    }
}
