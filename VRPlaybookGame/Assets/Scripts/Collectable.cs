using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static Action onCollect;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            onCollect.Invoke();
            Destroy(gameObject);
        }
    }
}
