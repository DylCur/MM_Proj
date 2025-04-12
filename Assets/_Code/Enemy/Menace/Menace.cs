using System.Collections;
using System.Collections.Generic;
using MathsAndSome;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Menace : BaseEnemy
{
    Vector2 playerOffset;

    bool moving;

    Rigidbody rb;
    [SerializeField] float offset = 10f;

    List<GameObject> fireballs;

    public GameObject fireball;

    IEnumerator MoveMenace(){
        moving = true;
        
        // This should move it a bit
        rb.linearVelocity = new Vector3(Random.Range(-offset/2, offset/2), 0, Random.Range(-offset/2, offset/2));
        
        yield return new WaitForSeconds(0.3f);

        moving = false;
    }

    void ShootFireball(){
        if(fireballs.Count < 5){
            GameObject fb = Instantiate(fireball, transform.position, Quaternion.identity);
            fb.GetComponent<FireballController>().parent = GetComponent<CapsuleCollider>();
            fireballs.Add(fb);
        }
    }

    public override IEnumerator Attack()
    {
        attacking = true;

        int randomNumber = Mathf.FloorToInt(Random.Range(0,9.999f));

        if(randomNumber == 0 && !moving){
            StartCoroutine(MoveMenace());
        }
        else{
            ShootFireball();
        }

        yield return new WaitForSeconds(attackTime);

        attacking = false;
    }

    public override void EnemyStart()
    {
       rb = GetComponent<Rigidbody>();
       fireballs = new List<GameObject>();
    }

    public override bool inHuntRange(){
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

    public override IEnumerator Hunt()
    {
        Debug.Log("Hunt");
        hunting = true;

        if(inAttackRange()){
            s = EState.attacking;
        }

        yield return new WaitForSeconds(0.1f);

        hunting = false;
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

}
