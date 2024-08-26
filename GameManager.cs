using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public AbilityCard abilityCard;
    public DiceReader diceReader;
    public GameObject diceTray;
    public List<DiceScript> staleDice = new List<DiceScript>();
    public List<int> results = new List<int>();
    public List<DiceData> dice;
    public List<string> diceIndexFinder;
    public DiceData critDice;
    public DiceScript diceObject;
    public AbilityPreview playerAbilityPreview;
    public AbilityController abilityController;
    public FightButton button;
    public CombatController combatController;
    public bool waitingForRollResults;
    public int rollResults;
    public bool crit = false;
    public bool isCritBonus = false;
    public PlayerController player;
    public EnemyController enemy;
    public string priority;
    public List<string> manaDeck;
    public List<string> manaDiscard;
    public TextMeshPro strValue;
    public TextMeshPro dexValue;
    public TextMeshPro conValue;
    public TextMeshPro intValue;
    public TextMeshPro wisValue;
    public TextMeshPro chaValue;
    public TextMeshPro universalValue;
    public bool playerSet;
    public bool enemySet;
    public string turnStep = "setup";
    // private bool waitingOnPlayer = false;
    public bool playerAbilityLocked = false;
    public bool enemyAbilityLocked = false;
    public Dictionary<string, int> manaHand = new Dictionary<string, int>(){
        {"ALL", 0},
        {"STR", 0},
        {"DEX", 0},
        {"CON", 0},
        {"INT", 0},
        {"WIS", 0},
        {"CHA", 0},
    };
    public List<string> enemyManaDeck;
    public List<string> enemyManaDiscard;
    public Dictionary<string, int> enemyManaHand = new Dictionary<string, int>(){
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
        RollForInitiative();
        print("priority to: "+ priority);
        generateAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space)) {
            crit = false;
            rollResults = 0;
            waitingForRollResults = true;
            diceReader.NewRoll(6, isCritBonus);
            diceReader.Reset();
            ClearTray();
            List<string> rainbow = new List<string>();
            rainbow.Add("STR"); // adding elements using add() method
            rainbow.Add("DEX");
            rainbow.Add("CON");
            rainbow.Add("INT");
            rainbow.Add("WIS");
            rainbow.Add("CHA");
            SpawnDice(rainbow, isCritBonus);
		}
         if (Input.GetKeyDown (KeyCode.H)) {
            print(manaHand["ALL"]);
            print(manaHand["STR"]);
            print(manaHand["DEX"]);
            print(manaHand["CON"]);
            print(manaHand["INT"]);
            print(manaHand["WIS"]);
            print(manaHand["CHA"]);
		}
        if (!waitingForRollResults && rollResults == 0){
            rollResults = SumArray(results);
        }
        if (turnStep == "setup" && playerSet && enemySet){
            turnStep = "Main One";
            DrawForTurn();
        }
        else if (turnStep == "Main One" && priority == "player" && !enemyAbilityLocked){
            enemy.PlayAttack();
            enemyAbilityLocked = true;
        }else if (turnStep == "Main One" && playerAbilityLocked && !enemyAbilityLocked){
            enemy.PlayAttack();
            enemyAbilityLocked = true;
        }
        if (turnStep == "Main One" && playerAbilityLocked && enemyAbilityLocked){
            turnStep = "Combat";
            combatController.DoCombat(priority);
        }
                
    }
    public void generateAbilities(){
        foreach(AbilityData data in player.currentMoveSet){
            print("generateAbilities");
            abilityController.populateAbility(data);
        }
    }
    public void RollDiceSet(List<string> stats, bool isCritBonus = false){
        if (!waitingForRollResults){
            crit = false;
            rollResults = 0;
            waitingForRollResults = true;
            diceReader.NewRoll(stats.Count, isCritBonus);
            diceReader.Reset();
            ClearTray();
            SpawnDice(stats, isCritBonus);
        }
    }
    private int SumArray(List<int> toBeSummed){
            int sum = 0;
            foreach (int item in toBeSummed)
            {
                sum += item;
            }
            return sum;
        }
    void SpawnDice(List<string> stats, bool useCrit = false){
        foreach(string stat in stats){
            int index = diceIndexFinder.FindIndex(a => a.Contains(stat));
            DiceScript newdice = Instantiate(diceObject);
            newdice.dice = dice[index];
            staleDice.Add(newdice);
        }
        if(useCrit){
            DiceScript critdice = Instantiate(diceObject);
            critdice.dice = critDice;
            staleDice.Add(critdice);
        }
    }
    void ClearTray(){
        for (int i = staleDice.Count - 1; i > -1; i--)
        {
            staleDice[i].killSelf();
            staleDice.RemoveAt(i);
        }
    }
    public void AddDiceResults(List<int> dieValues, bool isCrit){
        waitingForRollResults = false;
        abilityCard.ToggleLockedOff();
        results = dieValues;
        crit = isCrit;
    }
    private void RollForInitiative(){
        int playerRoll = player.initiative + Random.Range(1, 7);
        int enemyRoll = enemy.initiative + Random.Range(1, 7);
        if (playerRoll > enemyRoll){
            priority = "player";
        }
        else if(enemyRoll > playerRoll){
            priority = "enemy";
        }
        else{
            if(player.dexterity > enemy.dexterity){
                priority = "player";
            }
            else if(enemy.dexterity > player.dexterity){
                priority="enemy";
            }
            else{
                RollForInitiative();
            }
        }
    }
    public void buildPlayerDeck(){
        foreach(var item in player.manaPool){
            if (item.Value > 0){
                for (int i = 0; i < item.Value; i++)
                {
                    manaDeck.Add(item.Key);
                }
            }
        }
        shufflePlayerDeck();
        playerSet=true;
    }
    public void shufflePlayerDeck(){
        List<string> tempList = new List<string>();
        while (manaDeck.Count > 0){
            int index = Random.Range(0, manaDeck.Count);
            tempList.Add(manaDeck[index]);
            manaDeck.RemoveAt(index);
        }
        manaDeck = tempList;
    }
    public void buildEnemyDeck(){
        foreach(var item in enemy.manaPool){
            if (item.Value > 0){
                for (int i = 0; i < item.Value; i++)
                {
                    enemyManaDeck.Add(item.Key);
                }
            }
        }
        shuffleEnemyDeck();
    }
    public void shuffleEnemyDeck(){
        List<string> tempList = new List<string>();
        while (enemyManaDeck.Count > 0){
            int index = Random.Range(0, enemyManaDeck.Count);
            tempList.Add(enemyManaDeck[index]);
            enemyManaDeck.RemoveAt(index);
        }
        enemyManaDeck = tempList;
        enemySet=true;
    }
    public void DrawForTurn(){
        playerDrawMana(player.manaPerTurn);
        enemyDrawMana(enemy.manaPerTurn);
    }
    public void UpdateManaChart(){
        strValue.SetText(manaHand["STR"].ToString());
        dexValue.SetText(manaHand["DEX"].ToString());
        conValue.SetText(manaHand["CON"].ToString());
        intValue.SetText(manaHand["INT"].ToString());
        wisValue.SetText(manaHand["WIS"].ToString());
        chaValue.SetText(manaHand["CHA"].ToString());
        universalValue.SetText(manaHand["ALL"].ToString());
        abilityController.CheckMana();
    }
    public void playerDrawMana(int amount){
        for(int i = 0; i < amount; i++){
            manaHand[manaDeck[0]] = manaHand[manaDeck[0]] + 1;
            manaDeck.RemoveAt(0);
        }
        UpdateManaChart();
    }
    public void enemyDrawMana(int amount){
        for(int i = 0; i < amount; i++){
            enemyManaHand[enemyManaDeck[0]] = enemyManaHand[enemyManaDeck[0]] + 1;
            enemyManaDeck.RemoveAt(0);
        }
    }
    public void playerSpendMana(AbilityData data){
        if (manaHand[data.statOne] > 0){
            manaHand[data.statOne] = manaHand[data.statOne] - 1;
        } 
        else{
            manaHand["ALL"] = manaHand["ALL"] - 1;
        }
        if (manaHand[data.statTwo] > 0){
            manaHand[data.statTwo] = manaHand[data.statTwo] - 1;
        } 
        else{
            manaHand["ALL"] = manaHand["ALL"] - 1;
        }
        UpdateManaChart();
    }
    public void enemySpendMana(AbilityData data){
        if (enemyManaHand[data.statOne] > 0){
            enemyManaHand[data.statOne] = enemyManaHand[data.statOne] - 1;
        } 
        else{
            enemyManaHand["ALL"] = enemyManaHand["ALL"] - 1;
        }
        if (enemyManaHand[data.statTwo] > 0){
            enemyManaHand[data.statTwo] = enemyManaHand[data.statTwo] - 1;
        } 
        else{
            enemyManaHand["ALL"] = enemyManaHand["ALL"] - 1;
        }
    }
    
    public void lockInPlayerAttack(){
        playerAbilityLocked = true;
    }
}
