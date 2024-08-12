using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float StartSize { get; private set; }
    [field: SerializeField] public float DeadSize { get; private set; }
    [field: SerializeField] public float MinBulletSize { get; private set; }
    [field: SerializeField] public float ShotDistance { get; private set; }
    [field: SerializeField] public float ShotStepSize { get; private set; }
    [field: SerializeField] public float ReductionFactor { get; private set; }
    [field: SerializeField] public BulletView BulletView { get; private set; }
}
