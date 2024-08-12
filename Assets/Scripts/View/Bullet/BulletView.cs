using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    public event Action HitEvent;
    public event Action DestroyEvent;

    [field: SerializeField] public BulletConfig Config { get; private set; }

    [SerializeField] private GameObject _bullet;
    [SerializeField] private ParticleSystem _explosionAnimation;

    private bool _isShooting = false;

    public void SetSize(float size)
    {
        transform.localScale = Vector3.one * size;
        transform.position = new Vector3(transform.position.x, size / 2, transform.position.z);
    }

    private void Update()
    {
        if (_isShooting)
        {
            transform.position += transform.forward * Config.Speed * Time.deltaTime;
        }
    }

    public void Shot()
    {
        _isShooting = true;
    }

    public async UniTask Explosion(float force)
    {
        _isShooting = false;

        _bullet.SetActive(false);
        var mainAnimation = _explosionAnimation.main;
        mainAnimation.duration = Config.TimeExplosion;
        var startSpeed = mainAnimation.startSpeed;
        startSpeed.constant = Config.RadiusExplosion * force / Config.TimeExplosion;
        mainAnimation.startSpeed = startSpeed;

        _explosionAnimation.Play();

        await UniTask.Delay(TimeSpan.FromSeconds(Config.TimeExplosion));
        if (this != null)
        {
            DestroyEvent?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Config.HitLayers.Contains(other.gameObject.layer))
        {
            HitEvent?.Invoke();
        }
    }
}
