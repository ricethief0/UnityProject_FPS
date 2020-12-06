using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public float respawnTime = 10f;
    float lastSpawnTime = 0f;
    public GameObject enemyFactory;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lastSpawnTime)
        {
            GameObject enemy = Instantiate(enemyFactory);
            float xMin = transform.position.x - 2;
            float xMax = transform.position.x + 2;
            float zMin = transform.position.z - 2;
            float zMax = transform.position.z + 2;
            Vector3 spawnPosition = new Vector3(Random.Range(xMin, xMax), 1, Random.Range(zMin, zMax));
            //Debug.Log( " z : "+ spawnPosition.z);
            //Debug.Log( " x : "+ spawnPosition.x);
            enemy.transform.position = spawnPosition;
            lastSpawnTime = Time.time+respawnTime;
        }
    }
}
