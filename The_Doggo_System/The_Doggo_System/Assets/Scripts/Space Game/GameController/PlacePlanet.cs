using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacePlanet : MonoBehaviour {

    //DEbug
    public GameObject textGameobject;
    private Text debugText;

    public GameObject cam;
    
    public GameObject planet;

    public GameObject velocityButtonGameObject;
    public GameObject tickButtonGameObject;
    private Button velocityButton;
    private Button tickButton;
  
    
    private Vector3 tapLocation;

    private GameObject UFO;
    private float distance;
    private float SCALEMULTIPLYER = 500;

    protected bool editMode = false;
    protected bool velocityMode = false;

    // Use this for initialization
    void Start () {

        //debug
        debugText = textGameobject.GetComponent<Text>();
        string thing = "0";
        debugText.text = thing;

        velocityButton = velocityButtonGameObject.GetComponent<Button>();
        tickButton = tickButtonGameObject.GetComponent<Button>();
        velocityButton.onClick.AddListener(VelocityButtonOnClick);
        tickButton.onClick.AddListener(TickButtonOnClick);
        velocityButtonGameObject.SetActive(false);
        tickButtonGameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount == 2) 
        {
            //PAUSE
            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                tickButtonGameObject.SetActive(true);
                velocityButtonGameObject.SetActive(true);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                if (editMode == false)
                {
                    tapLocation = cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x) / 2, (Input.GetTouch(0).position.y + Input.GetTouch(1).position.y) / 2, 1));
                    UFO = Instantiate(planet, tapLocation, planet.transform.rotation);
                    editMode = true;
                }
                  
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                
                distance = Mathf.Sqrt(((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x) * (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y)) + ((Input.GetTouch(0).position.y - Input.GetTouch(1).position.y) * (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y))); //The distance between both fingers 
                debugText.text = distance.ToString();
                UFO.GetComponent<Transform>().localScale = new Vector3(distance / SCALEMULTIPLYER, distance / SCALEMULTIPLYER, 1);
            }

            
           
           
        }

        if (Input.touchCount == 1 && velocityMode == true)
        {

        }

    }
    
    
    void VelocityButtonOnClick()
    {
        velocityButtonGameObject.SetActive(false);
        velocityMode = true;
        

    }

    void TickButtonOnClick()
    {
        if (velocityMode == true)
        {
            velocityMode = false;
            velocityButtonGameObject.SetActive(true);
        }
        else
        {
            tickButtonGameObject.SetActive(false);
            velocityButtonGameObject.SetActive(false);
            editMode = false;
        }
        
    }



}
