using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField]
    TowerSpawner towerSpawner;
    [SerializeField]
    TowerDataViewer towerDataViewer;
    int[] towerUpgrade;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Button[] buttonUpgrade;
    [SerializeField]
    TextMeshProUGUI[] textUpgrade;
    [SerializeField]
    TextMeshProUGUI[] textUpgradeGold;
    [SerializeField]
    PlayerGold playerGold;
    [SerializeField]
    SystemTextViewer systemTextViewer;

    [SerializeField]
    private int upgradeGold = 15;
    private bool isPanelActive;
    private void Awake()
    {
        towerUpgrade = new int[(int)TowerType.COUNT];
        for(int i=0; i<(int)TowerType.COUNT; i++)
        {
            towerUpgrade[i] = 0;
        }
        for (int i = 0; i < (int)TowerType.COUNT; i++)
        {
            textUpgradeGold[i].text = upgradeGold.ToString();
        }
        for (int i=0; i<(int)TowerType.COUNT; i++)
        {
            int t = i;
            buttonUpgrade[i].onClick.AddListener(() => OnClickUpgrade((TowerType)(t)));
        }
        HideUpgradePanel();
    }
    public void ShowUpgradePanel()
    {
        if (isPanelActive)
        {
            panel.SetActive(false);
            isPanelActive = false;
        }
        else
        {
            panel.SetActive(true);
            isPanelActive = true;
        }
    }

    public void HideUpgradePanel()
    {
        panel.SetActive(false);
        isPanelActive = false;
    }

    private void UpdateUpgradePanel()
    {

    }

    public void OnClickUpgrade(TowerType type)
    {
        if(playerGold.CurrentGold < upgradeGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        else
        {
            playerGold.CurrentGold -= upgradeGold;
            towerUpgrade[(int)type]++;
            textUpgrade[(int)type].text = towerUpgrade[(int)type].ToString();
            towerSpawner.UpgradeTower(type);
            towerDataViewer.OnClickTowerUpgrade();
        }

    }

    public int GetUpgrade(TowerType type)
    {
        return towerUpgrade[(int)type];
    }
}
