using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField]
    private int health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            health--;
            if(health <= 0)
                Destroy(gameObject);
        }
    }
}
