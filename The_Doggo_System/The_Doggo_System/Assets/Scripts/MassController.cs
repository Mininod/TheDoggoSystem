using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassController : MonoBehaviour {

    public float startingMass;
    public GameObject sun;
    private float m_Mass;
    private int m_scaleTime;
    private float m_sunDistanceMultiplier;

    // Use this for initialization
    void Start () {
        gameObject.transform.localScale = new Vector3(startingMass, startingMass, 1);
	}
	
	// Update is called once per frame
	void Update () {

        m_sunDistanceMultiplier = Vector2.Distance(gameObject.transform.position, sun.transform.position) / 2;

        if (m_scaleTime <= 0)
        {
            m_scaleTime = 10; // hard value

            m_Mass += 0.1f / m_sunDistanceMultiplier;

            gameObject.transform.localScale += new Vector3(m_Mass, m_Mass, 0);

        }
        else
        {
            m_scaleTime--;
        }

        m_Mass = 0;
	}
}
