using UnityEngine;

public class EnemyMelee : EnemyBase
{
    [Header("Configuración melee")]
    public float meleeDetection = 3f;
    public float meleeAttack = 1.5f;
    public float meleeSpeed = 3.5f;

    protected override void Awake()
    {
        base.Awake();
        enemyName = "Melee";
        health = 150;
        damage = 10;

        if (movement != null)
        {
            movement.detectionRadius = meleeDetection;
            movement.attackRadius = meleeAttack;
            movement.speed = meleeSpeed;
        }
    }

    public override void Attack()
    {
        Debug.Log($"{enemyName} ataca cuerpo a cuerpo con {damage} de daño.");
    }
}
