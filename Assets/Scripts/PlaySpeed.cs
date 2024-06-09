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
            // �̹� 4����� ���
            Time.timeScale = 1;
            speedBeforePause = 1;
            fastPlay = false;
            image.sprite = fast;
        }
        else if (watchedAd == false && fastPlay == false)
        {
            // ���� �Ⱥð� 1����� ���
            // googleAdMob.ShowRewardedInterstitialAd();
        }
        else if(watchedAd == true && fastPlay == false)
        {
            OnWatchedAd();
        }
    }

    public void OnWatchedAd()
    {
        // ���� �ٺ��� ������
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
