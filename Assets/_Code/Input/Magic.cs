using UnityEngine;
using System.Collections;

namespace Magical{
    public static class keys{
        //* Movement
        public static KeyCode[] left = {KeyCode.LeftArrow, KeyCode.A};
        public static KeyCode[] right = {KeyCode.RightArrow, KeyCode.D};
        public static KeyCode[] jump = {KeyCode.Space};
        
        //* Combat
        public static KeyCode[] attack = {KeyCode.Mouse0};
    }

    public static class magic
    {
        public static class key{
            public static bool down(KeyCode[] key){
                foreach(KeyCode k in key){
                    if(Input.GetKeyDown(k)){
                        return true;
                    }
                }
                return false;
            }    

            public static bool up(KeyCode[] key){
                foreach(KeyCode k in key){
                    if(Input.GetKeyUp(k)){
                        return true;
                    }
                }
                return false;
            } 

            public static bool gk(KeyCode[] key){
                foreach(KeyCode k in key){
                    if(Input.GetKey(k)){
                        return true;
                    }
                }
                return false;
            }

            public static KeyCode PressedKey(){
                foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(kcode)){return kcode;}     
                }

                // Dont think anyones missing this key, and it should never be returned
                return KeyCode.DoubleQuote;
            }              
        }
        
        
    }    
}
