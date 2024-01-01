using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour
{
    private float timer;
    void Update(){
        if(gameObject.activeInHierarchy){
            timer += Time.deltaTime;
            if(timer >= 2){
                timer = 0;
                gameObject.SetActive(false);
            }
        }
    }
    
}
