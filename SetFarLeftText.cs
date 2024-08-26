using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetFarLeftText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text face;
    void Start()
    {
        face.text = GetComponentInParent<DiceScript>().diceNumbers[1].ToString();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
