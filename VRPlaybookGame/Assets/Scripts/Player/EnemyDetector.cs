using System;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public static Action onEnemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            onEnemy.Invoke();
        }
    }
}
