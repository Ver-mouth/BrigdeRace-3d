using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Bot>
{
    private float checkDelay = 0.1f;
    private float checkTimer = 0f;
    private LayerMask Stair;
    public void OnEnter(Bot t)
    {
        
        t.ChangeAnim("Run");
        t.SetDestination(LevelManager.Instance.FinishPoint);
    }

    public void OnExecute(Bot t)
    {
        if (t.BrickCount == 0)
        {
            Vector3 backPos = t.transform.position - t.transform.forward * 1.5f;
            t.SetDestination(backPos);

            // sau 0.3 giây đổi sang PatrolState
            t.StartCoroutine(DelayChangeState(t, 0.3f));
        }
    }

    private IEnumerator DelayChangeState(Bot t, float delay)
    {
        yield return new WaitForSeconds(delay);
        t.stage = LevelManager.Instance.GetCurrentStageForBot(t);
        t.ChangeState(new PatrolState());
    }

    public void OnExit(Bot t)
    {
    }

   
}
