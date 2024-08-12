using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class Player : IDisposable
{
    public event Action ChangeSizeEvent;
    public event Action DeadEvent;
    public event Action MoveEvent;

    private UserInput _input;
    private Gate _gate;
    private Map _map;

    private PlayerView _view;

    private CancellationTokenSource _actionCancellationTokenSource;
    private Bullet _currentBullet;

    private float _size;
    private float _moveDistance;
    private PlayerState _state;
    private PlayerState State
    {
        get => _state;
        set
        {
            if (IsEndState) return;
            if (_state == value) return;
            if ((_state == PlayerState.Shooting || _state == PlayerState.Moving) && (value == PlayerState.Shooting || value == PlayerState.Moving)) return;

            if (value == PlayerState.Idle && _moveDistance > 0f)
            {
                value = PlayerState.Moving;
            }

            _state = value;

            DisposeCancellationTokenSource();

            switch (_state)
            {
                case PlayerState.Moving:
                    Moving(_actionCancellationTokenSource.Token).Forget();
                    break;
                
                case PlayerState.Shooting:
                    Shooting(_actionCancellationTokenSource.Token).Forget();
                    break;

                case PlayerState.Dead:
                    DeadEvent?.Invoke();
                    break;
            }
        }
    }

    public float Size => _size;
    public Vector3 GetPosition => _view.transform.position;
    public bool IsEndState => State == PlayerState.Dead || State == PlayerState.Win;

    public Player(PlayerView view, UserInput input, Gate gate, Map map)
    {
        _view = view;
        _input = input;
        _gate = gate;
        _map = map;

        _size = _view.Config.StartSize;
        _moveDistance = 0f;

        _actionCancellationTokenSource = new CancellationTokenSource();
        _state = PlayerState.Idle;

        _view.transform.LookAt(new Vector3(_gate.GetPosition.x, GetPosition.y, _gate.GetPosition.z));

        Subscribe();
    }

    public void Move(float distance)
    {
        _moveDistance = distance;
        if (_moveDistance > 0f)
        {
            State = PlayerState.Moving;
        }
    }

    public void Win()
    {
        State = PlayerState.Win;
    }

    private async UniTask Moving(CancellationToken token)
    {
        while (_moveDistance > 0f && !token.IsCancellationRequested)
        {
            float deltaDistance = _view.Config.MoveSpeed * Time.deltaTime;

            if (_moveDistance < deltaDistance)
            {
                deltaDistance = _moveDistance;
            }

            _moveDistance -= deltaDistance;

            _view.transform.position += _view.transform.forward * deltaDistance;

            MoveEvent?.Invoke();

            await UniTask.Yield();
        }

        if (!token.IsCancellationRequested)
        {
            State = PlayerState.Idle;
        }
    }

    private void Subscribe()
    {
        _input.StartTouchEvent += OnStartTouch;
        _input.EndTouchEvent += OnEndTouch;
    }
    private void Unsubscribe()
    {
        _input.StartTouchEvent -= OnStartTouch;
        _input.EndTouchEvent -= OnEndTouch;
    }

    private void OnStartTouch()
    {
        State = PlayerState.Shooting;
    }

    private void OnEndTouch()
    {
        if (State == PlayerState.Shooting)
        {
            State = PlayerState.Idle;
        }
    }

    private async UniTask Shooting(CancellationToken token)
    {
        float bulletSize = _view.Config.MinBulletSize;
        _currentBullet = CreateBullet();
        _currentBullet.SetSize(bulletSize);

        UpdatePlayerSize(CalculateVolume(bulletSize));


        while (!token.IsCancellationRequested)
        {
            float deltaSize = _view.Config.ShotStepSize * Time.deltaTime;

            bulletSize += deltaSize;
            _currentBullet.SetSize(bulletSize);

            UpdatePlayerSize(CalculateVolume(bulletSize + deltaSize) - CalculateVolume(bulletSize));

            await UniTask.Yield();
        }

        _currentBullet.Shot();
        _currentBullet = null;
    }

    private float CalculateVolume(float radius)
    {
        return 4f / 3f * Mathf.PI * Mathf.Pow(radius, 3);
    }

    private float CalculateRadius(float volume)
    {
        return Mathf.Pow(volume * 3f / 4f / Mathf.PI, 1f / 3f);
    }

    private Bullet CreateBullet()
    {
        BulletView bulletView = GameObject.Instantiate(_view.Config.BulletView, _view.transform.position, _view.transform.rotation);
        bulletView.transform.position = _view.transform.position + bulletView.transform.forward * _view.Config.ShotDistance;

        Bullet bullet = new Bullet(bulletView, _map);

        return bullet;
    }

    private void UpdatePlayerSize(float deltaVolume)
    {
        float newVolume = CalculateVolume(_size) - deltaVolume * _view.Config.ReductionFactor;
        float newRadius = Mathf.Max(CalculateRadius(newVolume), 0f);

        SetSize(newRadius);
    }

    private void SetSize(float size)
    {
        if (IsEndState) return;

        if (size > _view.Config.DeadSize)
        {
            _size = size;
            _view.ChangeSize(size);
            ChangeSizeEvent?.Invoke();
        }
        else
        {
            State = PlayerState.Dead;
        }
    }

    private void DisposeCancellationTokenSource()
    {
        _actionCancellationTokenSource.Cancel();
        _actionCancellationTokenSource.Dispose();
        _actionCancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        DisposeCancellationTokenSource();
        Unsubscribe();
    }
}
