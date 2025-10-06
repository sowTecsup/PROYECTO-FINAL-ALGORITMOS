using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Slots disponibles")]
    public BuildSlot[] slots;

    public BuildSlot[] GetAllSlots()
    {
        return slots;
    }
}