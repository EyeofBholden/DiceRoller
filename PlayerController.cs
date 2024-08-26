using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public GameManager gameManager;
    public CombatController combatController;
    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;
    public int initiative;
    public int hit;
    public int power;
    public int defense;
    public int armour;
    // public List<string> manaPool = new List<string>();
    public int genericMana;
    public int manaPerTurn;
    public List<AbilityData> currentMoveSet = new List<AbilityData>();
    public Dictionary<string, int> modifiers = new Dictionary<string, int>(){
        {"STR", 0},
        {"DEX", 0},
        {"CON", 0},
        {"INT", 0},
        {"WIS", 0},
        {"CHA", 0},
    };
    public Dictionary<string, int> manaPool = new Dictionary<string, int>(){
        {"ALL", 0},
        {"STR", 0},
        {"DEX", 0},
        {"CON", 0},
        {"INT", 0},
        {"WIS", 0},
        {"CHA", 0},
    };

    // Start is called before the first frame update
    void Start()
    {
        strength=18;
        dexterity=8;
        constitution=18;
        intelligence=7;
        wisdom=7;
        charisma=7;
        modifiers["STR"] = CalculateMod(strength);
        modifiers["DEX"] = CalculateMod(dexterity);
        modifiers["CON"] = CalculateMod(constitution);
        modifiers["INT"] = CalculateMod(intelligence);
        modifiers["WIS"] = CalculateMod(wisdom);
        modifiers["CHA"] = CalculateMod(charisma);
        hit = 6 + modifiers["DEX"];
        power = modifiers["STR"];
        defense = 11 + modifiers["DEX"];
        armour = 19;
        genericMana = 15;
        generateManaPool();
        combatController.updateCombatStats();
        initiative = modifiers["DEX"];
        gameManager.buildPlayerDeck();
        manaPerTurn = 7 + modifiers["DEX"] + modifiers["WIS"];
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown (KeyCode.D)) {
        //     print(manaPool["ALL"]);
        //     print(manaPool["STR"]);
        //     print(manaPool["DEX"]);
        //     print(manaPool["CON"]);
        //     print(manaPool["INT"]);
        //     print(manaPool["WIS"]);
        //     print(manaPool["CHA"]);
		// }
    }

    private int CalculateMod(int stat){
        int result = 0;
        if (stat >= 10){
            result += (stat-10)/2;
        }
        else{
            result += (10-stat)/-2;
        }
        return result;
    }

    private void generateManaPool(){
        genericMana -= Mathf.Abs(modifiers["STR"]);
        genericMana -= Mathf.Abs(modifiers["DEX"]);
        genericMana -= Mathf.Abs(modifiers["CON"]);
        genericMana -= Mathf.Abs(modifiers["INT"]);
        genericMana -= Mathf.Abs(modifiers["WIS"]);
        genericMana -= Mathf.Abs(modifiers["CHA"]);
        manaPool["ALL"] = genericMana;
        if(modifiers["STR"]>0){
            manaPool["STR"] = modifiers["STR"];
        }
        if(modifiers["DEX"]>0){
            manaPool["DEX"] = modifiers["DEX"];
        }
        if(modifiers["CON"]>0){
            manaPool["CON"] = modifiers["CON"];    
        }
        if(modifiers["INT"]>0){
            manaPool["INT"] = modifiers["INT"];
        }
        if(modifiers["WIS"]>0){
            manaPool["WIS"] = modifiers["WIS"];
        }
        if(modifiers["CHA"]>0){
            manaPool["CHA"] = modifiers["CHA"];
        }
    }
}
