using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasePlatform : MonoBehaviour
{
    private protected GameObject centre;
    private protected Rigidbody2D rb2D;
    private protected Animator animator;

    private protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private protected virtual void Start()
    {
        centre = GameManager.Instance.CentreObject;
    }


    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
