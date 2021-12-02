using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textName;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textSell;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private GameObject panelHelp;
    private TowerWeapon currentTower;

    private string[] level;
    private void Awake()
    {
        HidePanel();
        level = new string[5];
        level[0] = "D";
        level[1] = "C";
        level[2] = "B";
        level[3] = "A";
        level[4] = "S";
        
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.HideAttackRange();
        panelHelp.SetActive(false);
    }
    public void ShowPanel(Transform towerWeapon)
    {
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerData();
        towerAttackRange.ShowAttackRange(currentTower.transform.position, currentTower.Range);
    }

    private void UpdateTowerData()
    {
        //////// 데미지 출력 ////////////
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(currentTower.BaseDamage);
        stringBuilder.Append(" + <color=#3E8948>");
        stringBuilder.Append(currentTower.UpgradeDamage);
        stringBuilder.Append("</color>");
        //////// 레벨 출력 /////////////
        textName.text = currentTower.TowerName;
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text = stringBuilder.ToString();
        textRate.text = currentTower.Rate.ToString();
        textRange.text = currentTower.Range.ToString();
        textLevel.text = level[currentTower.Level];
        textSell.text = currentTower.SellPrice.ToString();
    }

    public void OnClickTowerUpgrade()
    {
        if (currentTower == null)
            return;
        UpdateTowerData();
        //towerAttackRange.ShowAttackRange(currentTower.transform.position, currentTower.Range);

    }

    public void OnClickTowerSell()
    {
        currentTower.Sell();
        HidePanel();
    }

    public void OnClickHelpButton()
    {
        panelHelp.SetActive(true);
    }

    public void OnClickHelpCloseButton()
    {
        panelHelp.SetActive(false);
    }

}
