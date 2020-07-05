using UnityEngine;

public class MobileController : Controller
{
    public override float HorizontalInput => SmoothInput(GetHorizontalInput());

    private protected float slidingValue;


    private float GetHorizontalInput()
    {
        float horizontalInput = 0f;

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).position.x < Screen.width / 2) // Левая половина
            {
                horizontalInput = -1f;
            }

            if (Input.GetTouch(i).position.x > Screen.width / 2) // Правая половина
            {
                horizontalInput = 1f;
            }
        }

        return horizontalInput;
    }


    private float SmoothInput(float targetInput)
    {
        float sensitivity = 3f;
        float deadZone = 0.001f;

        if (Mathf.Abs(targetInput) < deadZone) // Если входящее значение равно нулю
        {
            slidingValue = 0.000f;
        }
        else if (slidingValue / targetInput < 0) // Если частное отрицательное, то входящее значение сменилось на противоположное (Пример: с -1 на 1 или наоборот)
        {
            slidingValue = 0.000f;
        }

        slidingValue = Mathf.MoveTowards(slidingValue, targetInput, sensitivity * Time.deltaTime);

        return (Mathf.Abs(slidingValue) < deadZone) ? 0f : slidingValue;
    }
}
