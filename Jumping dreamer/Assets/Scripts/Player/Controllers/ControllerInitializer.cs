using UnityEngine;

public static class ControllerInitializer
{
    public static Controller InitializeController(RuntimePlatform currentRuntimePlatform)
    {
        Debug.Log($"InitializeController with {currentRuntimePlatform}!");

        switch (currentRuntimePlatform)
        {
            case RuntimePlatform.Android:
                return new MobileController();
                //break;
            case RuntimePlatform.WindowsEditor:
                return new WindowsEditorController();
                //break;
            default:
                Debug.LogError($"{currentRuntimePlatform} is unknown platform!");
                return new WindowsEditorController();
                //break;
        }
    }
}
