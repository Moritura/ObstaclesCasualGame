using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [field: SerializeField] public PlayerConfig Config { get; private set; }

    public Vector3 GetPosition => transform.position;

    public void ChangeSize(float size)
    {
        transform.localScale = Vector3.one * size;
        transform.position = new Vector3(transform.position.x, size / 2, transform.position.z);
    }
}
