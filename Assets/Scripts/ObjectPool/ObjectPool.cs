using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private bool differentObjects;

    [Header("Pool of the same objects")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int amount;

    [Header("Pool of the different objects")]
    [SerializeField] private List<GameObject> prefabs;

    private List<GameObject> objectList;

    void Start(){
        if(differentObjects){
            objectList = new List<GameObject>();
            foreach (GameObject obj in prefabs)
            {
                GameObject temp = Instantiate(obj,this.transform);
                temp.SetActive(false);
                objectList.Add(temp);
            }
        }else{
            objectList = new List<GameObject>();
            for (int i = 0; i < amount; i++)
            {
                GameObject temp = Instantiate(prefab,this.transform);
                temp.SetActive(false);
                objectList.Add(temp);
            }
        }
    }

    public GameObject getObjectFromPool(){
        foreach (GameObject obj in objectList)
        {
            if(!obj.activeInHierarchy){
                return obj;
            }
        }
        return null;
    }

    public bool isAllDeactived(int numberOfObjects){
        for (int i = 0; i < numberOfObjects; i++)
        {
            if(objectList[i].activeInHierarchy) return false;
        }
        return true;
    }

    public GameObject GetRandomObjectFromPool(){
        GameObject obj = objectList[Random.Range(0,objectList.Count-1)];
        if(!obj.activeInHierarchy) return obj;
        else return GetRandomObjectFromPool();
    }

    public List<GameObject> getMultipleObjectsList(){
        return objectList;
    }
    
    public void disableObjectFromList(int index){
        objectList[index].SetActive(false);
    }
    
}
