using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGun : MonoBehaviour
{
    void Update()
    {
        if(Player.playerInstance.playerIsDead){
            gameObject.SetActive(false);
        }
    }
}
