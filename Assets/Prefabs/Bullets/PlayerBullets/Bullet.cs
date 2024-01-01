using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet bulletInstance;
    protected float powerUpTimer;
    public int damage;
    
    public float speed;
    
    public string effectPoolTag;
    protected ObjectPool effectPool;

    protected Vector2 moveDirection;

    void Awake(){
        bulletInstance = this;
    }


    protected virtual void Start(){
        
        effectPool = GameObject.FindGameObjectWithTag(effectPoolTag).GetComponent<ObjectPool>();
    }

    protected virtual void Update(){
        move();
        
    }

    void OnBecameInvisible(){
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D hitObject){
        Enemy enemy = hitObject.GetComponent<Enemy>();
        enemy.takeDamage(damage);
        GameObject effect = effectPool.getObjectFromPool();
        effect.SetActive(true);
        effect.transform.position = transform.position;
        effect.transform.rotation= transform.rotation;
        gameObject.SetActive(false);

    }


    public void setNewMoveDirection(Vector2 direction){
        moveDirection = direction;
    }

    void move(){
        transform.Translate(moveDirection * (speed + Player.playerInstance.bulletSpeed) * Time.deltaTime);
    }


   
    
    
    
    

}
