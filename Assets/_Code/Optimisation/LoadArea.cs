using Globals;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LoadArea : MonoBehaviour
{
    [SerializeField] GameObject areaToLoad;
    [SerializeField] GameObject areaToHide;   

    void Load(){
        areaToHide.SetActive(true);
    }

    void Hide(){
        areaToHide.SetActive(false);
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == glob.playerTag){
            
            if(areaToLoad != null){
                Load();
            }

            if(areaToHide != null){
                Hide();
            }

            Destroy(gameObject);
        }
    }

    void Start(){
        if(areaToLoad!=null){
            areaToLoad.SetActive(false);
        }
    }
}
