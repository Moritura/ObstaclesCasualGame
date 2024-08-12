using UnityEngine;

public class PathView : MonoBehaviour
{
    [field: SerializeField] public PathConfig Config { get; private set; }

    public void DrawPath(Vector3 start, Vector3 end)
    {
        start.y = 0.01f;
        end.y = 0.01f;

        Vector3 center = (start + end) / 2;
        transform.position = center;

        float distance = Vector3.Distance(start, end);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distance);

        transform.LookAt(end);
    }

    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
