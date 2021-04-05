namespace Desdiene.JsonConvertorWrapper
{
    public interface IJsonConvertor<T>
    {
        T DeserializeObject(string jsonData);

        string SerializeObject(T data);
    }
}
