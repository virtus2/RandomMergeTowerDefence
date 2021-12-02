using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerUpgrade towerUpgrade;
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    [SerializeField]
    private TowerDataViewer towerDataViewer;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private Tile[] tile;
    [SerializeField]
    private int buyGold;

    private TowerTemplate boughtTower = null;
    private Tile spawnTile = null;
    private Quaternion quaternion = new Quaternion(0, 0, 0, 0);

    public void OnClickBuyTower()
    {
        if (playerGold.CurrentGold < buyGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        // �������� ������� Ÿ�� �ϳ� ����
        boughtTower = towerTemplate[Random.Range(0, towerTemplate.Length)];
        // ������ Ÿ�� ����
        for(int i=0; i<tile.Length; i++)
        {
            if(tile[i].IsTowerBuilt == false)
            {
                spawnTile = tile[i];
                break;
            }
        }
        if (spawnTile == null)
        {

        }
        else
        {
            Vector3 position = new Vector3(spawnTile.transform.position.x, spawnTile.transform.position.y, -1);
            GameObject tower = Lean.Pool.LeanPool.Spawn(boughtTower.towerPrefab, position, quaternion, transform);
            
            TowerWeapon weapon = tower.GetComponent<TowerWeapon>();
            weapon.Setup(enemySpawner, playerGold, spawnTile, towerUpgrade.GetUpgrade(weapon.TowerType));
            tower.GetComponent<TowerMove>().Setup(towerDataViewer);
            spawnTile.IsTowerBuilt = true;
            spawnTile.OwnTower = tower;
            spawnTile = null;

            playerGold.CurrentGold -= buyGold;
        }
    }

    public void UpgradeTower(TowerType type)
    {
        int up = towerUpgrade.GetUpgrade(type);
        // ���� Ȱ��ȭ�� Ÿ���� ��������
        TowerWeapon[] weapon = GetComponentsInChildren<TowerWeapon>();
        for(int i=0; i<weapon.Length; ++i)
        {
            if((weapon[i].TowerType == type) && weapon[i].Upgrade <= up)
            {
                weapon[i].UpgradeTower(up);
            }
        }
    }


}
