using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollide : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Sun"))
        {
            gameObject.GetComponent<MassController>().enabled = false;
            gameObject.SetActive(false);
        }

        if(other.gameObject.CompareTag("Planet"))
        {
            float otherMass = other.gameObject.GetComponent<MassController>().m_Mass;
            float mass = GetComponent<MassController>().m_Mass;

            if (otherMass > mass)
            {
                gameObject.SetActive(false);
            }
            else if(GetComponent<MassController>().IsPlanetViable())
            {
                GetComponent<MassController>().HalfPlanet();
            }
            else
            {
                gameObject.GetComponent<MassController>().enabled = false;
                gameObject.SetActive(false);
            }
        }
    }
    
}
