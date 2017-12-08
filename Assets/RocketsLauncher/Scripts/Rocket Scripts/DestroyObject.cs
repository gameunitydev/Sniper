using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

    //variable used to determine how much time the script should wait until the object must be destroyed
    public float afterSeconds = 4;

    //variable used to store the start time
    private float _startTime;

	void Start () {
        //store start time
        _startTime = Time.time;
	}
	
	void FixedUpdate () {
        //compare the start time with how much time has passed and check if it's greater than "afterSeconds"
        if (Time.time - _startTime > afterSeconds)
            //destroy the game object that contains this script
            Destroy(gameObject);
	}
}
