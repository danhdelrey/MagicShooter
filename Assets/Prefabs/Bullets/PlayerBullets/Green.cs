using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : Bullet
{
    public float healRate;

    protected override void OnTriggerEnter2D(Collider2D hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();

        enemy.takeDamage(damage);
        Player.playerInstance.Heal(healRate);
        Player.playerInstance.instantiateEffectFromPool(0);

        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);

    }
}
