using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunController : MonoBehaviour {
    public float timer = 0;
    public float scaleMulitplyer = 0.001f;
    public float magnitudeMultiplyer = 2;
    public float maxSizeForSun = 3;
    private bool unstable = false;
    private bool expandEnded = false;
    public bool imploded = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += 1 * Time.deltaTime;
        if (!SunStable())
        {
            SunExplode();
        }
     
        

    }

    void SunExplode()
    {
        if(!Expand())
        {
            Implode();
        }
    }

    bool SunStable()
    {
        if (gameObject.transform.localScale.x >= 3 || unstable == true)
        {
            unstable = true;
            return false;
        }
        else
        {
            SizeSun();
            if (timer >= 10)
            {
                GetComponent<PointEffector2D>().forceMagnitude -= 2;
                timer = 5;
            }
            return true;
        }

    }

    bool Expand()
    {
        if (gameObject.transform.localScale.x >= maxSizeForSun && gameObject.transform.localScale.x <= maxSizeForSun * 2 && expandEnded == false)
        {
            scaleMulitplyer += 0.001f;
            GetComponent<PointEffector2D>().forceMagnitude += 0.1f;
            SizeSun();
            return true;
        }
        else
        {
            expandEnded = true;
            return false;
        }
    }

    bool Implode()
    {
        if (gameObject.transform.localScale.x >= 0)
        {
            scaleMulitplyer -= 0.001f;
            GetComponent<PointEffector2D>().forceMagnitude += 0.05f;
            SizeSun();
            return true;
        }
        else 
        {
            imploded = true;
            gameObject.SetActive(false);
        }
        return false;
    }

    void SizeSun()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + scaleMulitplyer, gameObject.transform.localScale.y + scaleMulitplyer, gameObject.transform.localScale.z);
    }

}


//Once Gameobject scale is vector3(3.0,3.0,1.0) Then we change to the blackhole game mode