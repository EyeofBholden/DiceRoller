using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTrayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExpandScale(){
        transform.localScale += new Vector3(0.015f, 0.00f, 0.01f);
    }    
    public void ResetScale(){
        transform.localScale = new Vector3(1.5f, 9f, 1f);
    }
}
