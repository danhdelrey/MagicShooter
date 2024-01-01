using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holy : Bullet
{
    
    public int damageReduction;
    public float duration;
   


    protected override void OnTriggerEnter2D(Collider2D hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();

        enemy.takeDamage(damage);
        Player.playerInstance.createShield(damageReduction,duration);

        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);
    }


}
