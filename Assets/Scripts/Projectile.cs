using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType { Normal = 0, Slow, Splash, Burn }
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Movement2D movement2D;
    private Transform target;
    private float damage;
    [SerializeField]
    private ProjectileType projectileType;

    public void Setup(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        if(target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            Lean.Pool.LeanPool.Despawn(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") == false)
        {
            return;
        }
        if (collision.transform != target)
        {
            return;
        }

        switch (projectileType)
        {
            case ProjectileType.Normal:
                collision.GetComponent<EnemyHP>().TakeDamage(damage);
                //Lean.Pool.LeanPool.Despawn(this);
                break;
            case ProjectileType.Slow:
                collision.GetComponent<EnemyHP>().TakeDamage(damage);
                collision.GetComponent<Movement2D>().SlowMoveSpeed(0.75f, 1.5f, collision.gameObject);
                //Lean.Pool.LeanPool.Despawn(this);
                break;
            case ProjectileType.Splash:
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                for(int i=0; i<colliders.Length; i++)
                {
                    EnemyHP enemy = colliders[i].GetComponent<EnemyHP>();
                    if(enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                }
                break;
            default:
                break;
        }
        Lean.Pool.LeanPool.Despawn(this);

    }

}
