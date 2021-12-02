using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject enemyHPSliderPrefab;
    [SerializeField]
    private Transform enemyHPSliderParent;
    [SerializeField]
    private Transform canvasTransform;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private WaveSystem waveSystem;
    private List<Enemy> enemyList;
    private Wave currentWave;
    private int currentEnemyCount;
    [SerializeField]
    private int maxEnemyCount;
    private int spawnEnemyCount;

    public List<Enemy> EnemyList => enemyList;
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;
    private void Awake()
    {
        enemyList = new List<Enemy>(maxEnemyCount);
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        spawnEnemyCount = 0;
        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Lean.Pool.LeanPool.Spawn(currentWave.enemyPrefabs[enemyIndex]);
            clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, -1);
            Enemy enemy = clone.GetComponent<Enemy>();
            EnemyHP enemyHP = clone.GetComponent<EnemyHP>();
            enemy.Setup(this, wayPoints);
            enemyHP.Setup(currentWave.hp);
            currentEnemyCount++;
            enemyList.Add(enemy);
            if(currentEnemyCount >= maxEnemyCount)
            {
                gameManager.OnEnemyCountOverMax(waveSystem.CurrentWave);
            }
            SpawnEnemyHPSlider(clone);
            spawnEnemyCount++;
            yield return new WaitForSeconds(currentWave.spawnTime);
        }    
    }    

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        if(type == EnemyDestroyType.Arrive)
        {
            playerHP.TakeDamage(1);
        }
        else if(type == EnemyDestroyType.Kill)
        {
            playerGold.CurrentGold += gold;
        }
        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
        
        if(CurrentEnemyCount == 0 && spawnEnemyCount == currentWave.maxEnemyCount)
        {
            // 다음웨이브로 스킵
            waveSystem.StartWave();
        }
    }

    public void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject slider = Lean.Pool.LeanPool.Spawn(enemyHPSliderPrefab, enemyHPSliderParent, false);
        //slider.transform.SetParent(canvasTransform);
        slider.transform.localScale = Vector3.one;

        slider.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        slider.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());

    }
}
