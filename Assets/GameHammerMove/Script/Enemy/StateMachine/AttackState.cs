using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private float attackDuration = 1f; 
    private float attackTimer = 0f;

    public void OnEnter(Enemy enemy)
    {
        attackTimer = 0f;
        enemy.ChangeAnim(Constants.ANIM_ATTACK); 
        enemy.Attack(); 
    }

    public void OnExecute(Enemy enemy)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDuration)
        {
            enemy.ChangeState(enemy.ChaseState);
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
