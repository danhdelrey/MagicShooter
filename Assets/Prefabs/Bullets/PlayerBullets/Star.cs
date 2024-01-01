using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Bullet
{

    public float duration;
    public float speedDecrease;


    protected override void OnTriggerEnter2D(Collider2D hitObject)
    {
        Enemy enemy = hitObject.GetComponent<Enemy>();

        enemy.takeDamage(damage);

        Player.playerInstance.increaseFiringSpeed(duration,speedDecrease);

        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);
    }

}
