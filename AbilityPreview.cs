using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPreview : MonoBehaviour
{   
    public AbilityData data;
    public Ability enemyAttack;
    public SpriteRenderer spriteArtPanel;
    public SpriteRenderer spriteArtBorder;
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateData(){
        GetComponent<Renderer>().enabled = true;
        spriteArtPanel.GetComponent<Renderer>().enabled = true;
        spriteArtBorder.GetComponent<Renderer>().enabled = true;
        spriteArtPanel.sprite = data.art;
    }
    public void Hide(){
        GetComponent<Renderer>().enabled = false;
        spriteArtPanel.GetComponent<Renderer>().enabled = false;
        spriteArtBorder.GetComponent<Renderer>().enabled = false;
        
    }
}
