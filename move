using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W)){
			transform.position += Time.deltaTime * Vector3.forward;// Move forward
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.position += Time.deltaTime * Vector3.back;
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.position += Time.deltaTime * Vector3.right;
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.position += Time.deltaTime * Vector3.left;
		}
	}
}
