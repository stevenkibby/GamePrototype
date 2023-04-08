using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
//Level requirements
    [SerializeField] int equipRequirement = 0;
    public int EquipRequirement { get { return equipRequirement;} }

    [SerializeField] int createRequirement = 0;
    public int CreateRequirement { get { return createRequirement;} }

    [SerializeField] int useRequirement = 0;
    public int UseRequirement { get { return useRequirement;} }
    
//Damage
    [SerializeField] float modifier = 0;
    public float Modifier { get { return modifier; } }

    [SerializeField] float maximumDamage = 0;
    public float MaximumDamage { get { return maximumDamage; } }

//Equipment
    [SerializeField] float maxHealth = 0;
    public float MaxHealth { get { return maxHealth; } }

    [SerializeField] float maxStamina = 0;
    public float MaxStamina { get { return maxStamina; } }

//Consumables
    [SerializeField] float healthRegen = 0;
    public float HealthRegen { get { return healthRegen; } }

    [SerializeField] float staminaRegen = 0;
    public float StaminaRegen { get { return staminaRegen; } }

    [SerializeField] float consumableTier = 0;
    public float ConsumableTier { get { return consumableTier; } }

    [SerializeField] int secondsTimer = 0;
    public int SecondsTimer { get { return secondsTimer; } }

//Compost value
    [SerializeField] float compostValue = 0;
    public float CompostValue { get { return compostValue; } }
}
