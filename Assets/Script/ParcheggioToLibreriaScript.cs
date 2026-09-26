using UnityEngine;

public class ParcheggioToLibreriaScript : MonoBehaviour
{
    public bool Abbordo=false;
    public GameObject Destinazione;
    private MoveForwardCar playerControllerScript;
    public Vector3 nuovaPosizione;
    public AudioClip SoundEntrata;
    public AudioSource passeggero3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript=GameObject.Find("CarPlayer").GetComponent<MoveForwardCar>();
        passeggero3=GetComponent<AudioSource>();
        Destinazione.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerStay(Collider other)
    {
        if (playerControllerScript.interazioneAction.triggered)
        {
            playerControllerScript.PlayerSource.PlayOneShot(SoundEntrata);
            gameObject.SetActive(false);
            Debug.Log("Passeggero a bordo!");
            Abbordo=true;
            Destinazione.SetActive(true);
        }
    }
}
