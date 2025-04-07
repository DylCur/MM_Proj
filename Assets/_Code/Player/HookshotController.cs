using System;
using System.Collections;
using UnityEngine;
using MathsAndSome;

public class HookshotController : MonoBehaviour
{

    [Header("Hookshot")]

    [SerializeField] [Range(50, 1000)] float range = 100f;
    [SerializeField] float cdTime = 0.2f;
    [SerializeField] [Range(3, 20)] float truthNukeRange = 3f;

    bool canHook = true;
    bool shouldHook => canHook && Input.GetKeyDown(hookKey);
    bool shouldBreak;

    [HideInInspector] public KeyCode hookKey = KeyCode.R;
    PlayerController pc;
    BaseEnemy enemy;



    void Start(){
        pc = GetComponent<PlayerController>();
    }

    IEnumerator HookCD(){
        canHook = false;
        yield return new WaitForSeconds(cdTime);
        canHook = true;
        Debug.Log("Hook CD done");
    }

    IEnumerator PullEnemy(RaycastHit hit, BaseEnemy ec, float a){
        enemy = ec;
        enemy.agent.enabled = false;
        float f = 0.01f;
        yield return new WaitForSeconds(f);
        ec.transform.position = mas.LerpVectors(hit.transform.position, transform.position, a);
        
        if(a < 1 && !shouldBreak){
            Debug.Log("Restarting!");
            a+=f;
            StartCoroutine(PullEnemy(hit, ec, a));
        }
        shouldBreak=false;
        enemy.agent.enabled = true;

    }   

    void Hook(){
        if(Physics.Raycast(transform.position, pc.playerCamera.transform.forward, out RaycastHit hit, range)){
            BaseEnemy ec = hit.collider.GetComponent<BaseEnemy>();

            if(ec != null){
                Debug.Log("Hooked enemy");
                StartCoroutine(PullEnemy(hit, ec, 0f));
            }
        }
    }


    void Update(){
        if(shouldHook){
            Hook();
        }
        if(enemy != null){
            if(mas.isInRadiusToPoint(enemy.transform.position,transform.position,"Player", truthNukeRange)){
                shouldBreak=true;
            }
        }
        
    }

}
