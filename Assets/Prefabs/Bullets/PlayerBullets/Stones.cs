using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stones : Bullet
{
    public float stunDuration;
    public int stunChance;
    

    

    protected override void OnTriggerEnter2D(Collider2D hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();

        enemy.takeDamage(damage);

        int randomNumber = Random.Range(0,100);
        if(stunChance <= randomNumber){
            enemy.getStunned(stunDuration);
        }

        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);
    }



}
