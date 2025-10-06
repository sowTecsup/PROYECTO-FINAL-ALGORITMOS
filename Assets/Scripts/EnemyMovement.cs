using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float stoppingDistance = 2.5f;
    public float separationDistance = 1.2f;
    public float separationForce = 3f;

    private NavMeshAgent agent;

    public event System.Action<GameObject> OnDeath;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        EnemyManager.Registrar(this);
    }

    void OnDestroy()
    {
        EnemyManager.Desregistrar(this);
    }

    void Update()
    {
        if (target == null) return;

        float distancia = Vector3.Distance(transform.position, target.position);

        if (distancia > stoppingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.isStopped = true;
        }

        EvitarSuperposicion();
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
    public void Die()
    {
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }
}