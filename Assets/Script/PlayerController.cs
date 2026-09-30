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
    int Completed=0;
    public InputAction moveAction;
    public Vector2 moveInput;
    public Vector3 pos;
    public InputAction interazioneAction;
    public InputAction apriMappaAction;
    public InputAction esciMappaAction;
    public TextMeshProUGUI testoTimer;
    public TextMeshProUGUI testoPowerUP;
    private PickUpClient pickUpClientScript;
    private PrendiPasseggeri prendiPasseggeriScript;
    private PrendiPasseggeri chiesaPasseggero;
    private PrendiPasseggeri libreriaScript;
    private Rigidbody playerRB;
    public AudioClip StartUp;
    public AudioClip boostPowerUp;
    public AudioClip timePowerUpAudio;
    public AudioSource PlayerSource;
    public GameObject PowerUPspeed;
    public GameObject HUD;
    public GameObject apriMappa;
    public AudioClip dropOFFSound;
    public ParticleSystem entranceParticle;
    public ParticleSystem boost;
    public TextMeshProUGUI testoVittoria;
    
    public bool HaPowerUp=false;
    public bool addTimePowerUp=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Utilizzare la classe prendipassegeriScript per gestire i passeggeri basta instanziare un oggetto con lo script PrendiPasseggeri e assegnarlo alla variabile prendiPasseggeriScript
        moveAction.Enable();
        interazioneAction.Enable();
        apriMappaAction.Enable();
        esciMappaAction.Enable();
        pickUpClientScript=GameObject.Find("Clienti").GetComponent<PickUpClient>();
        prendiPasseggeriScript=GameObject.Find("ScuolaToMunicipio").GetComponent<PrendiPasseggeri>();
        chiesaPasseggero=GameObject.Find("ChiesaToPark").GetComponent<PrendiPasseggeri>();
        libreriaScript=GameObject.Find("ParkToLibreria").GetComponent<PrendiPasseggeri>();
        playerRB=GetComponent<Rigidbody>();
        PlayerSource=GetComponent<AudioSource>();
        PlayerSource.PlayOneShot(StartUp);
        pickUpClientScript.TimePowerUp.SetActive(false);    //Allo start viene settato il PowerUP del tempo a false
        currentSpeed=baseSpeed;
        apriMappa.SetActive(false);
        boost.Stop();
        testoVittoria.gameObject.SetActive(false);
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

        if (apriMappaAction.triggered)
        {
            Debug.Log("Mappa aperta");
            HUD.SetActive(false);
            apriMappa.SetActive(true);
            moveAction.Disable();   //disabilità il movimento della macchina
            
            
        }

        if (esciMappaAction.triggered)
        {   
                Debug.Log("Mappa chiusa");
                HUD.SetActive(true);
                apriMappa.SetActive(false);
                moveAction.Enable();
        }

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
        

        if (Completed >= 4)
            {
                Destroy(testoTimer);
                testoVittoria.gameObject.SetActive(true);
                moveAction.Disable();
            }
            else if(Completed < 4 && time <= 0)
            {
                Destroy(testoTimer);
                testoVittoria.text="Tempo Scaduto, Hai Perso!";
                testoVittoria.gameObject.SetActive(true);
                moveAction.Disable();
            }

        
        if (HaPowerUp)
        {
            //Debug.Log("Tempo avviato"+HaPowerUp);
            if(timePowerUp> 0)
            {
                boost.Play();
                timePowerUp-=Time.deltaTime;
                AggiornaGraficaPowerUp();
            }
            else 
            {
                boost.Stop();
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
            boost.Play();
            PlayerSource.PlayOneShot(boostPowerUp);
            currentSpeed=baseSpeed+multiplierSpeed;
            Debug.Log("Colissione avvenuta con powerUp settato a"+HaPowerUp);
        }else
        {
            boost.Stop();
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
            PlayerSource.PlayOneShot(timePowerUpAudio);
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
            time+=15.0f;                 //Ad ogni fine corsa, viene aggiunto tempo extra
            Completed+=1;
            Debug.Log("Ci sei sopra!");
            
            
            
        }

        if (collision.gameObject.CompareTag("DestinazioneMunicipio"))
        {
            prendiPasseggeriScript.abbordo=false;
            prendiPasseggeriScript.Hud_descrizione.SetActive(false);
            prendiPasseggeriScript.gameObject.SetActive(true);
            prendiPasseggeriScript.nuovaPos=transform.position;
            prendiPasseggeriScript.gameObject.transform.position=prendiPasseggeriScript.nuovaPos;
            prendiPasseggeriScript.passeggero.PlayOneShot(dropOFFSound);
            prendiPasseggeriScript.Destinazione.SetActive(false);
            time+=15.0f;
            Completed+=1;
            Debug.Log("Arrivato");
            
        }

        //Gestisce il passeggero della libreria
        if (collision.gameObject.CompareTag("Libreria"))
        {
           libreriaScript.abbordo=false;
           libreriaScript.gameObject.SetActive(true);
           libreriaScript.Hud_descrizione.SetActive(false);
           libreriaScript.nuovaPos=transform.position;
           libreriaScript.gameObject.transform.position=libreriaScript.nuovaPos;
           libreriaScript.passeggero.PlayOneShot(dropOFFSound);
           libreriaScript.Destinazione.SetActive(false);
            time+=15.0f;
            Completed+=1;
            Debug.Log("Arrivato");
            
        }

        if (collision.gameObject.CompareTag("PowerUp"))
        {
            SpeedPowerUp();
        }


        if (collision.gameObject.CompareTag("DestinazioneParcheggio"))
        {
            chiesaPasseggero.abbordo=false;
            chiesaPasseggero.Hud_descrizione.SetActive(false);
            chiesaPasseggero.gameObject.SetActive(true);
            chiesaPasseggero.nuovaPos=transform.position;
            chiesaPasseggero.gameObject.transform.position=chiesaPasseggero.nuovaPos;
            chiesaPasseggero.passeggero.PlayOneShot(dropOFFSound);
            chiesaPasseggero.Destinazione.SetActive(false);
            time+=15.0f;
            Completed+=1;
            Debug.Log("Arrivato");
            
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

        
    }

}
