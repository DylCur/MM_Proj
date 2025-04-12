using System.Collections;
using MathsAndSome;
using UnityEngine;
using UnityEngine.AI;

public class SwordsMan : BaseEnemy
{

    [HideInInspector] public NavMeshAgent agent;

    public override void EnemyStart()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override bool inHuntRange()
    {
        Vector3 pd = mas.PlayerDistance(player, gameObject);
        
        return pd.x <= seekRange &&
        pd.z <= seekRange &&
        pd.y <= maxYRange;
    }

    public override bool inAttackRange()
    {
        Vector3 pd = mas.PlayerDistance(player, gameObject);
        
        return pd.x <= attackRange &&
        pd.z <= attackRange &&
        pd.y <= maxYRange;
    }

    public override IEnumerator Seek()
    {
        Debug.Log("Seek");
        seeking = true;

        if(inHuntRange()){
            s = EState.hunting;
        }

        yield return new WaitForSeconds(0.1f);

        seeking = false;
    }
    
    public override IEnumerator Hunt()
    {
        Debug.Log("Hunt");
        hunting = true;

        agent.SetDestination(player.transform.position);

        if(inAttackRange()){
            s = EState.attacking;
        }

        yield return new WaitForSeconds(0.1f);

        hunting = false;
    }
    
    public override IEnumerator Attack()
    {
        Debug.Log("Attack");
        attacking = true;

        agent.SetDestination(transform.position);
        
        Vector3 pd = mas.PlayerDistance(player, gameObject);

        bool playerInRange = pd.x <= attackRange &&
        pd.z <= attackRange &&
        pd.y <= maxYRange;

        if(!playerInRange){
            s = EState.hunting;
        }

        yield return new WaitForSeconds(attackTime);

        attacking = false;
    }
}
