using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PopupSystem;
using TMPro;
using DG.Tweening;

public class PopupEndGame : SingletonPopup<PopupEndGame>
{
    public GameObject winPanel, losePanel;
    //Win
    public TMP_Text tvWinReward;
    public TMP_Text tvBonus;
    public List<float> rotateBonusRange;
    public List<float> bonusRange;
    public Transform wheelTrans;
    //Lose
    public TMP_Text tvLoseReward;

    float rotateSpeed = 0;
    int rotateDirection = -1;
    bool isWheeling = false;
    bool isWheelDone = false;
    int rewardAmount;
    int bonusRewardAmount;

    public void Show(bool win, GameReward reward)
    {
        base.Show();
        winPanel.SetActive(win);
        losePanel.SetActive(!win);
        rewardAmount = reward.amount;
        tvWinReward.text = $"{rewardAmount}";
        tvLoseReward.text = $"{rewardAmount}";
        DoLuckyWheel();
    }

    void Update()
    {
        if (rotateSpeed <= 0)
            return;
        isWheeling = true;
        wheelTrans.Rotate(new Vector3(0, 0, Time.deltaTime * rotateSpeed * rotateDirection));
        float z = wheelTrans.localEulerAngles.z;
        if (z > 350)
        {
            rotateDirection = -1;
        }
        else if (z < 190)
        {
            rotateDirection = 1;
        }
        rotateSpeed -= Random.Range(150, 250) * Time.deltaTime;
        if (rotateSpeed <= 0)
        {
            rotateSpeed = 0;
            isWheelDone = true;
            isWheeling = false;

            float res = wheelTrans.localEulerAngles.z;
            int ind = rotateBonusRange.FindLastIndex(x => res < x);
            bonusRewardAmount = (int)(rewardAmount * bonusRange[ind]);
            tvBonus.text = $"{bonusRewardAmount}";
        }
    }

    void DoLuckyWheel()
    {
        rotateSpeed = Random.Range(800, 1100);
    }

    public void OnBonus()
    {
        if (!isWheelDone)
            return;
        Datamanager.Instance.UpdateGold(bonusRewardAmount);
    }
    public void OnBack()
    {
        base.Hide(delegate
        {
            Datamanager.Instance.UpdateGold(rewardAmount);
            GlobalSettings.Instance.ChangeScene(Const.SCENE_GAME);
        });
    }
}
