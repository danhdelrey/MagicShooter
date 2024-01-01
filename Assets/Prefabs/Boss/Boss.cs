using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Boss bossInstance;
    [Header("Show Up")]
    public bool callBoss = false;
    public GameObject shakeEffect;
    public GameObject boss;
    public GameObject healthBar;
    public Material dissolve;
    public bool showedUp = false;

    [Header("Die")]
    public GameObject portal;
    private float deathTimer;
    public bool theBossIsDead = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip showUpAudio;

    public AudioSource showUpSource;
    public AudioClip showUpMusic;
    private bool playedShowUpAudio = false;

    public AudioSource bossFightSource;
    public AudioClip bossFightMusic;
    
    


    


    private float timer1,timer2, timer3, timer4;





    void Awake(){
        bossInstance = this;
    }

    void Start(){
        
        dissolve.SetFloat("_Fade",0f);
    }



    void Update(){
       
        if(callBoss) showUp();
        if(theBossIsDead && !Player.playerInstance.playerIsDead) isDead();
    }



    void showUp(){
        timer3 += Time.deltaTime;
        if(timer3 >= 6){
            timer1 += Time.deltaTime;
            if(timer1 >= 2){
                if(timer2 <= 1){
                    timer2 += Time.deltaTime/2;
                    dissolve.SetFloat("_Fade",timer2);
                    
                }
            }
            if(!showedUp){
                healthBar.SetActive(true);
                boss.SetActive(true);
                timer4 += Time.deltaTime;
                if(timer4 >= 5){
                    Player.playerInstance.canShoot = true;
                    bossFightSource.Stop();

                    bossFightSource.clip = bossFightMusic;
                    bossFightSource.loop = true;
                    bossFightSource.Play();

                    showedUp = true;
                }
            }
        }else{
            shakeEffect.SetActive(true);
            if(!playedShowUpAudio){
                showUpSource.PlayOneShot(showUpMusic);
                audioSource.PlayOneShot(showUpAudio);
                playedShowUpAudio = true;
            }
        }
    }

    void isDead(){
            deathTimer += Time.deltaTime;
            if(deathTimer >= 4){
                boss.SetActive(false);
                healthBar.SetActive(false);
                portal.SetActive(true);
            }
            if(deathTimer >= 10){
                
                Player.playerInstance.transform.position = Vector2.MoveTowards(Player.playerInstance.transform.position, portal.transform.position,Time.deltaTime*5);
                if(Vector2.Distance(Player.playerInstance.transform.position, portal.transform.position)==0){
                    Player.playerInstance.backgroundSource.Stop();
                    MenuManger.menuMangerInstance.showWinMenu();
                }
    
            }
    }


}
