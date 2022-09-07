using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnDelay;
    public GameObject[] enemies;
    float widgh;
    float timer;
    void Start()
    {
        widgh = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= spawnDelay)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)],
                new Vector3(Random.Range(transform.position.x - (widgh / 2), transform.position.x + (widgh / 2)), transform.position.y, transform.position.z), 
                Quaternion.identity, 
                gameObject.transform.GetChild(0));

            timer = 0;
        }
        timer += Time.deltaTime;
    }
}
