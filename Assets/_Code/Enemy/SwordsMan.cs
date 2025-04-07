using System.Collections;
using UnityEngine;

public class SwordsMan : BaseEnemy
{
    
    protected override void SeekPlayer(){
        if(shouldRedirect){
            agent.SetDestination(player.transform.position);
            StartCoroutine(redirectDelay());
        }
    }

    protected override void AttackPlayer()
    {
        seeking = false;
        
        if(canAttack){
            agent.SetDestination(gameObject.transform.position);
            Debug.Log($"{gameObject.name} is attacking");
        }

        StartCoroutine(AttackCD());
    }
}
