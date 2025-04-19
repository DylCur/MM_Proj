using System;
using System.Collections;
using MathsAndSome;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnableOnStart : MonoBehaviour
{
    CombatController cc;
    PlayerController pc;

    [Header("Health")]

    [SerializeField] TMP_Text healthText;
    [SerializeField] Slider healthSlider;
    
    [Header("Stamina")]

    [SerializeField] TMP_Text staminaText;
    [SerializeField] Slider staminaSlider;

    [SerializeField] Image hookshotIndicator;

    [Header("Colours")]
    [SerializeField] Color hookshotInactiveColor;
    [SerializeField] Color hookshotActiveColor;

    HookshotController hc;

    void Start(){
        GetComponent<Canvas>().enabled = true;
        cc = mas.GetPlayer().gameObject.GetComponent<CombatController>();
        pc = mas.GetPlayer();
        hc = mas.GetPlayer().gameObject.GetComponent<HookshotController>();
        
        hookshotIndicator.color = hookshotActiveColor;
    }

    void Update(){
        healthText.text = cc.health.ToString();
        healthSlider.value = (float)cc.health/100;
        
        staminaText.text = pc.stamina.ToString();
        staminaSlider.value = pc.stamina;
    }

    Color LerpColors(Color a, Color b, float t){
        return new Color(Mathf.Lerp(a.r,b.r,t),Mathf.Lerp(a.g,b.g,t),Mathf.Lerp(a.b,b.b,t));
    }

    public IEnumerator LerpHookshotColour(float t){   
        
        hookshotIndicator.color = LerpColors(hookshotInactiveColor, hookshotActiveColor, t);

        yield return new WaitForSeconds(hc.cdTime / 10);
        
        if(t < 1){
            t*=2;
            StartCoroutine(LerpHookshotColour(t));
        }


    }
}
