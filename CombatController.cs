using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatController : MonoBehaviour
{
    public AbilityController abilityController;
    public GameManager gameManager;
    public EnemyController enemy;
    public PlayerController player;
    public AbilityCard abilityCard;
    public DiceReader diceReader;
    public GameObject diceTray;
    public FightButton button;
    public List<DiceScript> staleDice = new List<DiceScript>();
    public List<int> results = new List<int>();
    public List<DiceData> dice;
    public DiceData critDice;
    public DiceScript diceObject;
    public PlayerAbilityPreview playerAbilityPreview;
    public AbilityPreview enemyAbilityPreview;
    public AbilityData enemyAttack;
    public AbilityData playerAttack;
    private int playerAvdMod;
    private int playerArmMod;
    private int enemyAvdMod;
    private int enemyArmMod;
    private List<AbilityData> attacksToResolve = new List<AbilityData>();
    public List<string> diceIndexFinder;
    public bool waitingForRollResults;
    public int rollResults;
    public bool crit = false;
    public bool isCritBonus = false;
    public TextMeshPro readOut;
    Dictionary<string, int> playerCombatStats = new Dictionary<string, int>(){
        {"HIT", 0},
        {"POW", 0},
        {"AVD", 0},
        {"ARM", 0},
    };
    Dictionary<string, int> enemyCombatStats = new Dictionary<string, int>(){
        {"HIT", 0},
        {"POW", 0},
        {"AVD", 0},
        {"ARM", 0},
    };

    void Start(){
        updateCombatStats();
    }
    void Update(){
        if (Input.GetKeyDown (KeyCode.D)) {
            print(playerCombatStats["HIT"]);
            print(playerCombatStats["POW"]);
            print(playerCombatStats["AVD"]);
            print(playerCombatStats["ARM"]);
            print(enemyCombatStats["HIT"]);
            print(enemyCombatStats["POW"]);
            print(enemyCombatStats["AVD"]);
            print(enemyCombatStats["ARM"]);
		}
        if (!waitingForRollResults && rollResults == 0){
            rollResults = SumArray(results);
        }
    }
    public void DoCombat(string priority){
        attacksToResolve.Clear();
        print("starting Combat Logic");
            enemyAttack = enemyAbilityPreview.data;
            playerAttack = playerAbilityPreview.data;
            gameManager.playerSpendMana(playerAttack);
            gameManager.enemySpendMana(enemyAttack);
            modifyDefenses(playerAttack, enemyAttack);
            if (enemyAttack.speed == playerAttack.speed){
                if(priority == "enemy"){
                    attacksToResolve.Add(enemyAttack);
                    attacksToResolve.Add(playerAttack);
                    StartCoroutine(DoAttacking(attacksToResolve,enemyCombatStats,playerCombatStats));
                }
                else{
                    attacksToResolve.Add(playerAttack);
                    attacksToResolve.Add(enemyAttack);
                    StartCoroutine(DoAttacking(attacksToResolve,playerCombatStats,enemyCombatStats));
                }
            }
            else{
                if(enemyAttack.speed > playerAttack.speed){
                    attacksToResolve.Add(enemyAttack);
                    attacksToResolve.Add(playerAttack);
                    StartCoroutine(DoAttacking(attacksToResolve,enemyCombatStats,playerCombatStats));
                }
                else{
                    attacksToResolve.Add(playerAttack);
                    attacksToResolve.Add(enemyAttack);
                    StartCoroutine(DoAttacking(attacksToResolve,playerCombatStats,enemyCombatStats));
                }
            }
            abilityController.CheckMana();
    }
    IEnumerator DoAttacking(List<AbilityData> attacks, Dictionary<string, int> attacker, Dictionary<string, int> target){
        foreach(AbilityData attack in attacks){
            if(attack.type == "Attack"){
                List<string> statSet = new List<string>();

                statSet.Add(attack.statOne);
                statSet.Add(attack.statTwo);
                RollDiceSet(statSet, attack.critHit);
                
                yield return new WaitUntil(() => rollResults > 0);
                if((rollResults + attacker["HIT"] + attack.accuracy) >= target["AVD"] ){
                    readOut.SetText("Hit!: "+(rollResults + attacker["HIT"] + attack.accuracy).ToString() + " VS " + target["AVD"].ToString());
                    yield return new WaitForSeconds(1);
                    RollDiceSet(statSet, false);
                    yield return new WaitUntil(() => rollResults > 0);
                    int damage = rollResults + attacker["POW"] + attack.power - target["ARM"];
                    if(damage > 0 ){
                        readOut.SetText("Damage!: "+(rollResults + attacker["POW"] + attack.power).ToString() + " VS " + target["ARM"].ToString());
                    }
                    else{
                        readOut.SetText("Bounce!: "+(rollResults + attacker["POW"] + attack.power).ToString() + " VS " + target["ARM"].ToString());
                    }
                }else{
                    readOut.SetText("Miss!: "+(rollResults + attacker["HIT"] + attack.accuracy).ToString() + " VS " + target["AVD"].ToString());
                }
                yield return new WaitForSeconds(2);
            }
        }
        readOut.SetText("Turn Over");
        yield return new WaitForSeconds(1.5f);
        readOut.SetText("");
        cleanUp();
        yield return new WaitForSeconds(.5f);
        gameManager.turnStep = "Main One";
    }
    private void modifyDefenses(AbilityData player, AbilityData enemy){
        
        print(playerCombatStats["AVD"]);
        print(playerCombatStats["ARM"]);
        print(enemyCombatStats["AVD"]);
        print(enemyCombatStats["ARM"]);
        print(player.defense);
        playerAvdMod = player.defense;
        playerArmMod = player.armour;
        enemyAvdMod = enemy.defense;
        enemyArmMod = enemy.armour;
        playerCombatStats["AVD"] += playerAvdMod;
        playerCombatStats["ARM"] += playerArmMod;
        enemyCombatStats["AVD"] += enemyAvdMod;
        enemyCombatStats["ARM"] += enemyArmMod;
        print(playerCombatStats["AVD"]);
        print(playerCombatStats["ARM"]);
        print(enemyCombatStats["AVD"]);
        print(enemyCombatStats["ARM"]);
    }
    private void resetDefenses(){
        playerCombatStats["AVD"] -= playerAvdMod;
        playerCombatStats["ARM"] -= playerArmMod;
        enemyCombatStats["AVD"] -= enemyAvdMod;
        enemyCombatStats["ARM"] -= enemyArmMod;
        playerAvdMod = 0;
        playerArmMod = 0;
        enemyAvdMod = 0;
        enemyArmMod = 0;
    }
    public void updateCombatStats(){
        playerCombatStats["HIT"] = player.hit;
        playerCombatStats["POW"] = player.power;
        playerCombatStats["AVD"] = player.defense;
        playerCombatStats["ARM"] = player.armour;

        enemyCombatStats["HIT"] = enemy.hit;
        enemyCombatStats["POW"] = enemy.power;
        enemyCombatStats["AVD"] = enemy.defense;
        enemyCombatStats["ARM"] = enemy.armour;
    }
    public void RollDiceSet(List<string> stats, bool isCritBonus){
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
     void ClearTray(){
        for (int i = staleDice.Count - 1; i > -1; i--)
        {
            staleDice[i].killSelf();
            staleDice.RemoveAt(i);
        }
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
    private int SumArray(List<int> toBeSummed){
        int sum = 0;
        foreach (int item in toBeSummed)
        {
            sum += item;
        }
        return sum;
    }
    public void AddDiceResults(List<int> dieValues, bool isCrit){
        waitingForRollResults = false;
        abilityCard.ToggleLockedOff();
        results = dieValues;
        crit = isCrit;
    }
    private void cleanUp(){
        
        gameManager.playerAbilityLocked = false;
        gameManager.enemyAbilityLocked = false;
        abilityCard.ToggleLockedOff();
        enemyAbilityPreview.Hide();
        playerAbilityPreview.Hide();
        button.Toggle();
        button.abilityStaged = false;
        ClearTray();
    }
}
