using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiFoxController : MonoBehaviour {
    public Animator anim;
    public float moveRadius = 10f;
    public float wanderTime = 5f;
    public ParticleSystem blood;
    public GameObject trap;

    private float time;
    private NavMeshAgent agent;
    private Vector3 destination;

    private bool stop;
    
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        time = wanderTime;
        stop = false;
        destination = RandomDestination(transform.position, moveRadius);
    }
    
    void Update()
    {
        if (!stop)
        {
            time += Time.deltaTime;
            if (wanderTime <= time)
            {
                destination = RandomDestination(transform.position, moveRadius);
                agent.SetDestination(destination);
                time = 0;
            }
        }
        if (Vector3.Distance(destination, transform.position) <= 2f)
        {
            anim.SetBool("Walking", false);
        }
        else
        {
            anim.SetBool("Walking", true);
        }
    }

    public void DisableMovement()
    {
        agent.isStopped = true;
        stop = true;
        anim.SetBool("Hurt", true);
        trap.SetActive(true);
    }
    
    public void EnableMovement()
    {
        agent.isStopped = false;
        stop = false;
        anim.SetBool("Hurt", false);
        trap.SetActive(false);
    }

    public static Vector3 RandomDestination(Vector3 origin, float distance)
    {
        NavMeshHit hit;
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMesh.SamplePosition(randomDirection, out hit, distance, -1);
        return hit.position;
    }

    public void Die()
    {
        anim.SetBool("Die", true);
        blood.Play();
    }
}
