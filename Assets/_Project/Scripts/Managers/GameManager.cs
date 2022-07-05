using System;
using System.Collections;
using GameAnalyticsSDK;
using UnityEngine;

public enum GameStatus { Null, Playing, Win,Finish, GameOver }

public delegate void Action();
public class GameManager : Manager<GameManager>
{

    public Action GameOverAction, PlayAction, WinAction,FinishAction;
    public static bool isGameStarted;

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
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,LevelManager.Instance.currentLevel.ToString());
        PlayAction?.Invoke();
    }
  
    public void GameOver()
    {
        
        if (GameStatus == GameStatus.GameOver || GameStatus == GameStatus.Win) return;
        
        GameStatus = GameStatus.GameOver;
        StartCoroutine(WaitEnd());
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail,LevelManager.Instance.currentLevel.ToString());
    }

    private IEnumerator WaitEnd()
    {
        PlayerController.OnStopParticle?.Invoke();
        yield return new WaitForSeconds(1f);
        UIManager.Instance.LoseActivate(true);
        GameOverAction?.Invoke();
        Invoke("Reload", 1.5f);
        DataManager.Instance.MyBlockCount = 0;
    }
    public void Win()
    {
        if (GameStatus == GameStatus.Win || GameStatus == GameStatus.GameOver) return;
        
        GameStatus = GameStatus.Win;
        UIManager.Instance.WinActivate(true);
        
        WinAction?.Invoke();
        UpgradeManager.Instance.Reset("Speed");
        UpgradeManager.Instance.Reset("Size");
        UpgradeManager.Instance.Reset("Damage");
        UpgradeManager.Instance.Reset("Rolling Time");
        DataManager.Instance.MyBlockCount = 0;
        
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,LevelManager.Instance.currentLevel.ToString());

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
