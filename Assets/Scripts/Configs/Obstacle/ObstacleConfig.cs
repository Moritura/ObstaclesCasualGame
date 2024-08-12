using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleConfig", menuName = "Config/Obstacle")]
public class ObstacleConfig : ScriptableObject
{
    [field: SerializeField] public Material HitMaterial { get; private set; }
    [field: SerializeField] public float DelayToExplosion { get; private set; }
    [field: SerializeField] public float TimeExplosion { get; private set; }
    [field: SerializeField] public float Radius { get; private set; }
}
