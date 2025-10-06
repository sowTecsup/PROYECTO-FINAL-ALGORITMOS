using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform target;

    [Header("Radios y velocidad asignados desde el prefab o script")]
    public float detectionRadius = 5f;
    public float attackRadius = 2f;
    public float speed = 3f;

    [Header("Separación entre enemigos")]
    public float separationDistance = 1.2f;
    public float separationForce = 3f;

    private NavMeshAgent agent;

    private enum EnemyState { Walking, Chasing, Attacking }
    private EnemyState currentState;

    private Vector3 walkDestination;
    private bool hasWalkDestination = false;

    public event System.Action<GameObject> OnDeath;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0f;
        agent.speed = speed;

        EnemyManager.Registrar(this);
    }

    void OnDestroy()
    {
        EnemyManager.Desregistrar(this);
    }

    void Update()
    {
        if (target == null)
        {
            WalkForward();
            EvitarSuperposicion();
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRadius)
            currentState = EnemyState.Attacking;
        else if (distanceToTarget <= detectionRadius)
            currentState = EnemyState.Chasing;
        else
            currentState = EnemyState.Walking;

        switch (currentState)
        {
            case EnemyState.Walking: WalkForward(); break;
            case EnemyState.Chasing:
                agent.isStopped = false;
                agent.SetDestination(target.position);
                break;
            case EnemyState.Attacking:
                agent.isStopped = true;
                break;
        }

        EvitarSuperposicion();
    }

    public void SetWalkDestination(Vector3 destination)
    {
        walkDestination = destination;
        hasWalkDestination = true;
        if (agent != null)
        {
            agent.SetDestination(walkDestination);
            agent.isStopped = false;
        }
    }

    private void WalkForward()
    {
        if (!hasWalkDestination) return;

        if (Vector3.Distance(transform.position, walkDestination) > 0.1f)
        {
            agent.isStopped = false;
            agent.SetDestination(walkDestination);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void EvitarSuperposicion()
    {
        Vector3 empuje = Vector3.zero;
        int contador = 0;

        for (int i = 0; i < EnemyManager.Enemigos.Count; i++)
        {
            EnemyMovement otro = EnemyManager.Enemigos[i];
            if (otro == this) continue;

            float dist = Vector3.Distance(transform.position, otro.transform.position);
            if (dist < separationDistance)
            {
                Vector3 dir = (transform.position - otro.transform.position).normalized;
                empuje += dir / dist;
                contador++;
            }
        }

        if (contador > 0)
        {
            empuje /= contador;
            Vector3 nuevaPos = transform.position + empuje * separationForce * Time.deltaTime;
            agent.Move(nuevaPos - transform.position);
        }
    }

    public virtual void Die()
    {
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        // Color según estado
        switch (currentState)
        {
            case EnemyState.Walking: Gizmos.color = Color.gray; break;
            case EnemyState.Chasing: Gizmos.color = Color.blue; break;
            case EnemyState.Attacking: Gizmos.color = Color.red; break;
        }

        // Dibujar radio de detección
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        // Dibujar radio de ataque
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
