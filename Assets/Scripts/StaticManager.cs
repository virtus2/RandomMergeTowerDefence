using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticManager : MonoBehaviour
{
    public static StaticManager Instance;
    [SerializeField]
    private float EnemyHitAlphaAnimationFadeSecondsFloat;
    public static WaitForSeconds EnemyHitAlphaAnimationFadeSeconds;

    private void Awake()
    {
        Instance = this;
        EnemyHitAlphaAnimationFadeSeconds = new WaitForSeconds(EnemyHitAlphaAnimationFadeSecondsFloat);
    }
}
