using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public PlayerController player;
    public GameManager gameManager;
    public AbilityCard abilityCard;
    public AbilityData data;
    public PlayerAbilityPreview playerAbilityPreview;
    public FightButton button;
    public CombatController combatController;
    public string element;
    public string statOne;
    public string statTwo;
    public bool critHit = false;
    public int power;
    public int accuracy;
    public int defense;
    public int armour;
    public List<string> statSet = new List<string>();
    public SpriteRenderer spriteArtPanel;
    public int speed;
    public bool oom;
    // Start is called before the first frame update
    void Start()
    {
        element = data.element;
        statOne = data.statOne;
        statTwo = data.statTwo;
        power = data.power;
        accuracy = data.accuracy;
        defense = data.defense;
        armour = data.armour;
        statSet.Add(statOne);
        statSet.Add(statTwo);
        spriteArtPanel.sprite = data.art;
        speed = data.speed;

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnMouseOver()
    {
        if(!oom){
            abilityCard.ToggleOn();
            abilityCard.data = data;
        }
    }

    public void OnMouseExit()
    {
        
        abilityCard.ToggleOff();
    }
    void OnMouseDown()
    {   
        if(!oom){
            playerAbilityPreview.data = data;
            playerAbilityPreview.Toggle();
            button.abilityStaged = !button.abilityStaged;
            button.Toggle();
            // combatController.RollDiceSet(statSet);
            abilityCard.ToggleLockedOn();
        }
    }
    public void checkAvailableMana(){
        oom = false;
        spriteArtPanel.material.SetFloat("_GrayscaleAmount", 0);
        if(statOne == statTwo){
            if(gameManager.manaHand[statOne] < 2 || (gameManager.manaHand[statOne] < 1 && gameManager.manaHand["ALL"] < 1))
                oom = true;
                spriteArtPanel.material.SetFloat("_GrayscaleAmount", 1);
            
        }else if(gameManager.manaHand[statOne] < 1 && gameManager.manaHand[statTwo] < 1){
            oom = true;
            spriteArtPanel.material.SetFloat("_GrayscaleAmount", 1);
        }else if(gameManager.manaHand[statOne] < 1 && gameManager.manaHand["ALL"] < 1){
            oom = true;
            spriteArtPanel.material.SetFloat("_GrayscaleAmount", 1);
        }   
        else if(gameManager.manaHand["ALL"] < 1 && gameManager.manaHand[statTwo] < 1){
            oom = true;
            spriteArtPanel.material.SetFloat("_GrayscaleAmount", 1);
        }
    }

}
