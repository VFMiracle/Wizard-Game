using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector2 RandomSpawnPos()
    {
        Vector2 ret = Vector2.zero;
        if (transform.rotation.z == 0)
        {
            float posY = Random.Range(transform.position.y - transform.lossyScale.y / 2,
                transform.position.y + transform.lossyScale.y / 2);
            ret = new Vector2(transform.position.x, posY);
        }
        else
        {
            float posX = Random.Range(transform.position.x - transform.lossyScale.y / 2,
                transform.position.x + transform.lossyScale.y / 2);
            ret = new Vector2(posX, transform.position.y);
        }
        return ret;
    }

    public GameObject Spawn(GameObject enemy)
    {
        return Instantiate(enemy, RandomSpawnPos(), Quaternion.identity);
    }
}
