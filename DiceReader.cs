using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceReader : MonoBehaviour
{
    public List<int> rollResults = new List<int>();
    public List<int> rollCounter = new List<int>();
    public DiceTrayController diceTray;
    public int result;
    public bool reported;
    public GameManager gameManager;
    public CombatController combatController;
    public int expectedCount = 0;
    public int critResult = 0;
    public bool isCrit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rollCounter.Count == expectedCount && !reported){
            reported = true;
            // if(rollResults.Contains(critResult)){
            //     isCrit = true;
            // }
            // if(rollResults[0] == rollResults[1]){
            //     isCrit = true;
            // }
            // gameManager.Report(SumArray(rollResults));
            combatController.AddDiceResults(rollResults, isCrit);
            
        }
    }
    void OnTriggerStay(Collider col)
    {
        
        if (col.gameObject.GetComponentInParent<DiceScript>().settled){
            if(!col.gameObject.GetComponentInParent<DiceScript>().recorded){
                col.gameObject.GetComponentInParent<DiceScript>().recorded = true;
                switch(col.gameObject.name){
                    case "Bottom":
                        result = col.gameObject.GetComponentInParent<DiceScript>().diceNumbers[5];
                        break;
                    case "FarLeft":
                        result = col.gameObject.GetComponentInParent<DiceScript>().diceNumbers[4];
                        break;
                    case "FarRight":
                        result = col.gameObject.GetComponentInParent<DiceScript>().diceNumbers[3];
                        break;
                    case "MidLeft":
                        result = col.gameObject.GetComponentInParent<DiceScript>().diceNumbers[2];
                        break;
                    case "MidRight":
                        result = col.gameObject.GetComponentInParent<DiceScript>().diceNumbers[1];
                        break;
                    case "Top":
                        result = col.gameObject.GetComponentInParent<DiceScript>().diceNumbers[0];
                        break;
                    default:
                        
                        break;
                }
                if (col.gameObject.GetComponentInParent<DiceScript>().stat == "CRIT"){
                    critResult = result;
                    rollCounter.Add(result);
                }
                else{
                    rollResults.Add(result);
                    rollCounter.Add(result);
                }
                
            }
        }
        
    }
    public void Reset(){
        rollResults.Clear();
        rollCounter.Clear();
    }
    public void NewRoll(int count, bool useCrit){
        expectedCount = count;
        if(useCrit){
            expectedCount++;
        }
        reported = false;
        isCrit = false;
        diceTray.ResetScale();
        StartCoroutine(Nudger());

    }
    IEnumerator Nudger(){
        while(!reported){
            yield return new WaitForSeconds(4);
            diceTray.ExpandScale();
        }
        yield break;
    }
}
