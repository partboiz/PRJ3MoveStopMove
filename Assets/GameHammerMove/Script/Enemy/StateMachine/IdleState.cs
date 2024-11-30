using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float idleTime = 5f;
    private float timer = 0f;

    public void OnEnter(Enemy enemy)
    {
        timer = 0f;
        enemy.StartCoroutine(IdleCoroutine(enemy));
    }

    public void OnExecute(Enemy enemy)
    {
    }

    public void OnExit(Enemy enemy)
    {
        
    }

    private IEnumerator IdleCoroutine(Enemy enemy)
    {
        while (timer < idleTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        enemy.ChangeState(enemy.PatrolState);
    }
}
