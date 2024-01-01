using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAwayFromMe : MonoBehaviour
{
    public ObjectPool deathHitPool;
    public int damage;
    public float hitDuration;
    private bool staying = false;
    private float timer;
    void Update(){
        if(staying){
            timer += Time.deltaTime;
            if(timer >= hitDuration){
                Player.playerInstance.takeDamage(damage);
                GameObject deathHit = deathHitPool.getObjectFromPool();
                deathHit.transform.position = Player.playerInstance.transform.position;
                deathHit.transform.rotation = Player.playerInstance.transform.rotation;
                deathHit.SetActive(true);
                timer = 0;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider){
        staying = true;
    }
    void OnTriggerExit2D(Collider2D collider){
        staying = false;
    }
}
