public class Pauser : SuperMonoBehaviourContainer, IPausable
{
    public Pauser(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) 
    {
        superMonoBehaviour.AwakeInititialized += Subscribe;
        superMonoBehaviour.OnDestroying += Unsubscribe;
    }


    public bool IsPause { get; private set; }


    public void SetPause(bool isPause)
    {
        IsPause = isPause;
        TimeScaler.Instance.SetPause(this);
    }


    private void Subscribe()
    {
        superMonoBehaviour.OnEnabling += AddThisToAllPausables;
        superMonoBehaviour.OnDisabling += RemoveThisFromAllPausables;
    }


    private void Unsubscribe()
    {
        superMonoBehaviour.OnEnabling -= AddThisToAllPausables;
        superMonoBehaviour.OnDisabling -= RemoveThisFromAllPausables;
    }


    private void AddThisToAllPausables()
    {
        TimeScaler.Instance.AllPausers.Add(this);
    }


    private void RemoveThisFromAllPausables()
    {
        TimeScaler.Instance.AllPausers.Remove(this);
    }
}
