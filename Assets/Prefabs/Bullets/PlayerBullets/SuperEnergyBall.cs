using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperEnergyBall : Bullet
{

    public float explosionRadius;

    
    protected override void OnTriggerEnter2D(Collider2D hitObject)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            Enemy enemyInRange = collider.GetComponent<Enemy>();
            if(enemyInRange!=null){
                enemyInRange.takeDamage(damage);
            }
        }




        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);
    }
}
