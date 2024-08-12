using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public class Game : IStartable, IDisposable
{
    private Player _player;
    private Gate _gate;
    private Path _path;

    private GameView _view;

    private bool _isEndState;

    public Game(Player player, Gate gate, Path path, GameView view)
    {
        _player = player;
        _gate = gate;
        _path = path;
        _view = view;

        _isEndState = false;
    }

    public void Start()
    {
        _view.Hide();
        Subscribe();
    }

    private void Subscribe()
    {
        _player.DeadEvent += OnDead;
        _player.MoveEvent += OnPlayerMove;
        _view.RestartEvent += OnRestart;
    }

    private void Unsubscribe()
    {
        _player.DeadEvent -= OnDead;
        _player.MoveEvent -= OnPlayerMove;
        _view.RestartEvent -= OnRestart;
    }

    private void OnPlayerMove()
    {
        Vector3 player = _player.GetPosition;
        player.y = 0f;
        Vector3 gate = _gate.GetPosition;
        gate.y = 0f;

        float distance = Vector3.Distance(player, gate);

        if (!_gate.IsOpen && distance < _gate.OpenDistance)
        {
            _gate.OpenGate();
        }

        if (distance < 0.1f && !_isEndState)
        {
            _isEndState = true;
            _path.Hide();
            _view.ShowWinPanel();
        }
    }
    
    private void OnDead()
    {
        _isEndState = true;
        _view.ShowDeadPanel();
    }

    private void OnRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void Dispose()
    {
        Unsubscribe();
    }
}
