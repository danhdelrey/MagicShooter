using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunExplanation : MonoBehaviour
{
    public bool showedUp = false;

    
    void Update(){
        if(showedUp){
            Player.playerInstance.canChangeGun = false;
            if(Input.GetMouseButtonDown(0)){
                Time.timeScale = 1f;
                showedUp = false;
                Player.playerInstance.canChangeGun = true;
                Player.playerInstance.isReady = true;
                gameObject.SetActive(false);
            }
        }
    }



    public void ShowUp(){
        Time.timeScale = 0f;
        showedUp = true;
        Player.playerInstance.isReady = false;
        gameObject.SetActive(true);
    }
}
