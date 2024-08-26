using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightButton : MonoBehaviour
{
    public TextMeshPro text;
    public bool abilityStaged = false;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Toggle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Toggle(){
        text.GetComponent<Renderer>().enabled = !text.GetComponent<Renderer>().enabled;
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
    }
    void OnMouseDown()
    {
        if(abilityStaged){
            gameManager.lockInPlayerAttack();
            //TEMP FOR TESTING
            gameManager.turnStep = "Main One";
        }
    }   
    
}
