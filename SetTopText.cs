using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetTopText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text face;
    void Start()
    {
        face.text = GetComponentInParent<DiceScript>().diceNumbers[5].ToString();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
