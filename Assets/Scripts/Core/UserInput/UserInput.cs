using System;
using UnityEngine;
using VContainer.Unity;

public class UserInput : ITickable
{
    public event Action StartTouchEvent;
    public event Action EndTouchEvent;

    public void Tick()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartTouchEvent?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndTouchEvent?.Invoke();
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                StartTouchEvent?.Invoke();
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                EndTouchEvent?.Invoke();
            }
        }
#endif
    }
}
