using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityCard : MonoBehaviour
{
    public bool locked = false;
    public AbilityData data;
    public GameObject icon;
    public GameObject fontbox;
    public GameObject titleBox;
    public SpriteRenderer spriteArtPanel;
    public SpriteRenderer spriteArtBorder;
    public TextMeshPro headerTop;
    public TextMeshPro headerMiddle;
    public TextMeshPro headerBottom;
    public TextMeshPro body;
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleOn(){
        UpdateData();
        GetComponent<Renderer>().enabled = true;
    }
    public void ToggleOff(){
        if(!locked){
            Hide();
        }
    }
    public void ToggleLockedOn(){
        locked = true;
        GetComponent<Renderer>().enabled = true;
    }
    public void ToggleLockedOff(){
        locked = false;
        Hide();
    }
    public void UpdateData(){
        headerTop.SetText(data.headerTop);
        headerMiddle.SetText(data.headerMiddle);
        headerBottom.SetText(data.headerBottom);
        body.SetText(data.body);
        icon.GetComponent<Renderer>().enabled = true;
        spriteArtPanel.GetComponent<Renderer>().enabled = true;
        spriteArtBorder.GetComponent<Renderer>().enabled = true;
        spriteArtPanel.sprite = data.art;
        fontbox.GetComponent<Renderer>().enabled = true;
        titleBox.GetComponent<Renderer>().enabled = true;
    }
    public void Hide(){
        GetComponent<Renderer>().enabled = false;
        headerTop.SetText(" ");
        headerMiddle.SetText(" ");
        headerBottom.SetText(" ");
        body.SetText(" ");
        icon.GetComponent<Renderer>().enabled = false;
        spriteArtPanel.GetComponent<Renderer>().enabled = false;
        spriteArtBorder.GetComponent<Renderer>().enabled = false;
        fontbox.GetComponent<Renderer>().enabled = false;
        titleBox.GetComponent<Renderer>().enabled = false;

        
    }
    
}
