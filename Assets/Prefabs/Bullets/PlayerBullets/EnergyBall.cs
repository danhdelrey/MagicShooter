using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : Bullet
{
    

    public float duration;
    public int damageToIncrease;
    

    protected override void OnTriggerEnter2D(Collider2D hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();

        
        enemy.takeDamage(damage);

        Player.playerInstance.damageIncrease += damageToIncrease;
        Player.playerInstance.damageIncreaseTimer = duration;
        Player.playerInstance.increasedDamage = true;

        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);
    }

    



}
