using System;
using System.Collections;
using UnityEngine;
using MathsAndSome;
using Globals;

public class HookshotController : MonoBehaviour
{

    [Header("Hookshot")]

    [SerializeField] [Range(50, 1000)] float range = 100f;
    public float cdTime = 0.2f;
    [SerializeField] [Range(3, 20)] float truthNukeRange = 3f;

    [HideInInspector] public bool canHook = true;
    bool shouldHook => canHook && Input.GetKeyDown(hookKey);
    bool shouldBreak;

    [HideInInspector] public KeyCode hookKey = KeyCode.R;   
    PlayerController pc;
    BaseEnemy enemy;

    [SerializeField] LayerMask enemyLayer;



    void Start(){
        pc = GetComponent<PlayerController>();
    }

    IEnumerator HookCD(){
        StartCoroutine(GameObject.FindGameObjectWithTag("Canvas").GetComponent<EnableOnStart>().LerpHookshotColour(cdTime/20));
        canHook = false;
        yield return new WaitForSeconds(cdTime);
        canHook = true;
        Debug.Log("Hook CD done");
    }

    IEnumerator PullEnemy(RaycastHit hit, BaseEnemy ec, float a){
        enemy = ec; 
        ec.s = EState.hooked;

        float f = 0.01f;
        yield return new WaitForSeconds(f);
        ec.transform.position = mas.LerpVectors(hit.transform.position, transform.position, a);
        
        if(a < 1 && !shouldBreak){
            Debug.Log("Restarting!");
            a+=f;
            StartCoroutine(PullEnemy(hit, ec, a));
        }
        else{
            enemy=null;
        }

        StartCoroutine(HookStateCD(ec));
        shouldBreak=false;

    }   

    IEnumerator HookStateCD(BaseEnemy ec){
        yield return new WaitForSeconds(10f);
        ec.s = EState.seeking;
    }

    void Hook(){
        if(Physics.Raycast(transform.position, pc.playerCamera.transform.forward, out RaycastHit hit, range, enemyLayer)){ 
            BaseEnemy ec = hit.collider.GetComponent<BaseEnemy>();

            if(ec != null){
                Debug.Log("Hooked enemy");
                StartCoroutine(PullEnemy(hit, ec, 0f));
            }
            else{
                if(hit.collider.GetComponent<ProjectileController>()!=null){
                    Debug.Log($"Hit {hit.collider.name}");
                }
            }
        }

        StartCoroutine(HookCD());
    }


    void Update(){
        if(shouldHook){
            Hook();
        }
        if(enemy != null){
            if(mas.isInRadiusToPoint(enemy.transform.position,transform.position,glob.playerTag, truthNukeRange)){
                shouldBreak=true;
            }
        }
        
    }

}
