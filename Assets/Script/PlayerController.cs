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
    int sec;
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
    public bool addTimePowerUp=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        interazioneAction.Enable();
        pickUpClientScript=GameObject.Find("Clienti").GetComponent<PickUpClient>();
        playerRB=GetComponent<Rigidbody>();
        PlayerSource=GetComponent<AudioSource>();
        PlayerSource.PlayOneShot(StartUp);
        pickUpClientScript.TimePowerUp.SetActive(false);    //Allo start viene settato il gameobject a false
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
        sec=Mathf.FloorToInt(time % 60);
        //TimePowerUp();
        testoTimer.text=string.Format("{0:00}:{1:00}",min,sec);
        
    }
    //TIMER PowerUP
    void AggiornaGraficaPowerUp()
    {
        int secPowerUp=Mathf.FloorToInt(timePowerUp % 60);
        testoPowerUP.text=string.Format("{0:00}",secPowerUp);
    }
    //Funzione PowerUp legge se il valore è uguale a TRUE, se lo è incrementa la velocità di 5 secondi
    public void SpeedPowerUp()
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
    //Funzione TimePowerUp legge se il valore è uguale a TRUE, se lo è incrementa il timer di 5 secondi
    public void TimePowerUp()
    {
        Debug.Log(addTimePowerUp+" Tempo Power UP");
        if (addTimePowerUp)
        {
            time+=5;
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
            SpeedPowerUp();
        }
        
    }

    //Se l'oggetto entra in contatto con il powerUP viene distrutto, la varibile impostata a true e viene chiamata la funzione PowerUp
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            Debug.Log("Prima HaPowerup è "+HaPowerUp);
            HaPowerUp=true;
            SpeedPowerUp();
            Debug.Log("Dopo HaPowerup è "+HaPowerUp);
            other.gameObject.SetActive(false);
        }
        //Se la macchina entra in contatto con il powerUP del tempo, viene impostata la variabile a true e viene chiamata la funzione TimePowerUp
        if (other.CompareTag("TimePowerUp"))
        {
            addTimePowerUp=true;
            TimePowerUp();
            other.gameObject.SetActive(false);
        }
        
    }

}
