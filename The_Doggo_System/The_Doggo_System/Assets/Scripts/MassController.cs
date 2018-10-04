using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassController : MonoBehaviour {

    public Sprite planetviable;
    public Sprite planetNotViable;

    public float startingMass;
    private GameObject centreOfGavity;
    public float m_Mass;
    public float m_ToalMass;
    private int m_scaleTime;
    private float m_sunDistanceMultiplier;

    // Use this for initialization
    void Start () {
        gameObject.transform.localScale = new Vector3(startingMass, startingMass, 1);

        if (GameObject.Find("Sun"))
        {
            centreOfGavity = GameObject.Find("Sun");
        }
        else if (GameObject.Find("Blackhole"))
        {
            centreOfGavity = GameObject.Find("Blackhole");
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (!GetComponent<PlanetPause>().isGamePaused)
        {
            if(centreOfGavity != null)
            {
                m_sunDistanceMultiplier = Vector2.Distance(gameObject.transform.position, centreOfGavity.transform.position) / 2;
            }
            
            if (m_scaleTime <= 0)
            {
                m_scaleTime = 10; // hard value

                m_Mass += 0.02f / m_sunDistanceMultiplier;
                m_ToalMass += 0.02f / m_sunDistanceMultiplier;

                gameObject.transform.localScale = new Vector3(m_Mass / 2, m_Mass / 2, 0);

            }
            else
            {
                m_scaleTime--;
            }

            //m_Mass = 0;
        }

        if(IsPlanetViable())
        {
            GetComponent<SpriteRenderer>().sprite = planetviable;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = planetNotViable;
        }
	}

    public bool IsPlanetViable()
    {
        if (m_Mass / 2 > startingMass)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float HalfPlanet()
    {
        if(!(m_Mass / 2 < startingMass))
        {
            m_Mass /= 2;
            gameObject.transform.localScale = new Vector3(m_Mass, m_Mass, 0);
            return m_Mass;
        }
        else
        {
            return 0;
        }
    }
}
