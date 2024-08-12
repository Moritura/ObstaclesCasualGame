using UnityEngine;

[CreateAssetMenu(fileName = "GateConfig", menuName = "Config/Gate")]
public class GateConfig : ScriptableObject
{
    [field: SerializeField] public float OpenDistance { get; private set; }
}
