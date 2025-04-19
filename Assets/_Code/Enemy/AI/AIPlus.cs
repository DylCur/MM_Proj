using MathsAndSome;
using UnityEngine;

namespace AIPlus{
    public static class aip
        {
            static Rigidbody rb = new Rigidbody();
            static Transform transform;

            static Vector3 GetDirectionToVector(Vector3 v){
                return new Vector3(v.x,0,v.z);
            }

            public static void GoToDestination(Vector3 d){
                Vector3 moveDirection = GetDirectionToVector(d);
                
                Ray rayToD = new Ray(transform.position, d-transform.position);
            }

            public static void GoToPlayer(){
                GoToDestination(mas.GetPlayer().transform.position);
            }

            public static void GoToGameObject(GameObject obj){
                GoToDestination(obj.transform.position);
            }
        }
    }
