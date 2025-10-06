using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Camera cam;

    [Header("Settings")]
    [SerializeField] private float stoppingDistance = 0.5f;

    private PlayerStats playerStats;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        playerStats = GameManager.Instance.playerStats; // referencia PlayerStats
    }

    void Update()
    {
        // 🔹 Test: presiona K para ganar XP
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            if (playerStats != null)
                playerStats.AddExperience(50); // XP de prueba
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            agent.stoppingDistance = stoppingDistance;
            agent.SetDestination(hit.point);
        }
    }

    public bool IsMoving()
    {
        return agent.hasPath && agent.remainingDistance > agent.stoppingDistance;
    }
}