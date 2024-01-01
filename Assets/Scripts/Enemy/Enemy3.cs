using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
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
        Vector2 bulletMoveVector = Player.playerInstance.getPlayerPosition();
        Vector2 bulletDirection = (bulletMoveVector - (Vector2)transform.position).normalized;

        ObjectPool objectPool = enemyBulletPool.GetComponent<ObjectPool>();
        GameObject enemyBullet = objectPool.getObjectFromPool();

        GameObject muzzleFlash = muzzleFlashPool.getObjectFromPool();
        muzzleFlash.transform.position = transform.position;
        muzzleFlash.transform.rotation = transform.rotation;
        muzzleFlash.SetActive(true);

        StartCoroutine(AttackAnimation());

        enemyBullet.transform.position = firePoint.position;
        enemyBullet.transform.rotation = firePoint.rotation;
        enemyBullet.SetActive(true);
        enemyBullet.GetComponent<EnemyBullet>().setNewMoveDirection(bulletDirection);
    }

    IEnumerator AttackAnimation(){
        animator.SetBool("Attack",true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attack",false);
    }
}
