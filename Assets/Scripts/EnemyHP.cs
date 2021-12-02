using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    private bool isDead = false;
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrnetHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void Setup(float hp)
    {
        maxHP = hp;
        currentHP = maxHP;
    }
    

    public void TakeDamage(float damage)
    {
        if (isDead == true)
        {
            return;
        }
        currentHP -= damage;
        //StopCoroutine("HitAlphaAnimation");
        //StartCoroutine("HitAlphaAnimation");
        if(currentHP<=0)
        {
            isDead = true;
            enemy.OnDie(EnemyDestroyType.Kill);
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        Color color = spriteRenderer.color;
        color.a = 0.4f;
        spriteRenderer.color = color;
        yield return StaticManager.EnemyHitAlphaAnimationFadeSeconds;
   
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
