using System;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public static Action onEnemyEnter;
    public static Action onEnemyExit;
    public int enemyInRange = 0;

    private void OnTriggerEnter(Collider other)
    {
        enemyInRange += 1;
        if (other.gameObject.layer == 9)
            onEnemyEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        enemyInRange -= 1;
        if (enemyInRange != 0) return;
        if (other.gameObject.layer == 9)
            onEnemyExit?.Invoke();
    }
}