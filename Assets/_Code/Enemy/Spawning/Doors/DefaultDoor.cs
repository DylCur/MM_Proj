using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DefaultDoor : DoorController
{
    BoxCollider bc;

    void Start(){
        bc = GetComponent<BoxCollider>();
    }

    public override void OpenDoor()
    {
        bc.isTrigger = true;


    }

    public override void CloseDoor()
    {
        bc.isTrigger = false;
    }
}
