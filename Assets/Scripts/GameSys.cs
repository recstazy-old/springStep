using UnityEngine;
using Player;

// To control GameOvers and etc

public class GameSys : MonoBehaviour
{
    protected bool restarting = false;

    public GameObject gameOverPanel;

    void Awake()
    {
        InputHandler.OnRestart += Restart;
        ChainEnd.OnGameOver += GameOver;
    }

    public virtual void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public virtual void Restart()
    {
        restarting = true;
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
    }
}
