using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public PlayerController player;
    public GameManager gameManager;
    public List<Ability> activeAbilities = new List<Ability>();
    private List<Vector3> abilityPositions = new List<Vector3>();
    public AbilityCard abilityCard;
    public PlayerAbilityPreview playerAbilityPreview;
    public FightButton button;
    public CombatController combatController;
    public Ability baseAbility;
    public Transform min, max;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = 2.2f;
    }

    // Update is called once per frame
    void Update()
    {
        SetAbilityPositions();
    }
    public void SetAbilityPositions()
    {
        abilityPositions.Clear();
        
        Vector3 midPoint = (max.position + min.position)/2;
        float listMid = activeAbilities.Count/2;
        if(activeAbilities.Count % 2 == 0){
            float leftMid = listMid - 1;
            float rightMid = listMid;
            for(int i = 0; i <= activeAbilities.Count-1; i++){
                Vector3 mod = midPoint;
                mod.x += -1 * ((rightMid * 2) - 1 - (i * 2)) * offset/2;
                abilityPositions.Add(mod);
                
            }
        }
        else{
            for(int i = 0; i <= activeAbilities.Count-1; i++){
                Vector3 mod = midPoint;
                
                mod.x += (i-Mathf.Floor(listMid)) * offset;
                abilityPositions.Add(mod);   
            }
            
            
        }
        for(int i = 0; i <= activeAbilities.Count-1; i++){
            activeAbilities[i].transform.rotation = min.rotation;
            activeAbilities[i].transform.position = abilityPositions[i];
            
        }
    }
    public void populateAbility(AbilityData move){
        Ability newAbility = Instantiate(baseAbility, transform.position, transform.rotation);
        newAbility.data = move;
        newAbility.gameManager = gameManager;
        newAbility.playerAbilityPreview = playerAbilityPreview;
        newAbility.button = button;
        newAbility.abilityCard = abilityCard;
        newAbility.combatController = combatController;
        newAbility.player = player;
        activeAbilities.Add(newAbility);
        print("populateAbility");
    }

    public void CheckMana(){
        foreach(Ability ability in activeAbilities){
            ability.checkAvailableMana();
        }
    }
}
