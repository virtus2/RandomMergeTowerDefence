using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private float waitTime = 0f;
    private WaitForSeconds wait;
    private float elapsedTime = 0f;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private bool waveStarted = false;
    [SerializeField]
    private Wave lastWave;
    [SerializeField]
    private Wave[] waves;
    private int currentWaveIndex = -1;

    private int baseEnemyHP = 10;

    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;

    private void Awake()
    {

    }
    public float ElapsedTime
    {
        get => waitTime - elapsedTime;
    }
    private void Update()
    {
        if (waveStarted)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > waitTime)
            {
                elapsedTime = 0f;
                StartWave();
            }
        }
    }
    public void StartWave()
    {
        if(waveStarted)
        {
            // 시간남았는데도 진행 버튼 누르면 스킵
            elapsedTime = 0f;
        }
        else
        {
            // 처음 시작할때
            waveStarted = true;

        }
        if (currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            enemySpawner.StartWave(waves[currentWaveIndex]);
            systemTextViewer.PrintText(SystemType.NextWave);
        }
        else if(currentWaveIndex >= waves.Length)
        {
            lastWave.hp = 550 + currentWaveIndex * 30;
            currentWaveIndex++;
            enemySpawner.StartWave(lastWave);
            systemTextViewer.PrintText(SystemType.NextWave);
        }
    }

}
[System.Serializable]
public struct Wave
{
    public int hp;
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}
