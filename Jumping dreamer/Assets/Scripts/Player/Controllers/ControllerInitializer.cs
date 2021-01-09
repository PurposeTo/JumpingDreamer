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
            case RuntimePlatform.WindowsEditor:
                return new WindowsEditorController();
            default:
                Debug.LogError($"{currentRuntimePlatform} is unknown platform!");
                return new WindowsEditorController();
        }
    }
}
