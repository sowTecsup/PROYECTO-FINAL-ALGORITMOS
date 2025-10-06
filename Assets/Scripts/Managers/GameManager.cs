using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Torretas disponibles")]
    public GameObject[] towerPrefabs;
    private int selectedTowerIndex = 0;

    [Header("Referencias")]
    public AbilitySystem abilitySystem;
    public PlayerStats playerStats; // 👈 referencia PlayerStats desde el inspector

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanBuild()
    {
        return towerPrefabs.Length > 0;
    }

    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTowerIndex];
    }

    public void SelectTower(int index)
    {
        if (index >= 0 && index < towerPrefabs.Length)
            selectedTowerIndex = index;
    }
}