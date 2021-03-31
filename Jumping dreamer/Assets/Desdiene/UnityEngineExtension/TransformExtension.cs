using UnityEngine;

namespace Desdiene.UnityEngineExtension
{
    /// <summary>
    /// Данный класс позволяет делать цепочки методов с классом Transform, изменяя тому поля
    /// </summary>
    public static class TransformExtension
    {
        /// <summary>
        /// Устанавливает позицию объекта с:
        /// - нулевыми координатами
        /// - нулевым повтором
        /// </summary>
        /// <param name="transform"></param>
        public static void SetDefault(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Устанавливает позицию объекта с:
        /// - нулевыми координатами
        /// - нулевым повтором
        /// - размером 1
        /// </summary>
        /// <param name="transform"></param>
        public static void SetDefaultWithScale(this Transform transform)
        {
            transform.SetDefault();
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Устанавливает локальный размер объекта одинаковым по трем осям по задаваемому значению
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="scale"></param>
        public static void SetLocalScale(this Transform transform, float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }


        public static Transform SetPosition(this Transform transform, Vector3 position)
        {
            transform.position = position;
            return transform;
        }

        public static Transform SetLocalPosition(this Transform transform, Vector3 position)
        {
            transform.localPosition = position;
            return transform;
        }

        public static Transform SetLocalScale(this Transform transform, Vector3 scale)
        {
            transform.localScale = scale;
            return transform;
        }

        public static Transform SetParent(this Transform transform, Transform parent)
        {
            transform.parent = parent;
            return transform;
        }

        public static Transform SetNullParent(this Transform transform)
        {
            transform.parent = null;
            return transform;
        }


        public static Transform SetRotation(this Transform transform, Quaternion rotation)
        {
            transform.rotation = rotation;
            return transform;
        }

    }
}
