using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScene : MonoBehaviour
{
    public Slider loadingProgress;

    int step = 0;

    void Start()
    {
        StartCoroutine(LoadSceneMain());
        StartCoroutine(LoadingAsset());
    }

    IEnumerator LoadSceneMain()
    {
        int lastStep = 0;
        int percent = 0;
        bool isLoad = false;
        while (lastStep <= 10)
        {
            if (lastStep < step)
            {
                Debug.Log(lastStep);
                if (!isLoad)
                {
                    isLoad = true;
                    DOTween.To(() => percent, x => percent = x, lastStep * 10, 0.35f).OnUpdate(delegate
                    {
                        loadingProgress.value = percent;
                    }).OnComplete(delegate
                    {
                        lastStep += 1;
                        isLoad = false;
                    }).SetUpdate(true);
                }
                yield return null;
            }
            yield return null;
        }
        while (!Datamanager.Exists && !Datamanager.Instance.IsLoadDone)
            yield return new WaitForSecondsRealtime(0.5f);
        GlobalSettings.Instance.ChangeScene(Const.SCENE_GAME);
    }
    IEnumerator LoadingAsset()
    {
        step = 0;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.FirebaseManager));
        step = 1;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.AdsManager));
        step = 2;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.DataConfig));
        step = 4;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.PopupManager));
        step = 5;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.InputManager));
        step = 6;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.SoundManager));
        step = 7;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.GameAssetManager));
        step = 8;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.QuestManager));
        step = 9;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.DataManager));
        step = 10;
        yield return new WaitForSeconds(0.35f);
        yield return StartCoroutine(InstantiateAsset(Const.GlobalSettings));
        step = 11;
    }
    private IEnumerator<bool> InstantiateAsset(string path)
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        while (!request.isDone)
        {
            yield return false;
        }
        Instantiate(request.asset);
        Debug.Log($"Load {path} success");
        yield return true;
    }
}
