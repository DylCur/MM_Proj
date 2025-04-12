using System.Collections;
using MathsAndSome;
using UnityEngine;

public class Menace : BaseEnemy
{
    Vector2 playerOffset;

    public override IEnumerator Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void EnemyStart()
    {
       
    }

    public override IEnumerator Hunt()
    {
        Debug.Log("Hunt");
        hunting = true;

        Vector3 pd = mas.PlayerDistance(player, gameObject);
        
        bool playerInRange = pd.x <= attackRange &&
        pd.z <= attackRange &&
        pd.y <= maxYRange;

        if(playerInRange){
            s = EState.attacking;
        }

        yield return new WaitForSeconds(0.1f);

        hunting = false;
    }

    public override IEnumerator Seek()
    {
        Debug.Log("Seek");
        seeking = true;

        Vector3 pd = mas.PlayerDistance(player, gameObject);
        
        bool playerInRange = pd.x <= seekRange &&
        pd.z <= seekRange &&
        pd.y <= maxYRange;

        if(playerInRange){
            s = EState.hunting;
        }

        yield return new WaitForSeconds(0.1f);

        seeking = false;
    }

}
