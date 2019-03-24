using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class track : MonoBehaviour {
	public bool left;
	public bool forward;
	public float steer;
	public float motor;
	public GameObject[] visiblewheels;
	public WheelCollider[] wheelcolliders;
	public GameObject[] bones;
	public Material trackmt;
	private Material trackmt_clone;
	public GameObject track_model;

	public float banlancem;
	public float banlancet;
	private float banlance;
	private Vector2 track_offset;
	private Rigidbody upperrig;
	private float speed;

	private float origin_bone_offset;
	// Use this for initialization
	void Start () {
		trackmt_clone = new Material(trackmt);
		track_model.GetComponent<Renderer> ().material = trackmt_clone;
		origin_bone_offset = Vector3.Distance(bones [0].transform.position,wheelcolliders [0].transform.position);
		upperrig = gameObject.GetComponentInParent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		banlance = banlancem + banlancet;

		for (int x = 0; x < wheelcolliders.Length; x ++) {
			Quaternion rot;
			Vector3 pos;
			wheelcolliders [x].motorTorque = -motor * banlance;
			wheelcolliders [x].steerAngle = steer;
			wheelcolliders [x].GetWorldPose (out pos,out rot);
			visiblewheels [x].transform.rotation = rot;
			visiblewheels [x].transform.position = pos;
			if (left) {
				bones[x].transform.position = visiblewheels [x].transform.position - visiblewheels [x].transform.right * origin_bone_offset;
			} else {
				bones[x].transform.position = visiblewheels [x].transform.position + visiblewheels [x].transform.right * origin_bone_offset;
			}


		}
		speed = upperrig.velocity.magnitude;
		if (speed < 0.01f) {
			track_offset = Vector2.zero;
		}
		if (Vector3.Angle(upperrig.velocity.normalized,gameObject.transform.up) < 90f) {
			track_offset += new Vector2 (0, speed * 0.001f);
		} else {
			track_offset += new Vector2 (0, -speed * 0.001f);
		}
		track_model.GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex",track_offset);
	}

}
