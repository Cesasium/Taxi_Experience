using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class PickUpClient : MonoBehaviour
{
    private MoveForwardCar playerControllerScript; 
    public GameObject Destinazione;
    public bool IsInCar=false;
    public Vector3 newPos;
    public AudioClip SoundEntrata;
    public AudioSource passeggeroSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript=GameObject.Find("CarPlayer").GetComponent<MoveForwardCar>();
        passeggeroSource=GetComponent<AudioSource>();
        Destinazione.SetActive(false);
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
            playerControllerScript.PlayerSource.PlayOneShot(SoundEntrata);
            playerControllerScript.entranceParticle.Play();
            
            gameObject.SetActive(false);
            Debug.Log("Passeggero a bordo!");
            IsInCar=true;
            Destinazione.SetActive(true);
        }
        
    }

  
    
}
