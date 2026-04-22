using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    public NavMeshAgent agent;
    private Vector3 destination;
    public bool IsDestination => Vector3.Distance(destination, Vector3.right * transform.position.x + Vector3.forward * transform.position.z) < 0.1f; // kiểm tra bot đã đến đích chưa

    //protected override void Start()
    //{
    //    base.Start();
    //    ChangeState(new PatrolState());
    //}


    public override void OnInit()
    {
        base.OnInit();
        ChangeAnim("Idle");
    }    

    // đặt điểm đến cho bot
    public void SetDestination(Vector3 pos)
    {
        agent.enabled = true;
        destination = pos;
        destination.y = 0;
        agent.SetDestination(pos);
    }

    IState<Bot> currentState;

    private void Update()
    {
        if (GameManager.Instance.IsState(GameState.Gameplay) &&  currentState != null)
        {
            currentState.OnExecute(this);
            //check stair
            CanMove(transform.position);
        }
    }

    public void ChangeState(IState<Bot> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = state;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    internal void StopMove()
    {
        agent.enabled = false;
    }

    public void StopMoving()
    {
        if (agent != null)
            agent.isStopped = true;
    }

    public void ResumeMoving()
    {
        if (agent != null)
            agent.isStopped = false;
    }

}
