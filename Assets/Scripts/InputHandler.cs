using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

// Makes events with taps, so I can change or add an input method and nothing changes

public class InputHandler : GameSys
{
    public delegate void TapHandler();
    public static event TapHandler OnTap;
    public static event TapHandler OnTap2;
    public static event TapHandler OnTap3;
    public static event TapHandler OnRestart;

    int tapsCount = 0;

    private void Start()
    {
        ChainEnd.OnRestoreDefaults += Restart; // Just to avoid making one new method for default tapCount = 0
    }

    public void InvokeRestart()
    {
        OnRestart?.Invoke();
    }

    public override void GameOver()
    {
        tapsCount = 3;
    }

    public override void Restart()
    {
        tapsCount = 0;
    }

    void Tap()
    {
        if (tapsCount == 0)
        {
            OnTap?.Invoke();
            tapsCount++;
        }
        else if (tapsCount == 1)
        {
            OnTap2?.Invoke();
            tapsCount++;
        }
        else if (tapsCount == 2 && Player.Player.normalVector == Vector2.left)
        {
            OnTap3?.Invoke();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

#if UNITY_STANDALONE_WIN

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Tap();
        }

#endif

#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Tap();
            }
        }

#endif

    }
}


