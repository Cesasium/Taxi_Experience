using UnityEngine;
using UnityEngine.AI;

public class CarIaController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform movePositionTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination=movePositionTransform.position;
    }
}
