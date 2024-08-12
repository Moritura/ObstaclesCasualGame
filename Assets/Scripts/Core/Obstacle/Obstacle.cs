using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class Obstacle
{
    public event Action<Obstacle> ExplosionEvent;

    private ObstacleView _view;

    private CancellationTokenSource _cancellationTokenSource;

    public Vector3 GetPosition => _view.transform.position;
    public float GetRadius => _view.Config.Radius;

    public Obstacle(ObstacleView view, Map map)
    {
        _view = view;
        _cancellationTokenSource = new CancellationTokenSource();
        Subscribe();

        map.Registration(this);
    }

    public async UniTask Infect()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
        {
            await _view.Infect(_cancellationTokenSource.Token);
            if (_view != null)
            {
                UnityEngine.Object.Destroy(_view.gameObject);
            }
        }
    }

    private void Subscribe()
    {
        _view.ExplosionEvent += OnExplosion;
        _view.DestroyEvent += OnDestroy;
    }

    private void Unsubscribe()
    {
        _view.ExplosionEvent -= OnExplosion;
        _view.DestroyEvent -= OnDestroy;
    }

    private void OnExplosion()
    {
        ExplosionEvent?.Invoke(this);
        Unsubscribe();
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
    }
}
