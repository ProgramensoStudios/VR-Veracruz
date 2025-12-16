using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private FollowPointsManager patrolManager;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;
    private Transform _currentTarget;
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private ParticleSystem deathParticles;
    [Header("Vida")] [SerializeField] private int maxHealth = 100;
    
    // --- INTERNAS ---
    private NavMeshAgent _agent;
    private Transform _patrolTarget;
    [SerializeField] private FollowPointsManager.Zones zonaAsignada;
    private bool _isInvestigating = false;
    private float _investigateTimer = 0f;
    [SerializeField] private float investigateDuration = 5f;
    private Vector3 _investigatePosition;
    [SerializeField] private float destroyDelayAfterDeath = .5f;
    
    private int _currentHealth;

    
    private Animator _animator;
    private bool _isDead = false;
    
    // Events
    public static event Action OnDeath;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();  
        _animator = GetComponent<Animator>();
        _currentHealth = maxHealth;
        
    }

    private void Start()
    {
        SetRandomPatrolPoint();
    }
    private void Update()
    {
        if (_isDead) return;

        if (_currentTarget == null)
        {
            TryFindPlayer();

            if (_currentTarget == null)
            {
                if (_isInvestigating)
                {
                    Investigate();
                }
                else
                {
                    PatrolUpdate();
                }
            }
        }
        else
        {
            PursuePlayer();
        }
    }

    
    private void PursuePlayer()
    {
        if (_currentTarget == null) return;

        _agent.SetDestination(_currentTarget.position);

        Vector3 dir = (_currentTarget.position - transform.position);
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }
        
        float distance = Vector3.Distance(transform.position, _currentTarget.position);
        if (distance > detectionRange * 1.2f)
        {
            _currentTarget = null;
        }
    }
    
    private void Investigate()
    {
        _investigateTimer -= Time.deltaTime;

        if (_investigateTimer <= 0f)
        {
            _isInvestigating = false;
            return;
        }

        _agent.SetDestination(_investigatePosition);

        // Girar hacia la posición investigada
        Vector3 dir = (_investigatePosition - transform.position);
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
        }

        float distance = Vector3.Distance(transform.position, _investigatePosition);
        if (distance <= _agent.stoppingDistance + 0.5f)
        {
            _isInvestigating = false; // Llegó y no encontró nada
        }
    }
    
    private void TryFindPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        if (hits.Length > 0)
        {
            _currentTarget = hits[0].transform;
            _isInvestigating = false; // Ya lo encontró, no necesita investigar más
        }
    }
    
    private void PatrolUpdate()
    {
        if (_agent.pathPending) return;

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            SetRandomPatrolPoint();
        }
    }

    private void SetRandomPatrolPoint()
    {
        if (patrolManager == null) return;

        FollowPointsManager.Zone zona = patrolManager.zone.Find(z => z.zones == zonaAsignada);
        if (zona.points == null || zona.points.Count == 0) return;

        int randomIndex = Random.Range(0, zona.points.Count);
        _patrolTarget = zona.points[randomIndex];

        if (_patrolTarget != null)
        {
            _agent.SetDestination(_patrolTarget.position);
        }
    }
    

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        // Activamos modo investigación
        _investigatePosition = player.position; // Guardamos de dónde vino el ataque
        _isInvestigating = true;
        _investigateTimer = investigateDuration;
        _currentTarget = null;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;

        Debug.Log("Dead");

        // Stop movement
        _agent.isStopped = true;
        _agent.enabled = false;

        // Play death animation
        _animator.SetBool("isDead", true);

        StartCoroutine(WaitForDeathAnimation());
    }


    private IEnumerator WaitForDeathAnimation()
    {
        // Esperar a que entre al estado de muerte
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            yield return null;
        }

        // Esperar a que termine la animación
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        
        if (deathParticles != null)
        {
            Instantiate(deathParticles,transform.position,deathParticles.transform.rotation);
        }
        OnDeath?.Invoke();
        // Esperar EXTRA después de morir (cadáver en el suelo)
        yield return new WaitForSeconds(destroyDelayAfterDeath);
        Destroy(gameObject);
    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
