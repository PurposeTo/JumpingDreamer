using System;

[Serializable]
public class PlayerSettingsModel
{
    [NonSerialized] public const string FileName = "PlayerSettings.json";

    public string Language;
}
