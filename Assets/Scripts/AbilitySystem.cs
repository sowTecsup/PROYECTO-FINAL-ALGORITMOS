using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum AbilityType
{
    None,
    PrimaryAb,
    SecondaryAb,
    ThirdAb,
    Ultimate
}

public class AbilitySystem : MonoBehaviour
{
    //->
    public Dictionary<AbilityType, Ability> abilities2 = new Dictionary<AbilityType, Ability>();
    public Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GameManager.Instance.playerStats; // referencia PlayerStats

        Ability q = new Ability(" Bola de Fuego", 3f, 5);
        Ability w = new Ability(" Escudo", 5f, 5);
        Ability e = new Ability(" Dash", 2f, 5);
        Ability r = new Ability(" Ulti Explosiva", 10f, 3);

        q.OnCast = () => Debug.Log(" Bola de fuego lanzada!");
        w.OnCast = () => Debug.Log(" Escudo activado!");
        e.OnCast = () => Debug.Log(" Dash hacia adelante!");
        r.OnCast = () => Debug.Log(" ULTI EXPLOSIVA!");

        abilities["Q"] = q;
        abilities["W"] = w;
        abilities["E"] = e;
        abilities["R"] = r;
    }
    public void OnAbilityQ(InputAction.CallbackContext ctx) { if (ctx.performed) TryCast("Q"); }
    public void OnAbilityW(InputAction.CallbackContext ctx) { if (ctx.performed) TryCast("W"); }
    public void OnAbilityE(InputAction.CallbackContext ctx) { if (ctx.performed) TryCast("E"); }
    public void OnAbilityR(InputAction.CallbackContext ctx) { if (ctx.performed) TryCast("R"); }

    public void OnUpgradeQ(InputAction.CallbackContext ctx) { if (ctx.performed) playerStats?.SpendSkillPoint("Q"); }
    public void OnUpgradeW(InputAction.CallbackContext ctx) { if (ctx.performed) playerStats?.SpendSkillPoint("W"); }
    public void OnUpgradeE(InputAction.CallbackContext ctx) { if (ctx.performed) playerStats?.SpendSkillPoint("E"); }
    public void OnUpgradeR(InputAction.CallbackContext ctx) { if (ctx.performed) playerStats?.SpendSkillPoint("R"); }

    private void TryCast(string key)
    {
        if (abilities.ContainsKey(key))
            abilities[key].Cast();
    }

    public void UpgradeAbility(string key)
    {
        if (abilities.ContainsKey(key))
            abilities[key].Upgrade();
    }

    public bool TryUpgradeAbility(string key, int playerLevel)
    {
        if (!abilities.ContainsKey(key)) return false;

        Ability ability = abilities[key];

        if (key == "R" && playerLevel < 5)
        {
            Debug.Log(" La R se desbloquea en nivel 5.");
            return false;
        }

        if (!ability.Locked && ability.Level >= ability.MaxLevel)
        {
            Debug.Log($" {ability.Name} ya está en el nivel máximo.");
            return false;
        }

        ability.Upgrade();
        return true;
    }
}