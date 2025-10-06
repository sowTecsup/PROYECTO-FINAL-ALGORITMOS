using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    public GameObject enemyPrefab;
    public Transform player;
    public int enemigosPorGrupo = 7;
    public float radioSpawn = 10f;
    public float intervaloEntreGrupos = 20f;
    public float intervaloEntreEnemigos = 0.5f; 

    private Queue<GameObject> colaDeSpawn = new Queue<GameObject>();
    private CustomLinkedList<GameObject> enemigosVivos = new CustomLinkedList<GameObject>();

    private bool puedeSpawnear = false;
    private float tiempoSiguienteGrupo = 0f;

    void Update()
    {
        if (!puedeSpawnear) return;

        tiempoSiguienteGrupo -= Time.deltaTime;
        if (tiempoSiguienteGrupo <= 0)
        {
            StartCoroutine(SpawnGrupoConDelay()); 
            tiempoSiguienteGrupo = intervaloEntreGrupos;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            EliminarTodos();
        }
    }

    private IEnumerator SpawnGrupoConDelay()
    {
        for (int i = 0; i < enemigosPorGrupo; i++)
        {
            GameObject enemigo = Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity);
            EnemyMovement mov = enemigo.GetComponent<EnemyMovement>();

            if (mov != null)
            {
                mov.target = player;
                mov.OnDeath += OnEnemyDeath;
            }

            colaDeSpawn.Enqueue(enemigo);
            enemigosVivos.Add(enemigo);

            yield return new WaitForSeconds(intervaloEntreEnemigos); 
        }

        Debug.Log($"Spawner {name} generó {enemigosPorGrupo} enemigos (uno cada {intervaloEntreEnemigos}s). Total vivos: {enemigosVivos.Count}");
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 pos = transform.position + Random.insideUnitSphere * radioSpawn;
        pos.y = 0;
        return pos;
    }

    private void OnEnemyDeath(GameObject enemigo)
    {
        enemigosVivos.Remove(enemigo);
        Debug.Log($" Enemigo eliminado. Quedan: {enemigosVivos.Count}");
    }

    public void SetActive(bool active)
    {
        puedeSpawnear = active;
        tiempoSiguienteGrupo = 0f;

        if (active)
            Debug.Log($"Spawner {name} ACTIVADO");
        else
            Debug.Log($"Spawner {name} DESACTIVADO");
    }

    public void ActualizarDificultad(int numeroOleada, int incremento)
    {
        enemigosPorGrupo += incremento;
        intervaloEntreGrupos = Mathf.Max(5f, intervaloEntreGrupos - 1f);

        Debug.Log($"Spawner {name}: dificultad actualizada. " +
                  $"Oleada {numeroOleada}, Enemigos por grupo: {enemigosPorGrupo}, Intervalo: {intervaloEntreGrupos}s");
    }

    private void EliminarTodos()
    {
        foreach (var enemigo in enemigosVivos.GetAll())
        {
            if (enemigo != null)
                Destroy(enemigo);
        }

        enemigosVivos.Clear();
        colaDeSpawn.Clear();
        Debug.Log(" Todos los enemigos fueron eliminados manualmente (tecla J).");
    }
}