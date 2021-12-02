using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }
public class Enemy : MonoBehaviour
{
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner;
    [SerializeField]
    private int gold;

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;
        transform.position = wayPoints[currentIndex].position;

        StartCoroutine("OnMove");   
    }
    private IEnumerator OnMove()
    {
        // 다음 이동 방향 설정
        NextMoveTo();
        while(true)
        {
            // 오브젝트 회전
            // 현재위치와 목표위치의 거리가 0.02 * movement2D.MoveSpeed 보다 작을때 if 조건문 실행
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.05f * movement2D.MoveSpeed)
            {
                // 다음 이동 방향 설정
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        /*
        if (currentIndex < wayPointCount - 1)
        {
            // 아직 이동할 wayPoints가 남았을경우
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            gold = 0;
            OnDie(EnemyDestroyType.Arrive);
        }
        */
        transform.position = wayPoints[currentIndex].position;
        if(currentIndex == wayPointCount-1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }
        Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
        movement2D.MoveTo(direction);
    }

    public void OnDie(EnemyDestroyType type)
    {
        enemySpawner.DestroyEnemy(type, this, gold);
    }
}
