using System.Collections;
using System.Collections.Generic;
using MathsAndSome;
using UnityEngine;


// The menace flies away :(

[RequireComponent(typeof(Rigidbody))]
public class Archer : BaseEnemy
{
    Vector2 playerOffset;

    bool moving;

    Rigidbody rb;
    [SerializeField] float offset = 10f;
    [SerializeField] float[] speed = new float[2];

    [HideInInspector] public List<GameObject> arrows;

    public GameObject arrow;


    void ShootArrow(){
        if(arrows.Count < 5){
            GameObject ar = Instantiate(arrow, transform.position, Quaternion.identity);
            // ar.GetComponent<Arrow>().parent = GetComponent<CapsuleCollider>();
            // arrows.Add(ar);
        }
    }

    public override IEnumerator Attack()
    {
        attacking = true;

        ShootArrow();
        

        yield return new WaitForSeconds(attackTime);

        attacking = false;
    }

    public override void EnemyStart()
    {
       rb = GetComponent<Rigidbody>();
       arrows = new List<GameObject>();
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


}
