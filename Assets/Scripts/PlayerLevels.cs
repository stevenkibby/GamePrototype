using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevels : MonoBehaviour
{

//WMelee skills
    [SerializeField] int stabLevel = 1;
    public int StabLevel { get { return stabLevel;} }

    [SerializeField] int slashLevel = 1;
    public int SlashLevel { get { return slashLevel;} }

    [SerializeField] int crushLevel = 1;
    public int CrushLevel { get { return crushLevel;} }

//Ranged skills
    [SerializeField] int bowLevel = 1;
    public int BowLevel { get { return bowLevel;} }

    [SerializeField] int crossbowLevel = 1;
    public int CrossbowLevel { get { return crossbowLevel;} }

    [SerializeField] int thrownWeaponLevel = 1;
    public int ThrownWeaponLevel { get { return thrownWeaponLevel;} }

//Gathering skills
    [SerializeField] int woodcuttingLevel = 1;
    public int WoodcuttingLevel { get { return woodcuttingLevel;} }

    [SerializeField] int miningLevel = 1;
    public int MiningLevel { get { return miningLevel;} }

    [SerializeField] int fishingLevel = 1;
    public int FishingLevel { get { return fishingLevel;} }

    [SerializeField] int farmingLevel = 1;
    public int FarmingLevel { get { return farmingLevel;} }

//Processing skills
    [SerializeField] int smithingLevel = 1;
    public int SmithingLevel { get { return smithingLevel;} }

    [SerializeField] int cookingLevel = 1;
    public int CookingLevel { get { return cookingLevel;} }

    [SerializeField] int craftingLevel = 1;
    public int CraftingLevel { get { return craftingLevel;} }

    [SerializeField] int tamingLevel = 1;
    public int TamingLevel { get { return tamingLevel;} }
}
