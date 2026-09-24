using UnityEngine;

public class SpinScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject gameObjectRotation;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObjectRotation.transform.Rotate(Vector3.up *Time.deltaTime*speed);
    }
}
