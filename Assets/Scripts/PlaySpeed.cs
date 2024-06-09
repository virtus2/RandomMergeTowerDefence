using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySpeed : MonoBehaviour
{
    [SerializeField]
    private Sprite fast;
    [SerializeField]    
    private Sprite normal;

    private Image image;
    private bool fastPlay = false;
    private float speedBeforePause = 1;
    public bool watchedAd = true;
    private void Awake()
    {
        fastPlay = false;
        image = GetComponent<Image>();
    }

    public void ChangePlaySpeed()
    {
        if (watchedAd && fastPlay)
        {
            // 이미 4배속일 경우
            Time.timeScale = 1;
            speedBeforePause = 1;
            fastPlay = false;
            image.sprite = fast;
        }
        else if (watchedAd == false && fastPlay == false)
        {
            // 광고 안봤고 1배속일 경우
            // googleAdMob.ShowRewardedInterstitialAd();
        }
        else if(watchedAd == true && fastPlay == false)
        {
            OnWatchedAd();
        }
    }

    public void OnWatchedAd()
    {
        // 광고 다보고 보상얻기
        watchedAd = true;
        Time.timeScale = 3;
        speedBeforePause = 3;
        fastPlay = true;
        image.sprite = normal;
    }

    public void PauseGame()
    {
        speedBeforePause = Time.timeScale;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = speedBeforePause;
    }
}
