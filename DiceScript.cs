using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour {

	public DiceData dice;
	static Rigidbody rb;
	public GameObject GameManager;
	public Vector3 diceVelocity;
	public float posX;
	public float posY;
	public float posZ;
	public bool settled;
	public float rotX;
	public float rotY;
	public float rotZ;
	private float timer;
	public bool recorded;
	public string stat;
	public List<int> diceNumbers = new List<int>();

	// Use this for initialization
	void Start () {
		diceNumbers.Add(dice.top);
		diceNumbers.Add(dice.midRight);
		diceNumbers.Add(dice.midLeft);
		diceNumbers.Add(dice.farRight);
		diceNumbers.Add(dice.farLeft);
		diceNumbers.Add(dice.bottom);
		stat = dice.stat;
		gameObject.GetComponent<Renderer>().material = dice.diceMat;
		settled = false;
		recorded = false;
		rb = GetComponent<Rigidbody> ();
		RollDice();
		timer = 7f;
	}
	
	// Update is called once per frame
	void Update () {
		rotX = Mathf.RoundToInt(transform.eulerAngles.x) % 90;
		rotY = Mathf.RoundToInt(transform.eulerAngles.y) % 90;
		rotZ = Mathf.RoundToInt(transform.eulerAngles.z) % 90;
		diceVelocity = rb.velocity;
		if (!settled && timer>0f){
			timer -= Time.deltaTime;
		}
		if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f){
			
			if ((rotX == 0 && rotY == 0) || (rotZ == 0 && rotY == 0) || (rotX == 0 && rotZ == 0)){
				settled = true;
			}
		} 
		else{
			settled = false;
		}

		// if (Input.GetKeyDown (KeyCode.Space)) {
        //     RollDice();
		// }
	}
    void RollDice(){
		float dirX = Random.Range (0, 500);
		float dirY = Random.Range (0, 500);
		float dirZ = Random.Range (0, 500);
		transform.position = new Vector3 (0f, 14f, -8.25f);
		transform.rotation = Quaternion.identity;
		rb.AddForce (transform.up * 500);
		rb.AddTorque (dirX, dirY, dirZ);
    }
	public void killSelf(){
		Destroy(gameObject);
	}
}
