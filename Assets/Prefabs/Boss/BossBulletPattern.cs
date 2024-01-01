using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletPattern : MonoBehaviour
{
    protected ObjectPool bulletPool;
    protected ObjectPool bulletHitPool;
    protected ObjectPool muzzleFlashPool;

    public float timeBetweenFiring;
    
    private float timer;

    void Start(){
        bulletPool = GameObject.FindGameObjectWithTag("DeathHitPool").GetComponent<ObjectPool>();
        bulletHitPool = GameObject.FindGameObjectWithTag("DeathHitHitPool").GetComponent<ObjectPool>();
        muzzleFlashPool = GameObject.FindGameObjectWithTag("DeathHitMuzzleFlashPool").GetComponent<ObjectPool>();
    }

    protected virtual void Update(){
        if(!Boss.bossInstance.theBossIsDead){
            timer += Time.deltaTime;
            if(timer >= timeBetweenFiring){
                Fire();
                timer = 0;
            }
        }

    }




    public virtual void Fire(){

    }


}
