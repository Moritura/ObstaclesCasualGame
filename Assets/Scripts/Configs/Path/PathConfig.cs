using UnityEngine;

[CreateAssetMenu(fileName = "PathConfig", menuName = "Config/Path")]
public class PathConfig : ScriptableObject
{
    [field: SerializeField] public float DistanceToObstacle { get; private set; }
}
