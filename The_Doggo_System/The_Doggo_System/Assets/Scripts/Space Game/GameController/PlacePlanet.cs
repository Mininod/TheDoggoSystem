using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlacePlanet : MonoBehaviour {


    public List<GameObject> allThePlanets = new List<GameObject>();

    public GameObject canvasMain;
    public GameObject planet;
    public GameObject velocityArrowPreFab;

    public GameObject tickButtonGameObject;
    private Button tickButton;
    public GameObject pauseButtonGameObject;
    private Button pauseButton;
  
    
    private Vector3 tapLocation;

    private GameObject UFO;
    private Rigidbody2D ufoRig;
    private GameObject ghostPlanet;

    private float distance;

    
    private GameObject velocityArrow;
    private Vector2 velocityMeasure;



    protected bool editMode = false;
    protected bool velocityMode = false;
    public bool isPaused = false;

    private float coolDownTimer = 0;
    private float MAXTIME = 1;

    // Use this for initialization
    void Start () {
       
        tickButton = tickButtonGameObject.GetComponent<Button>();
        tickButton.onClick.AddListener(TickButtonOnClick);
        tickButtonGameObject.SetActive(false);

        pauseButton = pauseButtonGameObject.GetComponent<Button>();
        pauseButton.onClick.AddListener(PauseButtonOnClick);

	}
	
	// Update is called once per frame
	void Update () {
        coolDownTimer += 1 * Time.deltaTime;

        if (Input.touchCount == 1 && velocityMode == false && isPaused == true && !tickButtonGameObject.GetComponent<BoxCollider2D>().OverlapPoint(Input.GetTouch(0).position) && !pauseButtonGameObject.GetComponent<BoxCollider2D>().OverlapPoint(Input.GetTouch(0).position) && coolDownTimer > MAXTIME) 
        {
            if (editMode == false)
            {
                tapLocation = Camera.main.ScreenToWorldPoint(new Vector3((Input.GetTouch(0).position.x), (Input.GetTouch(0).position.y), 1));
                UFO = Instantiate(planet, tapLocation, planet.transform.rotation);
                ufoRig = UFO.GetComponent<Rigidbody2D>();
                allThePlanets.Add(UFO); 
                editMode = true;
                tickButtonGameObject.SetActive(true);
                pauseButtonGameObject.SetActive(false);
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
                ghostPlanet.GetComponent<PlanetPause>().isGamePaused = false;
            }
           
        }


    }
    
    

    void TickButtonOnClick()
    {
       
        if (ghostPlanet != null)
        {
            Destroy(ghostPlanet);
        }
        //UFO.GetComponent<PlanetPause>().isGamePaused = false;
       // isPaused = false;
        velocityMode = false;
        tickButtonGameObject.SetActive(false);
        pauseButtonGameObject.SetActive(true);
        editMode = false;                               
        UFO.GetComponent<CircleCollider2D>().enabled = true;
        UFO.GetComponent<MassController>().enabled = true;
        UFO.GetComponent<PlanetPause>().SetVelocity(new Vector3(velocityMeasure.x, velocityMeasure.y, 0));
        UFO = null;
        coolDownTimer = 0;
    }

    void PauseButtonOnClick()
    {
        isPaused = !isPaused;
        for(int i = 0; i < allThePlanets.Count; i++)
        {
            allThePlanets[i].GetComponent<PlanetPause>().isGamePaused = !allThePlanets[i].GetComponent<PlanetPause>().isGamePaused;
        }
    }



}
