using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    [Space(10)]
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;
    [SerializeField] private int amountOfBullets;
    public string muzzleFlashPoolTag;
    private ObjectPool muzzleFlashPool;
    public Animator animator;

    protected override void Start()
    {
        base.Start();
        enemyBulletPool = GameObject.FindGameObjectWithTag(bulletPoolTag);
        muzzleFlashPool = GameObject.FindGameObjectWithTag(muzzleFlashPoolTag).GetComponent<ObjectPool>();
    }
    protected override void firebulletPattern()
    {
        float angleStep = (endAngle - startAngle)/amountOfBullets;
        float angle = startAngle;

        GameObject muzzleFlash = muzzleFlashPool.getObjectFromPool();
        muzzleFlash.transform.position = transform.position;
        muzzleFlash.transform.rotation = transform.rotation;
        muzzleFlash.SetActive(true);

        StartCoroutine(AttackAnimation());

        for (int i = 0; i < amountOfBullets; i++)
        {
            float bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirectionX,bulletDirectionY,0f);
            Vector2 bulletDirection = (bulletMoveVector - transform.position).normalized;

            ObjectPool objectPool = enemyBulletPool.GetComponent<ObjectPool>();
            GameObject enemyBullet = objectPool.getObjectFromPool();


            


            enemyBullet.transform.position = firePoint.position;
            enemyBullet.transform.rotation = firePoint.rotation;
            enemyBullet.SetActive(true);
            enemyBullet.GetComponent<EnemyBullet>().setNewMoveDirection(bulletDirection);

            angle += angleStep;
        }
    }

    IEnumerator AttackAnimation(){
        animator.SetBool("Attack",true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attack",false);
    }
}
