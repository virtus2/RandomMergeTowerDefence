
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelTutorial;
    [SerializeField]
    private GameObject panelGameClear;
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private TextMeshProUGUI textGameHighScore;
    [SerializeField]
    private TextMeshProUGUI textMenuHighScore;
    [SerializeField]
    private GameObject panelGamePause;
    [SerializeField]
    private GameObject panelGameSpeed;
    [SerializeField]
    private GameObject mainGame;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private PlaySpeed playSpeed;
    private void Awake()
    {
        mainGame.SetActive(false);
        mainMenu.SetActive(true);
        if (PlayerPrefs.HasKey("HighScore"))
        {
            int highScore = PlayerPrefs.GetInt("HighScore");
            textMenuHighScore.text = "HighScore: " + highScore;
        }
        else
        {
            textMenuHighScore.text = "HighScore: 0";
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
        }
    }

    public void OnClickGameStart()
    {
        mainGame.SetActive(true);
        //mainMenu.SetActive(false);

        if (PlayerPrefs.HasKey("FirstPlay"))
        {
            PlayerPrefs.SetInt("FirstPlay", 1);
        }
        else
        {
            OnClickTutorial();
            PlayerPrefs.SetInt("FirstPlay", 1);
        }
    }

    public void OnEnemyCountOverMax(int wave)
    {
        playSpeed.PauseGame();
        panelGameOver.SetActive(true);
        if(PlayerPrefs.HasKey("HighScore"))
        {
            int highScore = PlayerPrefs.GetInt("HighScore");
            if (highScore <= wave)
            {
                textGameHighScore.text = "HighScore: " + wave;
                PlayerPrefs.SetInt("HighScore", wave);
                PlayerPrefs.Save();
            }
        }
        else
        {
            textGameHighScore.text = "HighScore: +" + wave;
            PlayerPrefs.SetInt("HighScore", wave);
            PlayerPrefs.Save();
        }
    }
    
    public void OnAllWaveFinished()
    {
        playSpeed.PauseGame();
        panelGameClear.SetActive(true);
    }
    #region Panel
    public void OnClickPanelGameOverClose()
    {
        playSpeed.ResumeGame();
        SceneManager.LoadScene(0);
    }
    public void OnClickPanelGameClearClose()
    {
        playSpeed.ResumeGame();
        SceneManager.LoadScene(0);
    }

    public void OnClickTutorial()
    {
        panelTutorial.SetActive(true);
    }

    public void OnClickPanelTutorialClose()
    {
        panelTutorial.SetActive(false);
    }

    public void OnClickPause()
    {
        playSpeed.PauseGame();
        panelGamePause.SetActive(true);
    }

    public void OnClickPanelPauseClose()
    {
        playSpeed.ResumeGame();
        panelGamePause.SetActive(false);
    }

    public void OnClickPlaySpeed()
    {
        if (playSpeed.watchedAd)
        {
            playSpeed.ChangePlaySpeed();
        }
        else
        {
            panelGameSpeed.SetActive(true);
        }
    }

    public void OnClickPlaySpeedYes()
    {
        playSpeed.ChangePlaySpeed();
        panelGameSpeed.SetActive(false);
    }

    public void OnClickPlaySpeedNo()
    {
        panelGameSpeed.SetActive(false);
    }
    #endregion Panel
}
