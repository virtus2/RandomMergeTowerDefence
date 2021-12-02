using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public string towerName;
    public GameObject towerPrefab;
    public int upgradeDamage; // ���׷��̵� �ܰ躰 �þ�� ������
    public Weapon[] weapon;

    [System.Serializable]
    public struct Weapon
    {
        public TowerType towerType;
        public Sprite sprite;
        public float damage;
        public float upgradeDamage;
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
