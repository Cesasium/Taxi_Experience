using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using Unity.VisualScripting;

public class PickUpClient : MonoBehaviour
{
    private MoveForwardCar playerControllerScript; 
    public GameObject Destinazione;
    public GameObject TimePowerUp;
    public bool IsInCar=false;
    public Vector3 newPos;
    public BoxCollider boxInterazione;
    public AudioClip SoundEntrata;
    public AudioSource passeggeroSource;
    private SpinScript spinScriptObject;
    public GameObject elementoHUD;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript=GameObject.Find("CarPlayer").GetComponent<MoveForwardCar>();
        spinScriptObject=GameObject.Find("Dollaro").GetComponent<SpinScript>();
        boxInterazione=GameObject.Find("Clienti").GetComponent<BoxCollider>();
        passeggeroSource=GetComponent<AudioSource>();
        Destinazione.SetActive(false);
        elementoHUD.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void OnTriggerStay(Collider other)
    {
        //Se il player preme E e si trova al'interno della zona avvia la corsa
        if (playerControllerScript.interazioneAction.triggered)
        {
            elementoHUD.SetActive(true);
            TimePowerUp.SetActive(true);    //Ho attaccato il componente powerUp cosi che una volta si sale in macchina viene attivato
            
            playerControllerScript.PlayerSource.PlayOneShot(SoundEntrata);
            playerControllerScript.entranceParticle.Play();
            spinScriptObject.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Debug.Log("Passeggero a bordo!");
            IsInCar=true;
            Destinazione.SetActive(true);
        }
        
    }

  
    
}
