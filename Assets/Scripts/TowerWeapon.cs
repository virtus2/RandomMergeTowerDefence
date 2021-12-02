using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public enum TowerType { arrow = 0, magic, gun, ice, cannon, COUNT}
public enum WeaponState { SearchTarget = 0, AttackTarget, Moving };
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private AudioControl audioControl;
    [SerializeField]
    private TowerTemplate towerTemplate;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private TowerMove towerMove;

    private TowerType towerType;
    private int level = 0;
    private int upgrade = 0;
    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private EnemySpawner enemySpawner;
    private SpriteRenderer spriteRenderer;
    private PlayerGold playerGold;
    private Tile ownerTile;

    public TowerType TowerType => towerTemplate.weapon[level].towerType;
    public string TowerName => towerTemplate.towerName;
    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    //public float Damage => towerTemplate.weapon[level].damage + (upgrade * towerTemplate.upgradeDamage);

    public float Damage => towerTemplate.weapon[level].damage + (upgrade * towerTemplate.weapon[level].upgradeDamage);
    public float BaseDamage => towerTemplate.weapon[level].damage;
    public float UpgradeDamage => upgrade * towerTemplate.weapon[level].upgradeDamage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int Level => level;
    public int Upgrade => upgrade;
    public int SellPrice => towerTemplate.weapon[level].sell;
    public int MaxLevel => towerTemplate.weapon.Length;

    public void Setup(EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile, int upgrade)
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.enemySpawner = enemySpawner;
        ChangeState(WeaponState.SearchTarget);
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        this.level = 0;
        this.upgrade = upgrade;
        this.spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
    }

    public void ChangeState(WeaponState newState)
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        if(attackTarget != null && !towerMove.IsMoving)
        {
            RotateToTarget();
        }
    }

    public void LevelUp()
    {
        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
    }

    public void UpgradeTower(int up)
    {
        upgrade = up;

    }

    public void Sell()
    {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        ownerTile.IsTowerBuilt = false;
        Destroy(gameObject);
    }

    private void RotateToTarget()
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }
    private IEnumerator Moving()
    {
        while(true)
        {
            if(!towerMove.IsMoving)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            yield return null;
        }
    }
    private IEnumerator SearchTarget()
    {
        while(true)
        {
            if (towerMove.IsMoving)
            {
                ChangeState(WeaponState.Moving);
                break;
            }
            float closestDistSqr = Mathf.Infinity;
            for(int i =0; i<enemySpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }
            if (attackTarget != null)
            {
                ChangeState(WeaponState.AttackTarget);
            }
            yield return null;
        }
    }
    private IEnumerator AttackTarget()
    {
        while(true)
        {
            if(towerMove.IsMoving)
            {
                ChangeState(WeaponState.Moving);
                break;
            }
            if(attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if(distance > towerTemplate.weapon[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            SpawnProjectile();
            audioControl.PlaySound();
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
        }
    }
    private void SpawnProjectile()
    {
        GameObject projectile = Lean.Pool.LeanPool.Spawn(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage+ (upgrade * towerTemplate.weapon[level].upgradeDamage));
    }
}
