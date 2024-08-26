using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    public GameManager gameManager;
    public CombatController combatController;
    public EnemyCard enemyCard;
    public MonsterData data;
    public AbilityPreview enemyAbilityCardPreview;
    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;
    public int hit;
    public int power;
    public int defense;
    public int armour;
    public int initiative;
    public int genericMana;
    public int manaPerTurn;
    public AbilityData pass;
    public List<AbilityData> moveset = new List<AbilityData>();
    public Dictionary<string, int> charges = new Dictionary<string, int>(){};
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
        strength=data.strength;
        dexterity=data.dexterity;
        constitution=data.constitution;
        intelligence=data.intelligence;
        wisdom=data.wisdom;
        charisma=data.charisma;
        modifiers["STR"] = CalculateMod(strength);
        modifiers["DEX"] = CalculateMod(dexterity);
        modifiers["CON"] = CalculateMod(constitution);
        modifiers["INT"] = CalculateMod(intelligence);
        modifiers["WIS"] = CalculateMod(wisdom);
        modifiers["CHA"] = CalculateMod(charisma);
        hit = 6 + modifiers["DEX"];
        power = modifiers["STR"];
        defense = 11 + modifiers["DEX"];
        armour = 12;
        genericMana = 15;
        generateManaPool();
        combatController.updateCombatStats();
        initiative = modifiers["DEX"];
        foreach(string chargeName in data.charges){
            charges[chargeName] = 0;
        }
        gameManager.buildEnemyDeck();
        manaPerTurn = 7 + modifiers["DEX"] + modifiers["WIS"];
        moveset = data.moveset.OrderBy(move => move.priority).ToList();
    }

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.E)) {
            print(charges["Claw"]);
            // print(manaPool["ALL"]);
            // print(manaPool["STR"]);
            // print(manaPool["DEX"]);
            // print(manaPool["CON"]);
            // print(manaPool["INT"]);
            // print(manaPool["WIS"]);
            // print(manaPool["CHA"]);
		}
    }

    public void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        enemyCard.GetComponent<Renderer>().enabled = true;
    }

    public void OnMouseExit()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        enemyCard.GetComponent<Renderer>().enabled = false;
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
    public void PlayAttack(){
        foreach(AbilityData move in moveset){
            print(move);
            print(move.chargeRequirement);
            if (!string.IsNullOrWhiteSpace(move.chargeRequirement)){
                if(charges[move.chargeRequirement] >= move.minimumcharges){
                    print(charges[move.chargeRequirement]);
                    print(move.minimumcharges);
                    if(CheckMana(move)){
                        spendCharges(move);
                        SetAttack(move);
                        return;
                    };
                }
            }
            else if(CheckMana(move)){
                chargeCheckCount(move);
                SetAttack(move);
                return;
            };
        }
        
        SetAttack(pass);
     
    }
    private bool CheckMana(AbilityData move){
        if(move.statOne == move.statTwo){
            if(manaPool[move.statOne] >= 2){
                manaPool[move.statOne] = manaPool[move.statOne] - 2;
                return true;
            }else if(manaPool[move.statOne] >= 1 && manaPool["ALL"] >= 1){
                manaPool[move.statOne] = manaPool[move.statOne] - 1;
                manaPool["ALL"] = manaPool["ALL"] - 1;
                return true;
            }
        }else if(manaPool[move.statOne] >= 1 && manaPool[move.statTwo] >= 1){
            manaPool[move.statOne] = manaPool[move.statOne] - 1;
            manaPool[move.statOne] = manaPool[move.statTwo] - 1;
            return true;
        }else if(manaPool[move.statOne] >= 1 && manaPool["ALL"] >= 1){
            manaPool[move.statOne] = manaPool[move.statOne] - 1;
            manaPool["ALL"] = manaPool["ALL"] - 1;
        }
        else if(manaPool["ALL"] >= 1 && manaPool[move.statTwo] >= 1){
            manaPool["ALL"] = manaPool["ALL"] - 1;
            manaPool[move.statOne] = manaPool[move.statTwo] - 1;
        }
        return false;
    }
    private void SetAttack(AbilityData move){
        enemyAbilityCardPreview.data = move;
        enemyAbilityCardPreview.UpdateData();
    }
    private void chargeCheckCount(AbilityData move){
        if(move.gratsCharge){
            if (charges.ContainsKey(move.chargeType)){
                charges[move.chargeType] = charges[move.chargeType] + 1;
            }
            else {
                // print("else");
                charges[move.chargeType] = 1;
            }
        }
    }
    private void spendCharges(AbilityData move){
        charges[move.chargeType] = charges[move.chargeType] - move.minimumcharges;
        print(charges[move.chargeType]);
    }
}

