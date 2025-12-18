using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 3f;

    [Header("Homing")]
    [SerializeField] private float detectionRadius = 6f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private LayerMask enemyLayer;

    private float timer;
    private Transform target;

    private void OnEnable()
    {
        timer = lifetime;
        target = null;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Disable();
            return;
        }

        DetectEnemy();
        MoveBullet();
    }

    // Detecta enemigos cercanos
    private void DetectEnemy()
    {
        if (target != null) return;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            enemyLayer
        );

        if (hits.Length > 0)
        {
            target = hits[0].transform; // puedes mejorar esto buscando el más cercano
        }
    }

    // Movimiento
    private void MoveBullet()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotateSpeed * Time.deltaTime
            );
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }

        Disable();
    }

    private void Disable()
    {
        target = null;
        gameObject.SetActive(false);
    }

    // Gizmo para ver el rango de detección
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
