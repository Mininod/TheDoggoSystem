using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlacePlanet : MonoBehaviour {


    public List<GameObject> allThePlanets = new List<GameObject>();
    private int planetsInactive = 0;

    public GameObject scoreTextGameObject;
    public GameObject planetTextGameObject;
    public GameObject resetTextGameObject;

    public GameObject canvasMain;
    public GameObject planet;
    public GameObject velocityArrowPreFab;

    public GameObject tickButtonGameObject;
    private Button tickButton;
    public GameObject pauseButtonGameObject;
    private Button pauseButton;
    public Sprite pauseButtonSprite;
    public Sprite playButtonSprite;

    public float score;
    private bool isThisPlayerALoser = false;
    
    private Vector3 tapLocation;

    private GameObject newPlanet;
    private GameObject ghostPlanet;
    public GameObject selectedPlanet;
    public GameObject sun;
    public GameObject blackhole;

    private float distance;

    
    private GameObject velocityArrow;
    private Vector2 velocityMeasure;



    protected bool editMode = false;
    protected bool velocityMode = false;
    public bool isPaused = false;

    private float coolDownTimer = 0;
    private float MAXTIME = 0.5f;


    //test
    public GameObject testplanet;

    // Use this for initialization
    void Start () {
       
        tickButton = tickButtonGameObject.GetComponent<Button>();
        tickButton.onClick.AddListener(TickButtonOnClick);
        tickButtonGameObject.SetActive(false);

        pauseButton = pauseButtonGameObject.GetComponent<Button>();
        pauseButton.onClick.AddListener(PauseButtonOnClick);

        allThePlanets.Add(testplanet);

	}
	
	// Update is called once per frame
	void Update () {
        coolDownTimer += 1 * Time.deltaTime;

        if(sun.GetComponent<SunController>().imploded == true)
        {
            //Choose random New mode
            blackhole.SetActive(true);
        }

        for (int i = 0; i < allThePlanets.Count; i++)
        {
            if(allThePlanets[i].activeInHierarchy == false)
            {
                planetsInactive += 1;
            }
        }

        if(allThePlanets.Count == planetsInactive)
        {
            Debug.Log("All the planets: " + allThePlanets.Count);
            Debug.Log("Planets inactive");
            Lose();
        }
        else
        {
            planetsInactive = 0;
        }


            //If you are using 1 finger while your not in velocity mode, and while the game is paused, and not touching any of the buttons and the cooldown timer is not done yet


        if (Input.touchCount == 1 && velocityMode == false && isPaused == true && !tickButtonGameObject.GetComponent<BoxCollider2D>().OverlapPoint(Input.GetTouch(0).position) && !pauseButtonGameObject.GetComponent<BoxCollider2D>().OverlapPoint(Input.GetTouch(0).position) && coolDownTimer > MAXTIME) 
        {
            for (int i = 0; i < allThePlanets.Count; i++)
            {
               if(allThePlanets[i].activeInHierarchy)
                {
                    if (allThePlanets[i].GetComponent<CircleCollider2D>().radius > Vector2.Distance(allThePlanets[i].gameObject.transform.position, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)) && allThePlanets[i].GetComponent<MassController>().IsPlanetViable())
                    {
                        Debug.Log(allThePlanets[i].gameObject.name);
                        selectedPlanet = allThePlanets[i];
                        coolDownTimer = 0;
                        break;
                    }
                }
 
            }

            if (editMode == false && selectedPlanet != null && coolDownTimer > MAXTIME)
            {
                tapLocation = Camera.main.ScreenToWorldPoint(new Vector3((Input.GetTouch(0).position.x), (Input.GetTouch(0).position.y), 1));
                newPlanet = Instantiate(planet, tapLocation, planet.transform.rotation);
                newPlanet.GetComponent<MassController>().m_Mass = selectedPlanet.GetComponent<MassController>().HalfPlanet();
                allThePlanets.Add(newPlanet);
                editMode = true;
                tickButtonGameObject.SetActive(true);
                pauseButtonGameObject.SetActive(false);
            }

            if (ghostPlanet != null)
            {
                Destroy(ghostPlanet);
            }
            if(editMode == true)
            {
                velocityMode = true;
                Vector3 location = Camera.main.WorldToScreenPoint(new Vector3(newPlanet.transform.position.x, newPlanet.transform.position.y, newPlanet.transform.position.z));
                velocityArrow = Instantiate(velocityArrowPreFab, location, velocityArrowPreFab.transform.rotation);
                velocityArrow.transform.SetParent(canvasMain.transform);
            }
        }
        
        ///*************************************************WHAT YOU ARE WORKING ON NOW

        if (Input.touchCount == 1 && velocityMode == true && newPlanet != null)
        {    
            //Velocity mode On, start
            Vector2 inputWorldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            float angle = Mathf.Atan2(inputWorldPoint.y - newPlanet.transform.position.y, inputWorldPoint.x - newPlanet.transform.position.x) * 180 / Mathf.PI; 
            velocityArrow.transform.eulerAngles = new Vector3(0, 0, angle+90);
            velocityMeasure = new Vector2(newPlanet.transform.position.x - inputWorldPoint.x, newPlanet.transform.position.y - inputWorldPoint.y);
            

            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                velocityMode = false;                               //Out of velocity
                Destroy(velocityArrow);
                ghostPlanet = Instantiate(planet, newPlanet.transform.position, planet.transform.rotation);
                ghostPlanet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                
                ghostPlanet.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityMeasure.x, velocityMeasure.y);
                ghostPlanet.GetComponent<CircleCollider2D>().isTrigger = true;
                ghostPlanet.GetComponent<CircleCollider2D>().enabled = true;
                ghostPlanet.GetComponent<PlanetPause>().isGamePaused = false;
            }
           
        }

       

        if(Input.touchCount == 2 && isThisPlayerALoser == true)
        {
            SceneManager.LoadScene("MainScene");
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
        newPlanet.GetComponent<CircleCollider2D>().enabled = true;
        newPlanet.GetComponent<MassController>().enabled = true;
        newPlanet.GetComponent<PlanetPause>().SetVelocity(new Vector3(velocityMeasure.x, velocityMeasure.y, 0));
        newPlanet = null;
        selectedPlanet = null;
        coolDownTimer = 0;
    }

    void PauseButtonOnClick()
    {
        isPaused = !isPaused;
        sun.GetComponent<SunController>().enabled = !sun.GetComponent<SunController>().enabled;
        if(isPaused)
        {
            pauseButton.image.overrideSprite = pauseButtonSprite;
        }
        else
        {
            pauseButton.image.overrideSprite = playButtonSprite;
        }

        for(int i = 0; i < allThePlanets.Count; i++)
        {
            if (allThePlanets[i].activeInHierarchy)
            {
                allThePlanets[i].GetComponent<PlanetPause>().isGamePaused = !allThePlanets[i].GetComponent<PlanetPause>().isGamePaused;
            }
        }
    }

    void Lose()
    {
        for(int i = 0; i < allThePlanets.Count; i++)
        {
            score += Mathf.Round(allThePlanets[i].GetComponent<MassController>().m_ToalMass);
        }
        Debug.Log("lose");
        Debug.Log("Score: " + score);
        Debug.Log("Total Planets Created: " + allThePlanets.Count);
        planetTextGameObject.GetComponent<Text>().text = "Total Planets: " + allThePlanets.Count;
        scoreTextGameObject.GetComponent<Text>().text = "Score: " + score;
        scoreTextGameObject.SetActive(true);
        planetTextGameObject.SetActive(true);
        resetTextGameObject.SetActive(true);
        isThisPlayerALoser = true;
        score = 0;
    }


}
