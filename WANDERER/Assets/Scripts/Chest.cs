using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    private static readonly int IsOpenParameter = Animator.StringToHash("IsOpen");

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        IsOpen = false;
    }

    public void Open()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            animator.SetBool(IsOpenParameter, true);
        }
    }
}