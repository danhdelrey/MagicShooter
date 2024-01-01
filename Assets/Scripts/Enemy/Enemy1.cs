using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    public Animator animator;
    private ObjectPool muzzleFlashPool;
    public string muzzleFlashPoolTag;
    protected override void Start()
    {
        base.Start();
        enemyBulletPool = GameObject.FindGameObjectWithTag(bulletPoolTag);
        muzzleFlashPool = GameObject.FindGameObjectWithTag(muzzleFlashPoolTag).GetComponent<ObjectPool>();
    }
    protected override void firebulletPattern()
    {
        ObjectPool objectPool = enemyBulletPool.GetComponent<ObjectPool>();
        GameObject enemyBullet = objectPool.getObjectFromPool();
        EnemyBullet bul = enemyBullet.GetComponent<EnemyBullet>();

        GameObject muzzleFlash = muzzleFlashPool.getObjectFromPool();
        muzzleFlash.transform.position = transform.position;
        muzzleFlash.transform.rotation = transform.rotation;
        muzzleFlash.SetActive(true);

        StartCoroutine(AttackAnimation());

        enemyBullet.SetActive(true);
        bul.setNewMoveDirection(Vector2.left);
        enemyBullet.transform.position = firePoint.position;
        enemyBullet.transform.rotation = firePoint.rotation;
    }

    IEnumerator AttackAnimation(){
        animator.SetBool("Attack",true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attack",false);
    }
}
