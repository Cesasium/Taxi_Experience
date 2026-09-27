using UnityEngine;

public class PrendiPasseggeri : MonoBehaviour
{

    public bool abbordo=false;
    private MoveForwardCar playerScript;
    public GameObject Destinazione;
    public Vector3 nuovaPos;
    public AudioClip SoundEntrata;
    public AudioSource passeggero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript=GameObject.Find("CarPlayer").GetComponent<MoveForwardCar>();
        passeggero=GetComponent<AudioSource>();
        Destinazione.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (playerScript.interazioneAction.triggered)
        {
            playerScript.PlayerSource.PlayOneShot(SoundEntrata);
            gameObject.SetActive(false);
            Debug.Log("Passegero a bordo");
            abbordo=true;
            Destinazione.SetActive(true);
        }
    }
}
