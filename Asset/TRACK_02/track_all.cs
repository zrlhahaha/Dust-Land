using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class track_all : MonoBehaviour {

	public float moveaxis;
	public float turnaxis;
	public float turn_steer_max;
	public float turn_steer_min;
	public float turn_steer;
	public float torque;
	public float break_torque;
	public float max_speed;

	public track[] tracks;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		Rigidbody upperrig = this.GetComponentInParent<Rigidbody> ();

		Debug.Log (upperrig.velocity.magnitude);

		if (upperrig.velocity.magnitude < max_speed) {
			turn_steer = ((turn_steer_min - turn_steer_max) / max_speed) * upperrig.velocity.magnitude + turn_steer_max;
		} else {
			turn_steer = turn_steer_min;
		}

		for (int x = 0; x < tracks.Length; x ++) {
			if (Vector3.Angle(-gameObject.transform.forward,upperrig.velocity.normalized) < 90f) {
				if (upperrig.velocity.magnitude < max_speed || moveaxis <= 0f) {
					tracks[x].banlancem = moveaxis;
				} else {
					tracks[x].banlancem = 0f;
				}
			} else {
				if (upperrig.velocity.magnitude < max_speed || moveaxis >= 0f) {
					tracks[x].banlancem = moveaxis;
				} else {
					tracks[x].banlancem = 0f;
				}
			}

			if (tracks[x].left) {
				tracks[x].banlancet = turnaxis * turn_steer;
			} else {
				tracks[x].banlancet = -turnaxis * turn_steer;
			}
			if (tracks[x].forward) {
				tracks[x].steer = turnaxis * turn_steer * 100f;
			} else {
				tracks[x].steer = - turnaxis * turn_steer * 100f;
			}
			tracks[x].motor = torque;
		}
	}
}
