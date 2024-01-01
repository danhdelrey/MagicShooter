using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : Bullet
{
    public float explosionRadius;
    public float slowRate;
    private ObjectPool slowEffectPool;
    public float duration;

   
    

    protected override void OnTriggerEnter2D(Collider2D hitObject)
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,explosionRadius);
            foreach (Collider2D collider in colliders)
            {
                Enemy enemyInRange = collider.GetComponent<Enemy>();
                if(enemyInRange!=null){
                    
                    if(!enemyInRange.isBeingSlowedDown){
                        enemyInRange.moveSpeed *= slowRate;
                        
                        enemyInRange.slowedDownDuration += duration;
                        enemyInRange.isBeingSlowedDown = true;
                    }else{
                        enemyInRange.slowedDownDuration += duration;
                        enemyInRange.isBeingSlowedDown = true;
                    }
                        
                   
                    
                    
                    
                    
                    
                    
                }
            }
        
        Enemy enemy = hitObject.GetComponent<Enemy>();
        enemy.takeDamage(damage);

        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);

    }

    

   
}
