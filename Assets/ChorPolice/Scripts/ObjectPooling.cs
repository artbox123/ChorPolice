using UnityEngine;
using System.Collections.Generic;

namespace ArtboxGames
{
    public class ObjectPooling : MonoBehaviour
    {
        public static ObjectPooling instance;

        public GameObject[] enemies;  //ref to enemies prefab
        public GameObject[] clouds;  //ref to cloud prefab
        public GameObject point;  //ref to point prefab
        public GameObject speedPickUp;  //ref to speedPickUp and shieldPickUp prefab
        public GameObject shieldPickUp;  //ref to speedPickUp and shieldPickUp prefab
        public GameObject pointSign;  //ref to pointSign prefab
        public GameObject enemySign;  //ref to carSign prefab
        public GameObject pickUpSign;  //ref to pickUpSign prefab
        public GameObject explosion;  //ref to carSign prefab
        public int count = 3; //total clones of each object to be spawned

        List<GameObject> spawnedEnemies = new List<GameObject>();    //list to add them
        List<GameObject> cloudsList = new List<GameObject>();    //list to add them
        List<GameObject> pointsList = new List<GameObject>();    //list to add them
        List<GameObject> speedPickUpList = new List<GameObject>();    //list to add them
        List<GameObject> shieldPickUpList = new List<GameObject>();    //list to add them
        List<GameObject> pointSignList = new List<GameObject>();    //list to add them
        List<GameObject> enemySignList = new List<GameObject>();    //list to add them
        List<GameObject> pickUpSignList = new List<GameObject>();    //list to add them
        List<GameObject> explosionList = new List<GameObject>();    //list to add them

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            //missiles
            for (int i = 0; i < count; i++)
            {
                //each block is spawn in the array 2 times
                for (int j = 0; j < enemies.Length; j++)
                {
                    GameObject obj = Instantiate(enemies[j]);
                    obj.transform.parent = gameObject.transform;
                    obj.SetActive(false);
                    spawnedEnemies.Add(obj);
                }
            }
            //clouds
            for (int i = 0; i < count; i++)
            {
                //each block is spawn in the array 2 times
                for (int j = 0; j < clouds.Length; j++)
                {
                    GameObject obj = Instantiate(clouds[j]);
                    obj.transform.parent = gameObject.transform;
                    obj.SetActive(false);
                    cloudsList.Add(obj);
                }
            }
            //speedPickUps
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(speedPickUp);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                speedPickUpList.Add(obj);
            }

            //shieldPickUps
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(shieldPickUp);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                shieldPickUpList.Add(obj);
            }

            //pointSign
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(pointSign);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                pointSignList.Add(obj);
            }
            //carSign
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(enemySign);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                enemySignList.Add(obj);
            }
            //PickUpSign
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(pickUpSign);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                pickUpSignList.Add(obj);
            }
            //points
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(point);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                pointsList.Add(obj);
            }
            //explosion
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(explosion);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                explosionList.Add(obj);
            }
        }

        //method which is used to call from other scripts to get the clone object
        //enemyCar
        public GameObject GetSpawnedEnemies()
        {
            //this statement checks for the in active object in the hierarcy and retun it
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (!spawnedEnemies[i].activeInHierarchy)
                {
                    return spawnedEnemies[i];
                }
            }
            //if the object are less then more are spawned
            GameObject obj = new GameObject();

            for (int j = 0; j < enemies.Length; j++)
            {
                obj = Instantiate(enemies[j]);
                obj.transform.parent = gameObject.transform;
            }
            obj.SetActive(false);
            spawnedEnemies.Add(obj);
            return obj;

        }

        //cloud
        public GameObject GetClouds()
        {
            //this statement checks for the in active object in the hierarcy and retun it
            for (int i = 0; i < cloudsList.Count; i++)
            {
                if (!cloudsList[i].activeInHierarchy)
                {
                    return cloudsList[i];
                }
            }
            //if the object are less then more are spawned
            GameObject obj = new GameObject();

            for (int j = 0; j < clouds.Length; j++)
            {
                obj = Instantiate(clouds[j]);
                obj.transform.parent = gameObject.transform;
            }
            obj.SetActive(false);
            cloudsList.Add(obj);
            return obj;

        }

        //speed pickup
        public GameObject GetSpeedPickUps()
        {
            for (int i = 0; i < speedPickUpList.Count; i++)
            {
                if (!speedPickUpList[i].activeInHierarchy)
                {
                    return speedPickUpList[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(speedPickUp);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            speedPickUpList.Add(obj);
            return obj;
        }

        //shield pickup
        public GameObject GetShieldPickUps()
        {
            for (int i = 0; i < shieldPickUpList.Count; i++)
            {
                if (!shieldPickUpList[i].activeInHierarchy)
                {
                    return shieldPickUpList[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(shieldPickUp);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            shieldPickUpList.Add(obj);
            return obj;
        }

        //point sign
        public GameObject GetPointSign()
        {
            for (int i = 0; i < pointSignList.Count; i++)
            {
                if (!pointSignList[i].activeInHierarchy)
                {
                    return pointSignList[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(pointSign);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            pointSignList.Add(obj);
            return obj;
        }

        //carSign
        public GameObject GetEnemySign()
        {
            for (int i = 0; i < enemySignList.Count; i++)
            {
                if (!enemySignList[i].activeInHierarchy)
                {
                    return enemySignList[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(enemySign);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            enemySignList.Add(obj);
            return obj;
        }

        //speedPickUp sign
        public GameObject GetPickUpSign()
        {
            for (int i = 0; i < pickUpSignList.Count; i++)
            {
                if (!pickUpSignList[i].activeInHierarchy)
                {
                    return pickUpSignList[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(pickUpSign);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            pickUpSignList.Add(obj);
            return obj;
        }

        //POints
        public GameObject GetPoints()
        {
            for (int i = 0; i < pointsList.Count; i++)
            {
                if (!pointsList[i].activeInHierarchy)
                {
                    return pointsList[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(point);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            pointsList.Add(obj);
            return obj;
        }

        //Explosion
        public GameObject GetExplosion()
        {
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].activeInHierarchy)
                {
                    return explosionList[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(explosion);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            explosionList.Add(obj);
            return obj;
        }
    }
}