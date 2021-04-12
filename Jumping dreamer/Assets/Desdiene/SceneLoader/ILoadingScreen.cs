using System;

namespace Assets.Desdiene.SceneLoader
{
    public interface ILoadingScreen
    {
        event Action AtOpeningEnd; // в конце открытия
        event Action AtClosingEnd; // в конце закрытия

        void Open();

        void Close();
    }
}
