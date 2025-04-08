using UnityEngine;
using Globals;
using System.Collections.Generic;
using Unity.VisualScripting;


// public float detectionRadius;
//     public float damage;
//     public float atkTime;

//     protected bool canSeekPlayer => inRange() && !seeking;
//     protected bool shouldSeekPlayer => s == eState.walking && canSeekPlayer;

//     protected bool canAttack = true;

//     protected bool seeking;

//     public GameObject player;
//     protected bool shouldRedirect = true;
//     protected float delay = 0.01f;
//     [SerializeField] protected Vector3 attackRange;



public struct Enemy{
    float damage;
    float atkTime;
    public BaseEnemy be;
    public Vector3 spawnPos;

    public Enemy(float dmg, float time, BaseEnemy baseEnemy, Vector3 sp){
        damage = dmg;
        atkTime = time;
        be = baseEnemy;
        spawnPos = sp;
    }
}

public class EnemyWave{
    public List<Enemy> eList;
    public List<float> spawnTime;

    protected void AddItem(float n){
        spawnTime.RemoveAt(0);

            for(int i = 0; i < eList.Count - 1; i++){
                spawnTime.Add(n);
            }
    }

    protected void UpdateSpawnTimes(){
        if(spawnTime.Count == 1){
            AddItem(spawnTime[0]);
        }
        else if(spawnTime.Count == 0){
            AddItem(0);
        }
        
    }

    public EnemyWave(List<Enemy> enemies, List<float> times){
        eList = enemies;
        spawnTime = times;
        UpdateSpawnTimes();
    }
}

[RequireComponent(typeof(BoxCollider))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int currentWave = 0;

    public List<EnemyWave> waves; 

    void Init(){
        GetComponent<BoxCollider>().enabled = false;
    }

    void SpawnWaves(){
        EnemyWave w = waves[currentWave];

        for(int i = 0; i < w.eList.Count - 1; i ++){
            // w.eList[i].be = Instantiate()

        }

        foreach(Enemy e in w.eList){
        }
    }



    void OnTriggerEnter(Collider other){
        if(other.tag == glob.playerTag){
            Init();
            SpawnWaves();
        }
    }
}
