using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Bullet
{
    private BulletView _view;

    private Map _map;

    private float _size;

    public Bullet(BulletView view, Map map)
    {
        _view = view;
        _map = map;
    }

    public void SetSize(float size)
    {
        _size = size;
        _view.SetSize(size);
    }

    public void Shot()
    {
        Subscribe();
        _view.Shot();
    }

    private void OnHit()
    {
        _map.GetObstaclesInSphere(_view.transform.position, _view.Config.RadiusExplosion * _size).ForEach(collision => InfectCollision(collision).Forget());
        _view.Explosion(_size).Forget();
    }

    private void OnDestroy()
    {
        Unsubscribe();
        UnityEngine.Object.Destroy(_view.gameObject);
    }

    private async UniTask InfectCollision((float distance, Obstacle obstacle) collision)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_view.Config.TimeExplosion * collision.distance / _view.Config.RadiusExplosion));

        collision.obstacle.Infect().Forget();
    }

    private void Subscribe()
    {
        _view.HitEvent += OnHit;
        _view.DestroyEvent += OnDestroy;
    }

    private void Unsubscribe()
    {
        _view.HitEvent -= OnHit;
        _view.DestroyEvent -= OnDestroy;
    }
}
