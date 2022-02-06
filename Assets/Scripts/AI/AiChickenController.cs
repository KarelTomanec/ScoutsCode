using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChickenController : MonoBehaviour
{
    public Animator anim;
    private NavMeshAgent agent;
    private bool stop;
    private Vector3 radius;

    public Transform target;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        stop = true;
    }

    void Update()
    {
        if(agent.velocity == Vector3.zero)
        {
            anim.SetBool("Walking", false);
        }  else
        {
            anim.SetBool("Walking", true);
        }
        if (!stop)
        {
            agent.SetDestination(target.position);
        }
    }

    public void DisableMovement()
    {
        agent.isStopped = true;
        stop = true;
    }

    public void EnableMovement()
    {
        agent.isStopped = false;
        stop = false;
    }
}
