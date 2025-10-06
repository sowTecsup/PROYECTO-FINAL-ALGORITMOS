using System.Collections.Generic;
using UnityEngine;

public static class EnemyManager
{
    public static List<EnemyMovement> Enemigos = new List<EnemyMovement>();

    public static void Registrar(EnemyMovement enemigo)
    {
        if (!Enemigos.Contains(enemigo))
            Enemigos.Add(enemigo);
    }

    public static void Desregistrar(EnemyMovement enemigo)
    {
        if (Enemigos.Contains(enemigo))
            Enemigos.Remove(enemigo);
    }
}