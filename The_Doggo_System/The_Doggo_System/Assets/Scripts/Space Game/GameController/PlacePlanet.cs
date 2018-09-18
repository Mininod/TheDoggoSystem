using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacePlanet : MonoBehaviour {

   

    public GameObject canvasMain;
    public GameObject planet;
    public GameObject tickButtonGameObject;
    public GameObject velocityArrowPreFab;

    private Button tickButton;
  
    
    private Vector3 tapLocation;

    private GameObject UFO;
    private Rigidbody2D ufoRig;
    private GameObject ghostPlanet;

    private float distance;

    
    private GameObject velocityArrow;
    private Vector2 velocityMeasure;



    protected bool editMode = false;
    protected bool velocityMode = false;

    private float coolDownTimer = 0;
    private float MAXTIME = 1;

    // Use this for initialization
    void Start () {
        

        
        tickButton = tickButtonGameObject.GetComponent<Button>();
        
        tickButton.onClick.AddListener(TickButtonOnClick);
       
        tickButtonGameObject.SetActive(false);


	}
	
	// Update is called once per frame
	void Update () {
        coolDownTimer += 1 * Time.deltaTime;

        if (Input.touchCount == 1 && velocityMode == false && !tickButtonGameObject.GetComponent<BoxCollider2D>().OverlapPoint(Input.GetTouch(0).position) && coolDownTimer > MAXTIME) //used to have && UFO!=null
        {
            if (editMode == false)
            {
                tapLocation = Camera.main.ScreenToWorldPoint(new Vector3((Input.GetTouch(0).position.x), (Input.GetTouch(0).position.y), 1));
                UFO = Instantiate(planet, tapLocation, planet.transform.rotation);
                ufoRig = UFO.GetComponent<Rigidbody2D>();
                editMode = true;
                tickButtonGameObject.SetActive(true);
            }

            if (ghostPlanet != null)
            {
                Destroy(ghostPlanet);
            }

            velocityMode = true;
            Vector3 location = Camera.main.WorldToScreenPoint(new Vector3(UFO.transform.position.x, UFO.transform.position.y, UFO.transform.position.z));
            velocityArrow = Instantiate(velocityArrowPreFab, location, velocityArrowPreFab.transform.rotation);
            velocityArrow.transform.SetParent(canvasMain.transform);
        }
        
        ///*************************************************WHAT YOU ARE WORKING ON NOW

        if (Input.touchCount == 1 && velocityMode == true && UFO != null)
        {    
            //Velocity mode On, start
            Vector2 inputWorldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            float angle = Mathf.Atan2(inputWorldPoint.y - UFO.transform.position.y, inputWorldPoint.x - UFO.transform.position.x) * 180 / Mathf.PI; 
            velocityArrow.transform.eulerAngles = new Vector3(0, 0, angle+90);
            velocityMeasure = new Vector2(UFO.transform.position.x - inputWorldPoint.x, UFO.transform.position.y - inputWorldPoint.y);
            

            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                velocityMode = false;                               //Out of velocity
                Destroy(velocityArrow);
                ghostPlanet = Instantiate(planet, UFO.transform.position, planet.transform.rotation);
                ghostPlanet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                
                ghostPlanet.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityMeasure.x, velocityMeasure.y);
                ghostPlanet.GetComponent<CircleCollider2D>().isTrigger = true;
                ghostPlanet.GetComponent<CircleCollider2D>().enabled = true;
            }
           
        }


    }
    
    

    void TickButtonOnClick()
    {
        if (ghostPlanet != null)
        {
            Destroy(ghostPlanet);
        }
        velocityMode = false;
        tickButtonGameObject.SetActive(false);          
        editMode = false;                               
        UFO.GetComponent<CircleCollider2D>().enabled = true;
        ufoRig.velocity = new Vector2(velocityMeasure.x, velocityMeasure.y);
        UFO = null;
        coolDownTimer = 0;
    }



}
