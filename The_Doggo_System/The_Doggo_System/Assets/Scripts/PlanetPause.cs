using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPause : MonoBehaviour {

    public bool isGamePaused;

    private bool m_isObjCurrentlyPaused;

    private Vector3 m_currentVelocity;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isGamePaused)
        {
            if(!m_isObjCurrentlyPaused)
            {
                m_currentVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                m_isObjCurrentlyPaused = true;
            }
        }
        else if(m_isObjCurrentlyPaused && !isGamePaused)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = m_currentVelocity;
            m_isObjCurrentlyPaused = false;
        }
	}
}
