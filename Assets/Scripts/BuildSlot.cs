using UnityEngine;

public enum BuildType
{
    Torreta,
    Bombardera,
    Balleta,
    Granja
}

public class BuildSlot : MonoBehaviour
{
    [Header("Slot State")]
    public bool isOccupied = false;
    public GameObject currentTower;

    [Header("Visuals")]
    public Color freeColor = Color.green;
    public Color occupiedColor = Color.red;

    public BuildType buildType;
    public int Cost;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        UpdateColor();
    }

    public void PlaceTower(GameObject towerPrefab)
    {
        if (!isOccupied && towerPrefab != null)
        {
            currentTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
            isOccupied = true;
            UpdateColor();
        }
    }

    public void ShowInfo()
    {

    }
    public void PlaceTower(BuildType buildType)
    {
        //->buuild manager GameObject tower = Instantiate(GetBuildPrefab(buildType))

    }

    void UpdateColor()
    {
        if (rend != null)
            rend.material.color = isOccupied ? occupiedColor : freeColor;
    }

    void OnMouseDown()
    {
        if (!isOccupied && GameManager.Instance.CanBuild())
        {
            GameObject towerPrefab = GameManager.Instance.GetSelectedTower();
            PlaceTower(towerPrefab);
        }
    }
}