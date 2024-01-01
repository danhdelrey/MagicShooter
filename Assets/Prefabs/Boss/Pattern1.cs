using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern1 : BossBulletPattern
{
   public float startAngle;
   public float endAngle;
   public int amountOfBullets;


    public override void Fire()
    {
        float angleStep = (endAngle - startAngle)/amountOfBullets;
        float angle = startAngle;

        GameObject muzzleFlash = muzzleFlashPool.getObjectFromPool();
        muzzleFlash.transform.position = transform.position;
        muzzleFlash.transform.rotation = transform.rotation;
        muzzleFlash.SetActive(true);


        for (int i = 0; i < amountOfBullets; i++)
        {
            float bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirectionX,bulletDirectionY,0f);
            Vector2 bulletDirection = (bulletMoveVector - transform.position).normalized;

            GameObject bullet = bulletPool.getObjectFromPool();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);

            bullet.GetComponent<EnemyBullet>().setNewMoveDirection(bulletDirection);

            
            angle += angleStep;
        }
    }


}
