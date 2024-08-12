using UnityEngine;

public class Gate
{
    private GateView _view;

    public bool IsOpen { get; private set; }
    public float OpenDistance => _view.Config.OpenDistance;

    public Gate(GateView view)
    {
        _view = view;

        IsOpen = false;
    }

    public Vector3 GetPosition => _view.GetPosition;

    public void OpenGate()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            _view.Open();
        }
    }
}
