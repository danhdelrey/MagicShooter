using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Creeps only")]
    public AudioSource creepAudioSource;
    public AudioClip sound;
    public float audioTimer;
    
    private ObjectPool deathEffectPool;
    [Header("Debuff effect")]
    public GameObject freezeEffect;
    public bool isBeingSlowedDown = false;
    protected bool isStunned = false;
    protected float stunDuration;
    public float slowedDownDuration;
    
    
    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    protected HealthBar healthBar;
    public float moveSpeed;
    private float curentMoveSpeed;
    [SerializeField] protected int killScore;
    

    protected Vector2 target;
    protected Vector3 bottomLeft,topRight;
    protected float minX,minY,maxX,maxY;

    [Space(10)]

    [Header("Firing Pattern")]
    public string bulletPoolTag;
    protected GameObject enemyBulletPool;
    public Transform firePoint;
    [SerializeField] protected float timeBetweenEachBullet;
    protected bool canFire = false;
    protected float timer1, timer2, timer3;
    
    [SerializeField] protected float timeForTheNextWave;
    [SerializeField] protected float firingTimeWhenInTheWave;
    protected bool canInitWave;

    protected float readyTimer;
    public bool isReady = false;
    [SerializeField] protected bool stopMovingWhileFiring;
    protected bool canMove = true;
    protected bool canInitBulletWave = true;
    

    protected virtual void Start(){
        deathEffectPool = GameObject.FindGameObjectWithTag("EnemyDeathEffectPool").GetComponent<ObjectPool>();
        healthBar = transform.GetChild(0).GetChild(0).gameObject.GetComponent<HealthBar>();
        currentHealth = maxHealth;
        curentMoveSpeed = moveSpeed;
        healthBar.SetMaxHealth(maxHealth);
        
    }

    

    protected virtual void Update(){
        
        healthBar.SetHealth(currentHealth);
        
        audioTimer += Time.deltaTime;
        if(audioTimer >= Random.Range(10f, 20f)){
            audioTimer = 0;
            creepAudioSource.PlayOneShot(sound);
        }

        getReadyIn(3);
        if(isReady){
            if(canMove) Move();
            if(canInitBulletWave) initBulletWave();

           if(isBeingSlowedDown){
                slowedDownDuration -= Time.deltaTime;
                freezeEffect.SetActive(true);
                if(slowedDownDuration <= 0){
                    slowedDownDuration = 0;
                    moveSpeed = curentMoveSpeed;
                    freezeEffect.SetActive(false);
                    isBeingSlowedDown = false;
                }
                
           }

            if(isStunned){
                stunDuration -= Time.deltaTime;
                if(stunDuration <= 0){
                    removeStunning();
                    stunDuration = 0;
                    isStunned = false;
                }
            }


        }
    }

    protected virtual void getReadyIn(int second){
        readyTimer += Time.deltaTime;
        if(readyTimer >= second){
            isReady = true;
        }
    }
    protected virtual void initBulletWave(){
            if(stopMovingWhileFiring){
                if(canInitWave){
                    canMove = false;
                    if(!canFire){
                        timer1 += Time.deltaTime;
                        timer3 += Time.deltaTime;

                        if(timer1 >= timeBetweenEachBullet){
                            canFire = true;
                            timer1 = 0;
                        }
                        if(timer3 >= firingTimeWhenInTheWave){
                            canMove = true;
                            canInitWave = false;
                            timer3 = 0;
                        }
                        
                    }else{
                        canFire = false;
                        firebulletPattern();
                    }
                }else{
                    timer2 += Time.deltaTime;
                    if(timer2 >= timeForTheNextWave){
                        canInitWave = true;
                        timer2 = 0;
                    }
                }
            }else{
                if(canInitWave){
                    if(!canFire){
                        timer1 += Time.deltaTime;
                        timer3 += Time.deltaTime;

                        if(timer1 >= timeBetweenEachBullet){
                            canFire = true;
                            timer1 = 0;
                        }
                        if(timer3 >= firingTimeWhenInTheWave){
                            canInitWave = false;
                            timer3 = 0;
                        }
                        
                    }else{
                        canFire = false;
                        firebulletPattern();
                    }
                }else{
                    timer2 += Time.deltaTime;
                    if(timer2 >= timeForTheNextWave){
                        canInitWave = true;
                        timer2 = 0;
                    }
                }
            }
    }

    public virtual void takeDamage(int damage){
        currentHealth -= (damage + Player.playerInstance.damageIncrease);
        if(currentHealth<=0){
            Die();
        }
        
    }
    public virtual void initEnemy(){
        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3((float)0.5,0,0));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
        
        transform.position = new Vector3(maxX+3,Random.Range(minY,maxY),0);
        target = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY));
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }

    public virtual void Die(){
        GameObject deathEffect = deathEffectPool.getObjectFromPool();
        deathEffect.transform.position = transform.position;
        deathEffect.transform.rotation = transform.rotation;
        deathEffect.SetActive(true);
    
        if(isBeingSlowedDown) freezeEffect.SetActive(false);
        gameObject.SetActive(false);
        ScoreManager.scoreManagerInstance.updateScore(killScore);
        
    }

    protected virtual void Move(){
        transform.position = Vector2.MoveTowards(transform.position,target,moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position,target) == 0){
            target = new Vector2(Random.Range(minX,maxX),Random.Range(minY,maxY));
        }
    }

    protected virtual void firebulletPattern(){}

    
    

    public void getStunned(float stunDuration){
        this.stunDuration = stunDuration;
        isStunned = true;
        canInitBulletWave = false;
        canMove = false;
    }
    protected virtual void removeStunning(){
        canInitBulletWave = true;
        canMove = true;
    }
    
    

    
}
