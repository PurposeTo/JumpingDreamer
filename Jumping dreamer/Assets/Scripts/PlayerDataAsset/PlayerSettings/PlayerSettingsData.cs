using System;

[Serializable]
public class PlayerSettingsData
{
    [NonSerialized] public const string FileName = "PlayerSettings.json";

    public string Language;
}
