using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{

    public void HideAttackRange()
    {
        gameObject.SetActive(false);
    }

    public void ShowAttackRange(Vector3 position, float range)
    {
        gameObject.SetActive(true);
        float diameter = range * 2.0f; // Áö¸§
        transform.localScale = Vector3.one * diameter;
        transform.position = position;
    }

}
