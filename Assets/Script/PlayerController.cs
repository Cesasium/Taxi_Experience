using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MoveForwardCar : MonoBehaviour
{

    public float speed = 10.0f;
    public float turnspeed=70.0f;
    public float time=60.0f;
    public InputAction moveAction;
    public Vector2 moveInput;
    public InputAction interazioneAction;
    public TextMeshProUGUI testoTimer;
    private PickUpClient pickUpClientScript;
    public AudioClip StartUp;
    public AudioSource PlayerSource;
    public AudioClip dropOFFSound;
    public ParticleSystem entranceParticle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        interazioneAction.Enable();
        pickUpClientScript=GameObject.Find("Clienti").GetComponent<PickUpClient>();
        PlayerSource=GetComponent<AudioSource>();
        PlayerSource.PlayOneShot(StartUp);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput=moveAction.ReadValue<Vector2>();
        if (moveAction.triggered)
        {
            PlayerSource.PlayOneShot(StartUp,0);
        }
        transform.Translate(Vector3.forward *speed*moveInput.y*Time.deltaTime);

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
        
    }

    void AggiornaGraficaTimer()
    {
        int min=Mathf.FloorToInt(time/60);
        int sec=Mathf.FloorToInt(time % 60);
        testoTimer.text=string.Format("{0:00}:{1:00}",min,sec);
    }

    void OnCollisionEnter(Collision collision)
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
    }

}
