using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawning : MonoBehaviour
{
    
    [System.Serializable]
    public class Wave{
        public List<int> numberOfEnemy;

    }
    public List<ObjectPool> enemyPools;
    
    public List<Wave> waves;

    public float timeEachSpawning;
    public float timer;
    
    public int currentWaveIndex;
    private bool canUpdateWave = true;
    private bool bossIsAlive = true;
    
    private List<GameObject> currentEnemies;
    
    private bool readyToSpawnTheNextWave = false;

    

    
    void Start(){
        currentEnemies = new List<GameObject>();
    }

    

    void Update(){
        if(bossIsAlive){
            timer += Time.deltaTime;
            if(timer >= timeEachSpawning){
                timer = 0;
                readyToSpawnTheNextWave = true;
            }

            if(readyToSpawnTheNextWave){
                if(currentWaveIndex==0){
                    spawnEnemyWave(0);
                    readyToSpawnTheNextWave = false;
                    if(canUpdateWave) currentWaveIndex++;
                }else if(currentWaveIndex == waves.Count){
                    canUpdateWave = false;
                    int randomWave = Random.Range(0,waves.Count-1);
                    spawnEnemyWave(randomWave);
                    readyToSpawnTheNextWave = false;
                }else{
                    spawnEnemyWave(currentWaveIndex);
                    if(canUpdateWave) currentWaveIndex++;
                    readyToSpawnTheNextWave = false;
                }
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

    public void DestroyAllOfTheEnemy(){
        foreach (GameObject enemy in currentEnemies)
        {
            enemy.GetComponent<Enemy>().Die();
            bossIsAlive = false;
        }
    }
    

    


}
