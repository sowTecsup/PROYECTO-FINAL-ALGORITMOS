using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("XP y Nivel")]
    public int playerLevel = 1;
    public int experience = 0;
    public int experienceToNext = 100;
    public int skillPoints = 0;

    private GameManager gm;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log($"Ganaste {amount} XP (Total: {experience}/{experienceToNext})");

        if (experience >= experienceToNext)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        experience -= experienceToNext;
        playerLevel++;
        experienceToNext = Mathf.RoundToInt(experienceToNext * 1.5f);
        skillPoints++;

        Debug.Log($" Subiste a nivel {playerLevel}. Puntos disponibles: {skillPoints}");
    }

    public bool SpendSkillPoint(string abilityKey)
    {
        if (skillPoints > 0 && gm.abilitySystem != null)
        {
            if (gm.abilitySystem.TryUpgradeAbility(abilityKey, playerLevel))
            {
                skillPoints--;
                Debug.Log($"Mejoraste {abilityKey}. Puntos restantes: {skillPoints}");
                return true;
            }
            else
            {
                Debug.Log(" No puedes mejorar esa habilidad (nivel requerido o máximo alcanzado).");
            }
        }
        return false;
    }

    /*->PlayerManager
     * Platyer camera
     * Player movement
     * player stats
     * player abilities
     * 
     * 
     * 
     * 
     */
}