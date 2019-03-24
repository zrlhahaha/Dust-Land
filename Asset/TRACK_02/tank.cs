using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tank : MonoBehaviour {
	public Transform com;
	public GameObject track_all;
	private track_all track_allsys;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Rigidbody> ().centerOfMass = com.localPosition;
		track_allsys = track_all.GetComponent<track_all> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		float moveaxis = Input.GetAxis ("Vertical");
		float turnaxis = Input.GetAxis ("Horizontal");
		
		track_allsys.moveaxis = moveaxis;
		track_allsys.turnaxis = turnaxis;
	}
}
