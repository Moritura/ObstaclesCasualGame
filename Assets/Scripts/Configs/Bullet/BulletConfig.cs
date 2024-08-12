using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Config/Bullet")]
public class BulletConfig : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float RadiusExplosion { get; private set; }
    [field: SerializeField] public float TimeExplosion { get; private set; }
    [field: SerializeField] public List<int> HitLayers { get; private set; }
}
