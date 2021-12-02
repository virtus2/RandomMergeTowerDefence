using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    private float baseMoveSpeed;
    private bool slow = false;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    private SpriteRenderer spriteRenderer;

    public float MoveSpeed => moveSpeed;
    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
        slow = false;
        spriteRenderer = null;
    }
    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }    

    public void SlowMoveSpeed(float value, float duration, GameObject enemy)
    {
        if (slow) return;
        slow = true;
        moveSpeed -= value;
        spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.blue;
        Invoke("ChangeMoveSpeed", duration);
    }
    private void ChangeMoveSpeed()
    {
        slow = false;
        moveSpeed = baseMoveSpeed;
        spriteRenderer.color = Color.white;
    }

}
