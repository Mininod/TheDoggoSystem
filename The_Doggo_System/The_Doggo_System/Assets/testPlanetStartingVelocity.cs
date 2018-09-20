using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlanetStartingVelocity : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().velocity = GetComponent<PlanetPause>().m_currentVelocity;
    }
	
	// Update is called once per frame
	void Update () {
       
    }
}
