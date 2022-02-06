using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiBearController : MonoBehaviour {
    public QuestManager questManager;
    public Animator anim;
    private NavMeshAgent agent;
    private bool stop;
    private bool inside;

    private Transform target;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        stop = true;
        inside = true;
    }

    void Update()
    {
        if(!stop)
        {
            if (Vector3.Distance(transform.position, target.position) < 0.5)
            {
                anim.SetBool("Walking", false);
                stop = true;
                if(inside)
                {
                    questManager.activeQuests.Add(Utils.bearFeedQuestIndex);
                    questManager.activeQuests.Add(Utils.rockDestroyQuestIndex);
                }
            }
            else
            {
                anim.SetBool("Walking", true);
            }
        }
    }

    public void setTarget(Transform target, bool inside)
    {
        stop = false;
        this.inside = inside;
        this.target = target;
        agent.SetDestination(target.position);
    }

    public bool isInside()
    {
        if(stop == true && inside == true)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
