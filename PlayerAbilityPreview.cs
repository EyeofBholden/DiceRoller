using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityPreview : MonoBehaviour
{   
    public AbilityData data;
    public Ability playerAttack;
    public SpriteRenderer spriteArtPanel;
    public SpriteRenderer spriteArtBorder;
    public bool hidden = true;
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Toggle(){
        if(hidden){
            UpdateData();
        }
        else{
            Hide();
        }
    }
    public void UpdateData(){
        GetComponent<Renderer>().enabled = true;
        spriteArtPanel.GetComponent<Renderer>().enabled = true;
        spriteArtBorder.GetComponent<Renderer>().enabled = true;
        spriteArtPanel.sprite = data.art;
        hidden = false;
    }
    public void Hide(){
        hidden = true;
        GetComponent<Renderer>().enabled = false;
        spriteArtPanel.GetComponent<Renderer>().enabled = false;
        spriteArtBorder.GetComponent<Renderer>().enabled = false;
        
    }
}
