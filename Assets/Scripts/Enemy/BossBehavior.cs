using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : Enemy
{   
    
    public static BossBehavior bossBehaviorInstance;
    [Space(10)]
    [Header("Boss")]
    public HealthBar bossHealthBar;
    public GameObject deathEffect;
    public AudioSource audioSource;
    public AudioSource bossDialogueSource;
    public AudioClip deathSound;
    public List<AudioClip> attackAudioList;
    public List<AudioClip> getHitAudioClip;
    public Animator animator;

    private int hitCount;

    [Header("Bullet Firing")]
    public List<GameObject> bulletPatternList;
    
    [System.Serializable]
    public class AirShoot{
      public List<GameObject> patternList; 
    }
    public List<AirShoot> airShootList;
    private bool onAirShoot = false;
    
    public GameObject impactEffect;
    
    void Awake(){
        bossBehaviorInstance = this;
    }

    protected override void Start()
    {
        currentHealth = maxHealth;
        bossHealthBar.SetMaxHealth(maxHealth);
    }

   
    public float audioTimer1, audioTimer2, audioTimer3;
    protected override void Update()
    {
        isBeingSlowedDown = false;
        bossHealthBar.SetHealth(currentHealth);
        
        if(!canPlayGetHitAudio){
            audioTimer1 += Time.deltaTime;
            if(audioTimer1 >= 5){
                audioTimer1 = 0;
                canPlayGetHitAudio = true;
            }
        }

        if(!canPlayFireAudio){
            audioTimer2 += Time.deltaTime;
            if(audioTimer2 >= 5){
                audioTimer2 = 0;
                canPlayFireAudio = true;
            }
        }

        if(!canPlayAirAttackAudio){
            audioTimer3 += Time.deltaTime;
            if(audioTimer3 >= 5){
                audioTimer3 = 0;
                canPlayAirAttackAudio = true;
            }
        }
       

   
       
    }

    public override void Die()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        Player.playerInstance.canAttachToMouse = false;
        Player.playerInstance.canShoot = false;
        bossDialogueSource.volume = 0;
        audioSource.PlayOneShot(deathSound);
        Boss.bossInstance.theBossIsDead = true;
        GetComponent<BossSpawning>().DestroyAllOfTheEnemy();
        deathEffect.SetActive(true);
    }

    
    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        int randomNumber = Random.Range(1,100);

        if(!onAirShoot){
            if(randomNumber > 20 && randomNumber <= 60 && !Boss.bossInstance.theBossIsDead){
                if(canPlayAirAttackAudio){
                    bossDialogueSource.PlayOneShot(attackAudioList[Random.Range(0,attackAudioList.Count-1)]);
    
                    canPlayAirAttackAudio = false;
                    canPlayGetHitAudio = false;
                    canPlayFireAudio = false;
                    StartCoroutine(Fire());
                       
                }
                    
            }
        }

        if(randomNumber>=1 && randomNumber <= 20 && !Boss.bossInstance.theBossIsDead){
            if(canPlayFireAudio){
                    if(!inFireState){
                        bossDialogueSource.PlayOneShot(attackAudioList[Random.Range(0,attackAudioList.Count-1)]);
                    
                        canPlayFireAudio = false;
                        canPlayAirAttackAudio = false;
                        canPlayGetHitAudio = false;
                        StartCoroutine(FlyAndFire());
                    }   
                }
        }

        if(randomNumber > 60 && randomNumber <= 100 && !Boss.bossInstance.theBossIsDead){
            if(!inFireState){
                if(canPlayGetHitAudio){
                    bossDialogueSource.PlayOneShot(getHitAudioClip[Random.Range(0,getHitAudioClip.Count-1)]);
                    canPlayGetHitAudio = false;
                    canPlayAirAttackAudio = false;
                    canPlayFireAudio = false;
                    StartCoroutine(getHit());
                }
                
            }
        }

        
        
    }

    private bool canPlayGetHitAudio = true;
   
    IEnumerator getHit(){
        impactEffect.SetActive(true);
        animator.SetBool("GetHit",true);
        yield return new WaitForSeconds(4f);
        animator.SetBool("GetHit",false);
    }

    IEnumerator FlyAndFire(){
        onAirShoot = true;
        
        animator.SetBool("Attack",true);
        GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(3.08f);
        int randomIndex = Random.Range(0,airShootList.Count-1);
        PatternOnAirFire(randomIndex);
        yield return new WaitForSeconds(10f);
        onAirShoot = false;

        stopAirFiring(randomIndex);
        animator.SetBool("Attack",false);
        StartCoroutine(ReturnToTheGround());
    }

    private bool inFireState = false;
    private bool canPlayAirAttackAudio = true;
    private bool canPlayFireAudio = true;
    
    IEnumerator Fire(){
        
        animator.SetBool("DeathHitShoot",true);
        inFireState = true;
        int randomIndex = Random.Range(0,bulletPatternList.Count-1);
        bulletPatternList[randomIndex].SetActive(true);

        yield return new WaitForSeconds(5f);
        inFireState = false;
        animator.SetBool("DeathHitShoot",false);
        bulletPatternList[randomIndex].SetActive(false);
    }


    IEnumerator ReturnToTheGround(){
        animator.SetBool("Return",true);
        yield return new WaitForSeconds(2.26f);
        animator.SetBool("Return",false);
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    


    void PatternOnAirFire(int index){
        foreach (GameObject pattern in airShootList[index].patternList)
        {
            pattern.SetActive(true);
        }
    }
    void stopAirFiring(int index){
        foreach (GameObject pattern in airShootList[index].patternList)
        {
            pattern.SetActive(false);
        }
    }


   

}
