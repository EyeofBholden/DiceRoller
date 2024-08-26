using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class AbilityData : ScriptableObject
{
    public string element;
    public string statOne;
    public string statTwo;
    public int power;
    public int accuracy;
    public int defense;
    public int armour;
    public Sprite art;
    public string headerTop;
    public string headerMiddle;
    public string headerBottom;
    public string body;
    public int priority;
    public bool gratsCharge;
    public string chargeType;
    public string chargeRequirement;
    public int minimumcharges;
    public int speed;
    public bool critHit;
    public bool multiAttack;
    public int multiCount;
    public bool stun;
    public int stunValue;
    public string type;
}
