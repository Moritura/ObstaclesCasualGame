using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

public class ObstacleView : MonoBehaviour
{
    public event Action ExplosionEvent;
    public event Action DestroyEvent;
 
    [field: SerializeField] public ObstacleConfig Config { get; private set; }
    
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private ParticleSystem _explosion;

    private CancellationTokenSource _cancellationTokenSource;

    private void Start()
    {
        _obstacle.transform.localScale = new Vector3(Config.Radius, _obstacle.transform.localScale.y, Config.Radius);
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Inject]
    public void Inject(Map map)
    {
        new Obstacle(this, map);
    }

    public async UniTask Infect(CancellationToken token)
    {
        _obstacle.GetComponent<MeshRenderer>().material = Config.HitMaterial;
        await UniTask.Delay(TimeSpan.FromSeconds(Config.DelayToExplosion), cancellationToken: token).SuppressCancellationThrow();
        if (token.IsCancellationRequested) return;

        _obstacle.SetActive(false);
        var mainExplosion = _explosion.main;
        mainExplosion.duration = Config.TimeExplosion;
        _explosion.Play();
        ExplosionEvent?.Invoke();
        await UniTask.Delay(TimeSpan.FromSeconds(Config.TimeExplosion));
    }

    private void OnDestroy()
    {
        DestroyEvent?.Invoke();
    }
}
