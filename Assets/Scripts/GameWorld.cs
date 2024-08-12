using UnityEngine;

public class GameWorld : MonoBehaviour
{
    [field: SerializeField] public PlayerView PlayerView { get; private set; }
    [field: SerializeField] public GateView GateView { get; private set; }
    [field: SerializeField] public GameView GameView { get; private set; }
    [field: SerializeField] public PathView PathView { get; private set; }
}
