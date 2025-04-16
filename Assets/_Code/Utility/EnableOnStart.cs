using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    void Start(){
        GetComponent<Canvas>().enabled = true;
    }
}
