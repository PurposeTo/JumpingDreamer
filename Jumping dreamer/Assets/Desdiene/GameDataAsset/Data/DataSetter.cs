namespace Desdiene.GameDataAsset.Data
{
    public class DataSetter<T> : Data.IDataSetter where T : GameData
    {
        private T data;

        /// <summary>
        /// Невозможно вызвать конструктор с параметрами у T (обобщения). 
        /// Воспользоваться данным методом.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void SetData(T data)
        {
            this.data = data;
        }
    }
}
