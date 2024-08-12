using System;
using VContainer.Unity;

public class Path : IStartable, IDisposable
{
    private PathView _view;

    private Player _player;
    private Gate _gate;
    private Map _map;

    public Path(PathView view, Player player, Gate gate, Map map)
    {
        _view = view;
        _player = player;
        _gate = gate;
        _map = map;
    }

    public void Start()
    {
        OnMove();
        OnChangeSize();

        Subscribe();
    }

    public void Hide()
    {
        _view.Hide();
    }

    private void Subscribe()
    {
        _player.ChangeSizeEvent += OnChangeSize;
        _player.MoveEvent += OnMove;
        _map.ExplosionEvent += OnExplore;
    }

    private void Unsubscribe()
    {
        _player.ChangeSizeEvent -= OnChangeSize;
        _player.MoveEvent += OnMove;
        _map.ExplosionEvent -= OnExplore;
    }

    private void OnMove()
    {
        _view.DrawPath(_player.GetPosition, _gate.GetPosition);
    }

    private void OnExplore()
    {
        CalculateFreePath();
    }

    public void OnChangeSize()
    {
        _view.SetSize(_player.Size);
        CalculateFreePath();
    }

    private void CalculateFreePath()
    {
        var result = _map.GetAdjustmentDistance(_player.GetPosition, _gate.GetPosition, _player.Size);
        _player.Move(result - _view.Config.DistanceToObstacle);
    }

    public void Dispose()
    {
        Unsubscribe();
    }
}
