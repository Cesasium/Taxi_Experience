using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class MoveForwardCar : MonoBehaviour
{

    public float baseSpeed = 10.0f;
    public float multiplierSpeed;
    public float currentSpeed;
    public float turnspeed=70.0f;
    public float time=60.0f;
    public float timePowerUp=5.0f;
    public InputAction moveAction;
    public Vector2 moveInput;
    public InputAction interazioneAction;
    public TextMeshProUGUI testoTimer;
    public TextMeshProUGUI testoPowerUP;
    private PickUpClient pickUpClientScript;
    private Rigidbody playerRB;
    public AudioClip StartUp;
    public AudioSource PlayerSource;
    public AudioClip dropOFFSound;
    public ParticleSystem entranceParticle;
    public bool HaPowerUp=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        interazioneAction.Enable();
        pickUpClientScript=GameObject.Find("Clienti").GetComponent<PickUpClient>();
        playerRB=GetComponent<Rigidbody>();
        PlayerSource=GetComponent<AudioSource>();
        PlayerSource.PlayOneShot(StartUp);

        currentSpeed=baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput=moveAction.ReadValue<Vector2>();
        if (moveAction.triggered)
        {
            PlayerSource.PlayOneShot(StartUp,0);
        }
        transform.Translate(Vector3.forward *currentSpeed*moveInput.y*Time.deltaTime);

        transform.Rotate(Vector3.up, Time.deltaTime* turnspeed*moveInput.x);

        if (pickUpClientScript.IsInCar)
        {
            if (time > 0)
            {
                time-=Time.deltaTime;
                AggiornaGraficaTimer();
            }
            else
            {
                time=0;
                pickUpClientScript.IsInCar=false;
                Debug.Log("Tempo Scaduto");
            }
        }
        
        if (HaPowerUp)
        {
            //Debug.Log("Tempo avviato"+HaPowerUp);
            if(timePowerUp> 0)
            {
                timePowerUp-=Time.deltaTime;
                AggiornaGraficaPowerUp();
            }
            else 
            {
                timePowerUp=0;
                HaPowerUp=false;
                currentSpeed=baseSpeed;
                //speed=10;
                Debug.Log("Tempo Scaduto "+HaPowerUp);
            }
        }

        
    }

    void AggiornaGraficaTimer()
    {
        int min=Mathf.FloorToInt(time/60);
        int sec=Mathf.FloorToInt(time % 60);
        
        testoTimer.text=string.Format("{0:00}:{1:00}",min,sec);
        
    }
    //TIMER PowerUP
    void AggiornaGraficaPowerUp()
    {
        int secPowerUp=Mathf.FloorToInt(timePowerUp % 60);
        testoPowerUP.text=string.Format("{0:00}",secPowerUp);
    }
    //Funzione PowerUp legge se il valore è uguale a TRUE, se lo è incrementa la velocità di 5 secondi
    void PowerUp()
    {
        if (HaPowerUp)
        {
            currentSpeed=baseSpeed+multiplierSpeed;
            Debug.Log("Colissione avvenuta con powerUp settato a"+HaPowerUp);
        }else
        {
            currentSpeed=baseSpeed;
            Debug.Log("Variabile impostata false,Velocità ristabilita");
        }

        
        
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destinazione"))
        {
            pickUpClientScript.IsInCar=false;
            
            Destroy(testoTimer);
            pickUpClientScript.gameObject.SetActive(true);
            pickUpClientScript.newPos=new Vector3(84.4457932f,0.140000001f,-44.9099121f);
            pickUpClientScript.gameObject.transform.position=pickUpClientScript.newPos;
            pickUpClientScript.passeggeroSource.PlayOneShot(dropOFFSound);
            pickUpClientScript.Destinazione.SetActive(false); 
            Debug.Log("Ci sei sopra!");
            
        
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            PowerUp();
        }
        
    }

    //Se l'oggetto entra in contatto con il powerUP viene distrutto, la varibile impostata a true e viene chiamata la funzione PowerUp
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            Debug.Log("Prima HaPowerup è "+HaPowerUp);
            HaPowerUp=true;
            PowerUp();
            Debug.Log("Dopo HaPowerup è "+HaPowerUp);
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

}
