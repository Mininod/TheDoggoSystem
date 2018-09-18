using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacePlanet : MonoBehaviour {

    //DEbug
    public GameObject textGameobject;
    private Text debugText;
    public GameObject textGameobject2;
    private Text debugText2;

    public GameObject planet;

    public GameObject canvasMain;
    public GameObject velocityButtonGameObject;
    public GameObject tickButtonGameObject;
    private Button velocityButton;
    private Button tickButton;
  
    
    private Vector3 tapLocation;

    private GameObject UFO;
    private Rigidbody2D ufoRig;

    private float distance;
    private float SCALEMULTIPLYER = 125;

    public GameObject velocityArrowPreFab;
    private GameObject velocityArrow;
    private Vector2 velocityMeasure;


    protected bool editMode = false;
    protected bool velocityMode = false;

    // Use this for initialization
    void Start () {

        //debug
        debugText = textGameobject.GetComponent<Text>();
        debugText2 = textGameobject2.GetComponent<Text>();
        string thing = "0";
        debugText.text = thing;
        debugText2.text = thing;

        velocityButton = velocityButtonGameObject.GetComponent<Button>();
        tickButton = tickButtonGameObject.GetComponent<Button>();
        velocityButton.onClick.AddListener(VelocityButtonOnClick);
        tickButton.onClick.AddListener(TickButtonOnClick);
        velocityButtonGameObject.SetActive(false);
        tickButtonGameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount == 2 && velocityMode == false) 
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
                    tapLocation = Camera.main.ScreenToWorldPoint(new Vector3((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x) / 2, (Input.GetTouch(0).position.y + Input.GetTouch(1).position.y) / 2, 1));
                    UFO = Instantiate(planet, tapLocation, planet.transform.rotation);
                    ufoRig = UFO.GetComponent<Rigidbody2D>();
                    editMode = true;
                }
                  
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                
                distance = Mathf.Sqrt(((Input.GetTouch(0).position.x - Input.GetTouch(1).position.x) * (Input.GetTouch(0).position.x - Input.GetTouch(1).position.x)) + ((Input.GetTouch(0).position.y - Input.GetTouch(1).position.y) * (Input.GetTouch(0).position.y - Input.GetTouch(1).position.y))); //The distance between both fingers 
                //debugText.text = distance.ToString();
                UFO.GetComponent<Transform>().localScale = new Vector3(distance / SCALEMULTIPLYER, distance / SCALEMULTIPLYER, 1);
            }

            
           
           
        }

        ///*************************************************WHAT YOU ARE WORKING ON NOW
        if (Input.touchCount == 1 && velocityMode == true)
        {
            
            //Velocity mode On, start
            Vector2 inputWorldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            float angle = Mathf.Atan2(inputWorldPoint.y - UFO.transform.position.y, inputWorldPoint.x - UFO.transform.position.x) * 180 / Mathf.PI;
            velocityArrow.transform.eulerAngles = new Vector3(0, 0, angle+90);
            velocityMeasure = new Vector2(UFO.transform.position.x - inputWorldPoint.x, UFO.transform.position.y - inputWorldPoint.y);
            

            debugText.text = velocityMeasure.x.ToString();
            debugText2.text = velocityMeasure.y.ToString();

            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                
            }
           
        }


    }
    
    
    void VelocityButtonOnClick()
    {
        velocityButtonGameObject.SetActive(false);
        velocityMode = true;
        Vector3 location = Camera.main.WorldToScreenPoint(new Vector3(UFO.transform.position.x, UFO.transform.position.y, UFO.transform.position.z));
        velocityArrow = Instantiate(velocityArrowPreFab, location, velocityArrowPreFab.transform.rotation);
        velocityArrow.transform.SetParent(canvasMain.transform);

    }

    void TickButtonOnClick()
    {
        if (velocityMode == true) //If editing velocity
        {
            velocityMode = false;                               //Out of velocity
            velocityButtonGameObject.SetActive(true);           //Re-see the velocity
            Destroy(velocityArrow);
            
        }
        else
        {
            tickButtonGameObject.SetActive(false);          //
            velocityButtonGameObject.SetActive(false);      //  Out of Edit mode
            editMode = false;                               //
            UFO.GetComponent<CircleCollider2D>().enabled = true;
            ufoRig.velocity = new Vector2(velocityMeasure.x * -1, velocityMeasure.y * -1);
        }
        
    }



}
