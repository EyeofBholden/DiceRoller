using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dice", menuName = "Dice")]
public class DiceData : ScriptableObject
{
    public string stat;
    public Material diceMat;
    public int top;
    public int midRight;
    public int midLeft;
    public int farRight;
    public int farLeft;
    public int bottom;
}
