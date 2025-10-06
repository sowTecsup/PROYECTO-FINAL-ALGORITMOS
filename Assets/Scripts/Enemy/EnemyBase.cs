using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public string enemyName;
    public int health;
    public int damage;

    [Header("Movimiento y radios")]
    public EnemyMovement movement;

    protected virtual void Awake()
    {
        if (movement == null)
            movement = GetComponent<EnemyMovement>();
    }

    public abstract void Attack();

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{enemyName} recibe {amount} de daño. Vida restante: {health}");
        if (health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log($"{enemyName} ha muerto.");
        movement.Die();
    }
}
