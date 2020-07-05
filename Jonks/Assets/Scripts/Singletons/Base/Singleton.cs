using UnityEngine;

/// <summary> 
/// To access the heir by a static field "Instance".
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    [SerializeField] private bool dontDestroyOnLoad;

    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            AwakeSingleton();
        }
        else
        {
            Destroy(gameObject); //Destroy(gameObject.GetComponent<T>());
        }
    }
    protected virtual void AwakeSingleton() { }
}

/*Пример с GameManager
 * 
 *  public class GameManager : Singleton<GameManager>
{
	protected override void AwakeSingleton() { }
}
 */
