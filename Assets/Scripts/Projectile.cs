using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;
    private Element.ElementType element;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        Move();
        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spawner")) Destroy(gameObject);
    }

    //Determines the direction and the speed of the projectile movement.
    private void Move()
    {
        rb.velocity = transform.right * speed;
    }

    //Changes the color of the gameobject's sprite based on the value of the 'element' variable.
    private void ChangeColor()
    {
        switch (element)
        {
            case Element.ElementType.Fire:
                sprite.color = Element.getElementColor("Fire");
                break;
            case Element.ElementType.Water:
                sprite.color = Element.getElementColor("Water");
                break;
            case Element.ElementType.Nature:
                sprite.color = Element.getElementColor("Nature");
                break;
        }
    }

    //Changes the value of the 'element' variable.
    public void setElement(Element.ElementType elm)
    {
        element = elm;
    }

    public Element.ElementType getElement()
    {
        return element;
    }
}
