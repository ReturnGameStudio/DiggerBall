using System;
using System.Collections;
using UnityEngine;

public enum GameStatus { Null, Playing, Win, Finish, GameOver }

public delegate void Action();
public class GameManager : Manager<GameManager>
{

    public Action GameOverAction, PlayAction, WinAction, FinishAction;
    // public static bool isGameStarted;

    [Header("Game Status")]
    public GameStatus GameStatus;

    [Header("Player")]
    public Transform Player;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        base.Awake();
    }

    private void Start()
    {
        GameStatus = GameStatus.Null;
    }

    #region  Game Status

    public bool IsPlaying()
    {
        return GameStatus == GameStatus.Playing;
    }

    public void Play()
    {
        GameStatus = GameStatus.Playing;
        UIManager.Instance.MenuActivate(false);

        PlayAction?.Invoke();
    }

    public void GameOver()
    {
        if (GameStatus == GameStatus.GameOver || GameStatus == GameStatus.Win) return;

        GameStatus = GameStatus.GameOver;
        DataManager.Instance.BlockCount = 0;
        StartCoroutine(WaitEnd());
    }

    private IEnumerator WaitEnd()
    {
        PlayerController.OnStopParticle?.Invoke();
        yield return new WaitForSeconds(1f);
        UIManager.Instance.LoseActivate(true);
        GameOverAction?.Invoke();
        Invoke("Reload", 1.5f);
    }
    public void Win()
    {
        if (GameStatus == GameStatus.Win || GameStatus == GameStatus.GameOver) return;

        GameStatus = GameStatus.Win;
        UIManager.Instance.WinActivate(true);

        WinAction?.Invoke();
        DataManager.Instance.BlockCount = 0;
    }


    public void Finish()
    {
        if (GameStatus == GameStatus.Finish) return;
        GameStatus = GameStatus.Finish;

        if (FinishAction != null)
            FinishAction.Invoke();
    }

    public void Reload()
    {
        LevelManager.Instance.LoadCurrentLevel();
    }


    #endregion


}
