using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public GameState GameState { get; private set; } = GameState.Prepare;
    public ThirdPersonController mainCharacter;

    float playingTime;
    bool usedBoosterTime = false;
    bool usedBoosterSize = false;
    bool usedBoosterSpeed = false;

    void Start()
    {
        playingTime = 5;
        GameUIManager.Instance.UpdateTime(playingTime);
    }

    void Update()
    {
        if (GameState == GameState.Playing)
        {
            playingTime -= Time.deltaTime;
            GameUIManager.Instance.UpdateTime(playingTime);
            if (playingTime <= 0)
                EndGame();
        }
    }

    public void PlayGame()
    {
        GameState = GameState.Playing;
    }
    public void PauseGame()
    {
        GameState = GameState.Pause;
    }
    public void EndGame()
    {
        if (!usedBoosterTime)
        {
            GameState = GameState.Pause;
            PopupExtraTime.Instance.Show(delegate
            {
                playingTime += 20;
                GameState = GameState.Playing;
            }, delegate
            {
                GameState = GameState.End;
                GameUIManager.Instance.UpdateTime(0);
                PopupEndGame.Instance.Show(true, new GameReward());
            });
        }
        else
        {
            GameState = GameState.End;
            GameUIManager.Instance.UpdateTime(0);
            PopupEndGame.Instance.Show(true, new GameReward());
        }
    }

    public void UseBoosterTime()
    {
        playingTime += 15;
        GameUIManager.Instance.UpdateTime(playingTime);
    }
    public void UseBoosterSize()
    {
        usedBoosterSize = true;
        mainCharacter.InitStarterBooster(usedBoosterSize, false);
    }
    public void UseBoosterSpeed()
    {
        usedBoosterSpeed = true;
        mainCharacter.InitStarterBooster(false, usedBoosterSpeed);
    }
}

public enum GameState
{
    Prepare = 0,
    Playing = 1,
    Pause = 2,
    End = 3
}
