using UnityEngine;

public class GateView : MonoBehaviour
{
    [field: SerializeField] public GateConfig Config { get; private set; }
    [SerializeField] private Animator _animator;
    
    public Vector3 GetPosition => transform.position;

    public void Open()
    {
        _animator.SetBool("IsOpen", true);
    }
}
