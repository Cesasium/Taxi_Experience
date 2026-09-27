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
    private float time=100.0f;
    public float timePowerUp=5.0f;
    int sec;
    public InputAction moveAction;
    public Vector2 moveInput;
    public Vector3 pos;
    public InputAction interazioneAction;
    public TextMeshProUGUI testoTimer;
    public TextMeshProUGUI testoPowerUP;
    private PickUpClient pickUpClientScript;
    private PrendiPasseggeri prendiPasseggeriScript;
    private PrendiPasseggeri libreriaScript;
    private Rigidbody playerRB;
    public AudioClip StartUp;
    public AudioSource PlayerSource;
    public GameObject PowerUPspeed;
    public AudioClip dropOFFSound;
    public ParticleSystem entranceParticle;
    public bool HaPowerUp=false;
    public bool addTimePowerUp=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Utilizzare la classe prendipassegeriScript per gestire i passeggeri basta instanziare un oggetto con lo script PrendiPasseggeri e assegnarlo alla variabile prendiPasseggeriScript
        moveAction.Enable();
        interazioneAction.Enable();
        pickUpClientScript=GameObject.Find("Clienti").GetComponent<PickUpClient>();
        prendiPasseggeriScript=GameObject.Find("ScuolaToMunicipio").GetComponent<PrendiPasseggeri>();
        libreriaScript=GameObject.Find("ParkToLibreria").GetComponent<PrendiPasseggeri>();
        playerRB=GetComponent<Rigidbody>();
        PlayerSource=GetComponent<AudioSource>();
        PlayerSource.PlayOneShot(StartUp);
        pickUpClientScript.TimePowerUp.SetActive(false);    //Allo start viene settato il PowerUP del tempo a false
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

        
            if (time > 0)
            {
                time-=Time.deltaTime;
                AggiornaGraficaTimer();
            }
            else
            {
                time=0;
                Debug.Log("Tempo Scaduto");
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
                timePowerUp=5;          //Invece di imposarlo a 0 il timer cosi viene resettato in grado di poter essere ripreso di nuovo
                HaPowerUp=false;
                currentSpeed=baseSpeed;
                Debug.Log("Tempo Scaduto "+HaPowerUp);
            }
        }


        
    }

    void AggiornaGraficaTimer()
    {
        int min=Mathf.FloorToInt(time/60);
        sec=Mathf.FloorToInt(time % 60);
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
        //gestisce il passeggero degli uffici
        if (collision.gameObject.CompareTag("Destinazione"))
        {
            pickUpClientScript.IsInCar=false;
            pickUpClientScript.elementoHUD.SetActive(false);
            pickUpClientScript.boxInterazione.gameObject.SetActive(false);
            pickUpClientScript.gameObject.SetActive(true);
            pickUpClientScript.newPos=transform.position;
            pickUpClientScript.gameObject.transform.position=pickUpClientScript.newPos;
            pickUpClientScript.passeggeroSource.PlayOneShot(dropOFFSound);
            pickUpClientScript.Destinazione.SetActive(false); 
            time+=15.0f;                    //Ad ogni fine corsa, viene aggiunto tempo extra
            Debug.Log("Ci sei sopra!");
            
            StartCoroutine(MioRitardo());
            
        }

        if (collision.gameObject.CompareTag("DestinazioneMunicipio"))
        {
            prendiPasseggeriScript.abbordo=false;
            prendiPasseggeriScript.gameObject.SetActive(true);
            prendiPasseggeriScript.nuovaPos=transform.position;
            prendiPasseggeriScript.gameObject.transform.position=prendiPasseggeriScript.nuovaPos;
            prendiPasseggeriScript.Destinazione.SetActive(false);
            time+=15.0f;
            Destroy(prendiPasseggeriScript);
            Debug.Log("Arrivato");
            StartCoroutine(MioRitardo());
        }

        //Gestisce il passeggero della libreria
        if (collision.gameObject.CompareTag("Libreria"))
        {
           libreriaScript.abbordo=false;
           libreriaScript.gameObject.SetActive(true);
           libreriaScript.nuovaPos=transform.position;
           libreriaScript.gameObject.transform.position=libreriaScript.nuovaPos;
           libreriaScript.passeggero.PlayOneShot(dropOFFSound);
           libreriaScript.Destinazione.SetActive(false);
            time+=15.0f;
            Debug.Log("Arrivato");
            StartCoroutine(MioRitardo());
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            SpeedPowerUp();
        }
        
    }

    //Se l'oggetto entra in contatto con il powerUP viene disattivato, la varibile impostata a true e viene chiamata la funzione SpeedPowerUp
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

    IEnumerator MioRitardo()
    {
        Debug.Log("Inzio attesa....");
        yield return new WaitForSeconds(5f);
        Debug.Log("Son passati 5 secondi.");
        Destroy(pickUpClientScript.gameObject);
        if (prendiPasseggeriScript.abbordo == false && libreriaScript.abbordo ==false)
        {
            Destroy(prendiPasseggeriScript.gameObject);
            Destroy(libreriaScript.gameObject);
        }
        
    }

}
