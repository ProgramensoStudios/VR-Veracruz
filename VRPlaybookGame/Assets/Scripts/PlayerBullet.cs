using UnityEngine;
using System.Collections;

public class BulletPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;

    private float timer;



    private void OnEnable()
    {
        speed = 20;
        timer = lifetime;
        MoveBullet();
    }

    private void Update()
    {
        MoveBullet();
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Disable();
        }
    }

    private void MoveBullet()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enem))
        {
            enem.TakeDamage(damage);
        }
        speed = 0;
        Disable();
    }
    

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}