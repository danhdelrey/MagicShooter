using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn enemySpawnInstance;
    [System.Serializable]
    public class Wave{
        public List<int> numberOfEnemy;

    }
    public List<ObjectPool> enemyPools;
    
    public List<Wave> waves;
    
    private int currentWaveIndex;
    
    private List<GameObject> currentEnemies;
    private bool bossFight = false;
    private bool isCurrentWaveCleared = false;
    private bool canSpawnWave = true;

    void Awake(){
        enemySpawnInstance = this;
    }
    void Start(){
        currentEnemies = new List<GameObject>();
        spawnEnemyWave(currentWaveIndex);
    }

    void Update(){
        isCurrentWaveCleared = checkWaveCleared();
        if(isCurrentWaveCleared && !bossFight){
            currentWaveIndex++;
            if(currentWaveIndex == waves.Count){
                canSpawnWave = false;
            }
            if(currentWaveIndex < waves.Count){
                if(canSpawnWave) spawnEnemyWave(currentWaveIndex);
                isCurrentWaveCleared = false;
            }
            if(!canSpawnWave && !bossFight){
                Boss.bossInstance.callBoss = true;
                Player.playerInstance.canShoot = false;
                bossFight = true;
            }
        }
    }


    void spawnEnemyWave(int waveIndex){
        for (int i = 0; i < enemyPools.Count; i++)
        {
            for (int j = 0; j < waves[waveIndex].numberOfEnemy[i]; j++)
            {
                GameObject enemy = enemyPools[i].getObjectFromPool();
                enemy.GetComponent<Enemy>().initEnemy();
                currentEnemies.Add(enemy);
            }
        }
    }

    bool checkWaveCleared(){
       foreach (GameObject enemy in currentEnemies)
       {
            if(enemy.activeInHierarchy) return false;
       }
       return true;
    }

    


}
