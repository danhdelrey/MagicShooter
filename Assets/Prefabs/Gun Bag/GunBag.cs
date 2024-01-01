using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBag : MonoBehaviour
{
    public List<GameObject> gunList;
    public List<GameObject> activeFrameList;
    public List<GameObject> gunSpriteList;





    void Start(){
        gunList = new List<GameObject>();
        activeFrameList = new List<GameObject>();
        gunSpriteList = new List<GameObject>();



        for (int i = 0; i < 9; i++)
        {
            GameObject activeFrame = gameObject.transform.GetChild(i).GetChild(0).gameObject;
            activeFrameList.Add(activeFrame);
        }
        for (int i = 0; i < 9; i++)
        {
            GameObject gunSprite = gameObject.transform.GetChild(i).GetChild(2).gameObject;
            gunSpriteList.Add(gunSprite);
        }
        for (int i = 0; i < 9; i++)
        {
            GameObject gun = gameObject.transform.GetChild(i).gameObject;
            gun.SetActive(false);
            gunList.Add(gun);
        }

        gunList[0].SetActive(true);
    }


    public void ActivateFrame(int gunIndex){
        activeFrameList[gunIndex].SetActive(true);
    }
    public void DeactivateFrame(int gunIndex){
        activeFrameList[gunIndex].SetActive(false);
    }

}
