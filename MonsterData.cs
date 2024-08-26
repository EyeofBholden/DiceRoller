using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster")]
public class MonsterData : ScriptableObject
{
    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;
    public List<string> charges = new List<string>();
    public List<AbilityData> moveset = new List<AbilityData>();
}