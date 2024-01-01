using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2 : BossBulletPattern
{

   
    public override void Fire()
    {
        Vector2 bulletMoveVector = Player.playerInstance.getPlayerPosition();
        Vector2 bulletDirection = (bulletMoveVector - (Vector2)transform.position).normalized;

        GameObject bullet = bulletPool.getObjectFromPool();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<EnemyBullet>().setNewMoveDirection(bulletDirection);

        GameObject muzzleFlash = muzzleFlashPool.getObjectFromPool();
        muzzleFlash.transform.position = transform.position;
        muzzleFlash.transform.rotation = transform.rotation;
        muzzleFlash.SetActive(true);

        
    }


}
