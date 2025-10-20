using System;
using UnityEngine;

public class Ability
{
    public string Name;
    public float Cooldown;
    private float lastCastTime;
    public int Level { get; private set; }
    public int MaxLevel;
    public bool Locked = true; 

    public Action OnCast;

    public Ability(string name, float cooldown, int maxLevel = 5)
    {
        Name = name;
        Cooldown = cooldown;
        lastCastTime = -cooldown;
        Level = 0;
        MaxLevel = maxLevel;
    }

    //public bool CanCast() => !Locked && Level > 0 && Time.time >= lastCastTime + Cooldown;
    public bool CanCast()
    {
        return !Locked && Level > 0 && Time.time >= lastCastTime + Cooldown;
    }

    public void Cast()
    {
        if (CanCast())
        {
            lastCastTime = Time.time;
            Debug.Log($" {Name} lanzada (Nivel {Level})");
            OnCast?.Invoke();
            return;
        }

        if (Locked || Level == 0)
        {
            Debug.Log($" {Name} está bloqueada.");
            return;
        }
        
        float remaining = (lastCastTime + Cooldown) - Time.time;
        Debug.Log($" {Name} en cooldown. Faltan {remaining:F1}s");
        
        
    }

    public void Upgrade()//->considerar overhull a sistema con grafos
    {
        if (Locked)
        {
            Locked = false;
            Level = 1; 
            Debug.Log($" {Name} desbloqueada en nivel {Level}!");
            return;
        }

        if (Level < MaxLevel)
        {
            Level++;
            Cooldown = Mathf.Max(0.5f, Cooldown * 0.9f);
            Debug.Log($" {Name} mejorada a nivel {Level}, CD: {Cooldown:F1}s");
        }
        else
        {
            Debug.Log($" {Name} ya está al máximo nivel!");
        }
    }
}