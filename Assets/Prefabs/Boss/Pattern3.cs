using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern3 : BossBulletPattern
{
    private float angle;
    public float spinningAngleSpeed;

    public override void Fire()
    {
        GameObject muzzleFlash = muzzleFlashPool.getObjectFromPool();
        muzzleFlash.transform.position = transform.position;
        muzzleFlash.transform.rotation = transform.rotation;
        muzzleFlash.SetActive(true);
        
        for (int i = 0; i <= 1; i++)
        {
            float bulletDirectionX = transform.position.x + Mathf.Sin(((angle + 180f * i) * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos(((angle + 180f * i) * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirectionX,bulletDirectionY,0f);
            Vector2 bulletDirection = (bulletMoveVector - transform.position).normalized;

            GameObject bullet = bulletPool.getObjectFromPool();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);

            bullet.GetComponent<EnemyBullet>().setNewMoveDirection(bulletDirection);
        }

        angle += spinningAngleSpeed;

        if(angle >= 360){
            angle = 0;
        }
    }
}
