using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public AudioSource backgroundSource;
    public AudioClip backgroundMusic;
    private Animator animator;
    public GameObject playerSprite;
    public bool playerIsDead = false;
    [Header("Effects")]
    public ObjectPool muzzleFlashEffectPool;
    public GameObject deathEffect;
    public List<ObjectPool> buffEffectPools;
    public List<Transform> effectPositionList;
    public ObjectPool upgradeEffectPool;
    public Transform upgradeEffectPosition;
    

    
    [Header("Protection")]
    public int damageReduction;
    private int currentDmgReduction;
    private bool isProtected = false;
    private float protectionTimer;
    public GameObject protectionEffect;
    public Transform protectionPosition;

    [Header("Buff")]
    public GameObject firingSpeedEffect;
    public Transform firingSpeedEffectPosition;

    public GameObject damageIncreaseEffect;
    public Transform damageIncreaseEffectPosition;
    public int damageIncrease;
    [HideInInspector] public bool increasedDamage = false;
    [HideInInspector] public float damageIncreaseTimer;



    public GameObject bulletSpeedIncreaseEffect;
    public Transform bulletSpeedEffectPosition;
    public int bulletSpeed;
    [HideInInspector] public bool increasedBulletSpeed = false;
    [HideInInspector] public float bulletIncreaseTimer;
   



    public static Player playerInstance;
    private Vector2 playerPosition;
    [Header("Player Stats")]
    [SerializeField] private float maxHealth;
    private float currentHealth;
    [SerializeField] private HealthBar healthBar;
    public GameObject healthBarUI;
    private int score;
    public bool canAttachToMouse = true;


    [Space(10)]
    [Header("Shooting Controller")]
    public bool canShoot = true;
    [SerializeField] private List<ObjectPool> bulletPoolList;
    
    [SerializeField] private Transform firePoint;
    public float timeBetweenFiring;
    public List<float> timeBetweenFiringList;
    private bool canFire = true;
    private float timer;
    private float readyTimer;
    public bool isReady = false;

    [Space(10)]
    [Header("Gun Controller")]
    public GunBag gunBag;
    private int previousGunIndex;
    [SerializeField] private Transform gunPosition;
    [SerializeField] private ObjectPool gunPool;
    private int current_gun;


    [Header("Gun[i] to Gun[i+1] needs Score[i] scores!")]
    [SerializeField] private List<int> scoreToUpgradeGun;
    private int currentScoreLevel;
    private List<GameObject> gunList;
    private bool canUpgrade;

    [Header("Gun Explanation UI")]
    public List<GunExplanation> gunExplanations;
    private bool isExplained = false;
    
    
    void Awake(){
        playerInstance = this;
    }

    void Start(){
        gunBag.ActivateFrame(0);
        backgroundSource.clip = backgroundMusic;
        backgroundSource.loop = true;
        backgroundSource.Play();


        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        hideCursor();
        getFirstGun();
        setFirstPosition();
    }

    private bool startGame = false;
    void Update(){
        if(Boss.bossInstance.theBossIsDead) canDie = false;
        healthBar.SetHealth(currentHealth);

        score = ScoreManager.scoreManagerInstance.getCurrentScore();
        playerPosition = transform.position;
        if(!startGame) moveFromFirstPosition();

        updateCurrentScoreLevel();
        if(isReady){
            if(!isExplained){
                gunExplanations[0].ShowUp();
                isExplained = true;
            }
            if(canAttachToMouse)attachToMouse();
            if(canShoot) Fire();
        }
        if(isProtected){
            protectionTimer -= Time.deltaTime;
            if(protectionTimer <= 0){
                protectionTimer = 0;
                protectionEffect.SetActive(false);
                damageReduction = 0;
                isProtected = false;
            }
        }
        if(setSpeed){
            firingSpeedTimer -= Time.deltaTime;
            if(firingSpeedTimer <= 0){
                firingSpeedTimer = 0;
                firingSpeedEffect.SetActive(false);
                setSpeed = false;
            }
        }else{
            timeBetweenFiring = timeBetweenFiringList[previousGunIndex];
        }

        if(increasedDamage){
            damageIncreaseTimer -= Time.deltaTime;

            damageIncreaseEffect.transform.position = damageIncreaseEffectPosition.position;
            damageIncreaseEffect.transform.rotation = damageIncreaseEffectPosition.rotation;
            damageIncreaseEffect.SetActive(true);

            if(damageIncreaseTimer <= 0){
                damageIncreaseTimer = 0;
                damageIncrease = 0;
                damageIncreaseEffect.SetActive(false);
                increasedDamage = false;
            }
        }

        if(increasedBulletSpeed){
            bulletIncreaseTimer -= Time.deltaTime;

            bulletSpeedIncreaseEffect.transform.position = bulletSpeedEffectPosition.position;
            bulletSpeedIncreaseEffect.transform.rotation = bulletSpeedEffectPosition.rotation;
            bulletSpeedIncreaseEffect.SetActive(true);

            if(bulletIncreaseTimer <= 0){
                bulletIncreaseTimer = 0;
                bulletSpeed = 0;
                bulletSpeedIncreaseEffect.SetActive(false);
                increasedBulletSpeed = false;
            }
        }

        

    }


    void setFirstPosition(){
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,(float)0.5,0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3((float)0.5, 1, 0));

        float minX = bottomLeft.x;
        float minY = bottomLeft.y;
        float maxX = topRight.x;
        float maxY = topRight.y;

        Vector2 firstPosition = new Vector2(minX-3,minY);
        

        transform.position = firstPosition;
    }
    
    void moveFromFirstPosition(){
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,(float)0.5,0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3((float)0.25, 1, 0));

        float minX = bottomLeft.x;
        float minY = bottomLeft.y;
        float maxX = topRight.x;
        float maxY = topRight.y;

        Vector2 targetPosition = new Vector2(maxX,0);

        transform.position = Vector2.MoveTowards(transform.position,targetPosition,5 * Time.deltaTime);
        if(Vector2.Distance(transform.position,targetPosition)==0){
            isReady = true;
            startGame = true;
        }

    }

    void attachToMouse(){
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float minX = bottomLeft.x;
        float minY = bottomLeft.y;
        float maxX = topRight.x;
        float maxY = topRight.y;

        mousePos.x = Mathf.Clamp(mousePos.x, minX, maxX);
        mousePos.y = Mathf.Clamp(mousePos.y, minY, maxY);

        transform.position = new Vector2(mousePos.x, mousePos.y);
    }

    void hideCursor(){
        Cursor.visible = false;
    }
    public Vector2 getPlayerPosition(){
        return playerPosition;
    }
    public void disableAttachToMouse(){
        canAttachToMouse = false;
    }
    public void enableAttachToMouse(){
        canAttachToMouse = true;
    }

    private bool canDie = true;
    public void takeDamage(int damage){
        damage = damage-damageReduction;
        if(damage <= 0) damage = 0;
        currentHealth -= damage;
        if(currentHealth<=0){
            if(canDie) StartCoroutine(Die());
        }
    }
    public void setDamageReduction(int newDmgReduction){
        damageReduction = newDmgReduction;
    }

    IEnumerator Die(){
        playerIsDead = true;
        canAttachToMouse = false;
        canShoot = false;
        healthBarUI.SetActive(false);
        playerSprite.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        deathEffect.SetActive(true);
        yield return new WaitForSeconds(4f);
        MenuManger.menuMangerInstance.showDeadMenu();
        backgroundSource.Stop();
    }

    public bool canChangeGun;
    void Fire(){
        canUpgrade = checkUpgrade();
        if(canChangeGun) changeGun();
        if(!canFire){
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring){
                canFire = true;
                timer = 0;
            }
        }else{
            canFire = false;
            if(canUpgrade){
                instantiateUpgradeEffect();
                unlockGun();
            }
            fireBullet();
        }
    }
    

    void fireBullet(){

        GameObject muzzleFlash = muzzleFlashEffectPool.getObjectFromPool();
        muzzleFlash.transform.position = firePoint.position;
        muzzleFlash.SetActive(true);
        
        GameObject bullet = bulletPoolList[previousGunIndex].getObjectFromPool();
        Bullet bul = bullet.GetComponent<Bullet>();
        bullet.SetActive(true);
        bul.setNewMoveDirection(Vector2.right);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        StartCoroutine(playAttackAnimation());
    }

    IEnumerator playAttackAnimation(){
        animator.SetBool("Attack",true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Attack",false);
    }

    bool checkUpgrade(){
        if(current_gun < scoreToUpgradeGun.Count){
            if(currentScoreLevel >= scoreToUpgradeGun[current_gun]){
                return true;
            }
            return false;
        }
        return false;
    }

    private bool noChangingGun = true;
    void unlockGun(){
        
        currentScoreLevel = scoreToUpgradeGun[current_gun];
        if(current_gun < gunList.Count){
            current_gun++;
            if(noChangingGun){
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(current_gun);
                previousGunIndex++;
                
            }
            
            
        }
        gunBag.gunList[current_gun].SetActive(true);
        gunExplanations[current_gun].ShowUp();
        
    }

   void getFirstGun(){
        gunList = gunPool.getMultipleObjectsList();
        gunList[0].transform.position = gunPosition.position;
        gunList[0].transform.rotation = gunPosition.rotation;
        gunList[0].SetActive(true);
   }
   void updateCurrentScoreLevel(){
        if(current_gun < scoreToUpgradeGun.Count){
            if(score >= scoreToUpgradeGun[current_gun]){
                currentScoreLevel = scoreToUpgradeGun[current_gun];
            }
        }
   }

    public void Heal(float healRate){
        currentHealth += (maxHealth - currentHealth)*healRate;
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
    }

    


    void getGun(int previousGunIndex, int gunIndex){
        gunList = gunPool.getMultipleObjectsList();
        gunList[gunIndex].transform.position = gunPosition.position;
        gunList[gunIndex].transform.rotation = gunPosition.rotation;

        gunList[previousGunIndex].SetActive(false);
        gunList[gunIndex].SetActive(true);
        //effect
        
    }
  
    void changeGun(){
       if(Input.GetKeyDown(KeyCode.Alpha1)){
            if(gunBag.gunList[0].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(0);
                getGun(previousGunIndex,0);
                previousGunIndex = 0;
                instantiateUpgradeEffect();
            }
            
       }
       if(Input.GetKeyDown(KeyCode.Alpha2)){
            if(gunBag.gunList[1].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(1);
                getGun(previousGunIndex,1);
                previousGunIndex = 1;
                instantiateUpgradeEffect();
            }
            
       }
       if(Input.GetKeyDown(KeyCode.Alpha3)){
            if(gunBag.gunList[2].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(2);
                getGun(previousGunIndex,2);
                previousGunIndex = 2;
                instantiateUpgradeEffect();
            }
            
       }
       if(Input.GetKeyDown(KeyCode.Alpha4)){
            if(gunBag.gunList[3].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(3);
                getGun(previousGunIndex,3);
                previousGunIndex = 3;
                instantiateUpgradeEffect();
            }
            
       }
       if(Input.GetKeyDown(KeyCode.Alpha5)){
            if(gunBag.gunList[4].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(4);
                getGun(previousGunIndex,4);
                previousGunIndex = 4;
                instantiateUpgradeEffect();
            }
            
       }
       if(Input.GetKeyDown(KeyCode.Alpha6)){
            if(gunBag.gunList[5].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(5);
                getGun(previousGunIndex,5);
                previousGunIndex = 5;
                instantiateUpgradeEffect();
            }
            
       }
       if(Input.GetKeyDown(KeyCode.Alpha7)){
            if(gunBag.gunList[6].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(6);
                getGun(previousGunIndex,6);
                previousGunIndex = 6;
                instantiateUpgradeEffect();
            }
            
       }
       if(Input.GetKeyDown(KeyCode.Alpha8)){
            if(gunBag.gunList[7].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(7);
                getGun(previousGunIndex,7);
                previousGunIndex = 7;
                instantiateUpgradeEffect();
            }
            
       }
       if(Input.GetKeyDown(KeyCode.Alpha9)){
            if(gunBag.gunList[8].activeInHierarchy){
                noChangingGun = false;
                gunBag.DeactivateFrame(previousGunIndex);
                gunBag.ActivateFrame(8);
                getGun(previousGunIndex,8);
                previousGunIndex = 8;
                instantiateUpgradeEffect();
            }
            
       }
    }


    public void instantiateEffectFromPool(int poolIndex){
        GameObject effect = buffEffectPools[poolIndex].getObjectFromPool();
        effect.transform.position = effectPositionList[poolIndex].position;
        effect.transform.rotation = effectPositionList[poolIndex].rotation;
        effect.SetActive(true);
    }


    public void createShield(int damageReduction, float protectionTimer){
        currentDmgReduction = this.damageReduction;
        this.protectionTimer = protectionTimer;
        this.damageReduction = damageReduction;

        protectionEffect.transform.position = protectionPosition.position;
        protectionEffect.transform.rotation = protectionPosition.rotation;
        protectionEffect.SetActive(true);
        isProtected = true;
    }
    
    private float firingSpeedTimer;
    private bool setSpeed;

    public void increaseFiringSpeed(float duration , float timeReduction){
        timeBetweenFiring -= timeReduction;
        if(timeBetweenFiring <= 0.1f){
            timeBetweenFiring = 0.1f;
        }
        firingSpeedTimer = duration;
        firingSpeedEffect.SetActive(true);
        setSpeed = true;
    }
    
    void instantiateUpgradeEffect(){
        GameObject effect = upgradeEffectPool.getObjectFromPool();
        effect.transform.position = upgradeEffectPosition.position;
        effect.SetActive(true);
    }



}
