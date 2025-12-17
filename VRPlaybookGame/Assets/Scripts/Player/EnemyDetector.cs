using System;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public static Action onEnemy;
    public static Action onRelax;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            onEnemy.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            onRelax.Invoke();
        }
    }
}
