using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;
    public float speed;

    private Vector2 moveDirection;
    public string HitEffectPoolTag;
    private ObjectPool hitEffectPool;

    

    void Start(){
        hitEffectPool = GameObject.FindGameObjectWithTag(HitEffectPoolTag).GetComponent<ObjectPool>();
    }

    void Update(){
        move();
    }

    

    public void OnBecameInvisible(){
        gameObject.SetActive(false);
    }
    
    void OnTriggerEnter2D(Collider2D hitObject){
        Player player = hitObject.GetComponent<Player>();
        if(player != null){
            
            GameObject hitEffect = hitEffectPool.getObjectFromPool();
            hitEffect.transform.position = transform.position;
            hitEffect.transform.rotation = transform.rotation;
            hitEffect.SetActive(true);

            player.takeDamage(damage);
            gameObject.SetActive(false);
        }
        
    }

    
    

    public void setNewMoveDirection(Vector2 direction){
        moveDirection = direction;
    }

    void move(){
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public virtual IEnumerator disable(float seconds){
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    

}
