namespace Desdiene.Tools
{
    public class AtomicReference<T>
    {
        private T value;

        public AtomicReference() { }
        public AtomicReference(T value)
        {
            this.value = value;
        }

        public void Set(T value) => this.value = value;

        public T Get() => value;
    }
}
