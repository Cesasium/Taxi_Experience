using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public GameObject wings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wings.transform.Rotate(Vector3.forward *Time.deltaTime * 250);
    }
}
