using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Love : Bullet
{
    public float duration;
    public int speedToIncrease;
    

    protected override void OnTriggerEnter2D(Collider2D hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();
        enemy.takeDamage(damage);

        Player.playerInstance.bulletSpeed += speedToIncrease;
        Player.playerInstance.bulletIncreaseTimer = duration;
        Player.playerInstance.increasedBulletSpeed = true;

        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);
    }

}
