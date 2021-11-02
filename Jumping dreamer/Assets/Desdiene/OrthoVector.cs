using UnityEngine;

namespace Desdiene
{
    /// <summary>
    /// Класс для работы с ортогональными векторами в 2d плоскостях
    /// </summary>
    public static class OrthoVector2
    {

        /// <summary>
        /// Возвращает ортогональный вектор
        /// </summary>
        public static Vector2 Get(Vector2 normal)
        {
            return new Vector2(normal.y, -normal.x);
        }

        /// <summary>
        /// Возвращает нормализованный ортогональный вектор
        /// </summary>
        public static Vector2 GetNormalized(Vector2 normal)
        {
            return Get(normal).normalized;
        }

        /// <summary>
        /// Возвращает кватернион, повернутый перпендикулярно цели. 
        /// Используется Vector3 для работы с кватернионом.
        /// </summary>
        public static Quaternion GetOrthoRotation(Vector3 currentPosition, Vector3 target)
        {
            Vector3 difference = (currentPosition - target).normalized;

            // Вычисляем кватернион нужного поворота. Вектор forward говорит вокруг какой оси поворачиваться
            Quaternion quaternionRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: difference);

            return quaternionRotation;
        }
    }
}
