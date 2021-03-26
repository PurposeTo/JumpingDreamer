namespace Desdiene.Object_pooler {
    public interface IPooledObject
    {
        /// <summary>
        /// Данный метод вызывается каждый раз, при спавне объекта через ObjectPool
        /// </summary>
        void OnObjectSpawn();
    }
}
