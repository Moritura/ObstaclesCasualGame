using System;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public event Action RestartEvent;

    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _deadPanel;

    [SerializeField] private Button _winRestart;
    [SerializeField] private Button _deadRestart;

    private void Start()
    {
        _winRestart.onClick.AddListener(() => RestartEvent?.Invoke());
        _deadRestart.onClick.AddListener(() => RestartEvent?.Invoke());
    }

    public void ShowWinPanel()
    {
        gameObject.SetActive(true);

        _winPanel.SetActive(true);
        _deadPanel.SetActive(false);
    }

    public void ShowDeadPanel()
    {
        gameObject.SetActive(true);
        
        _winPanel.SetActive(false);
        _deadPanel.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
