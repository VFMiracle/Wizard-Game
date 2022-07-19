using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool inAttackPosition = false, isAlive = true;
    [SerializeField]
    private float minDistToHit;
    [SerializeField]
    private int scoreValue, energyReward;
    private int health = 3, speedBuffState = 0;
    private float speed = 2.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private AudioSource hitPlayerSound;
    private Animator animator;
    [SerializeField]
    private Element.ElementType element;

    private void Awake()
    {
        inAttackPosition = false;
        isAlive = true;
        health = 3;
        speedBuffState = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hitPlayerSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
    }

    private void FixedUpdate()
    {
        if (GameManager.isPlayerAlive && isAlive) Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Element.ElementType prjctlElement = collision.gameObject.GetComponent<Projectile>().getElement();
            if (prjctlElement != element)
            {
                if (Element.IsWeakness(element, prjctlElement))
                    StartCoroutine(Death(prjctlElement));
            }
            else
            {
                health--;
                if (health <= 0)
                    StartCoroutine(Death(prjctlElement));
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Attack", true);
            inAttackPosition = true;
        }
        else if (collision.gameObject.name == "Barrier(Clone)")
            StartCoroutine(Death(Element.ElementType.Nature, true));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) inAttackPosition = false;
    }

    //Changes the direction the enemy is facing.
    private void Turn()
    {
        if (GameManager.isPlayerAlive && isAlive)
        {
            Vector2 direction = (GameManager.getPlayerPosition() - (Vector2)transform.position).normalized;
            transform.up = direction;
        }
        else rb.velocity = Vector2.zero;
    }

    //Changes the 'velocity' of the enemy.
    private void Move()
    {
        if (!inAttackPosition) rb.velocity = transform.up * speed;
        else rb.velocity = new Vector2();
    }

    private void DamagePlayer()
    {
        if (GameManager.isPlayerAlive)
        {
            if (Vector2.Distance(transform.position, GameManager.getPlayerPosition()) <= minDistToHit)
            {
                hitPlayerSound.pitch = Random.Range(0.8f, 1.2f);
                hitPlayerSound.Play();
                if (GameManager.playerRef != null) GameManager.playerRef.RecieveDamage();
            }
        }
        animator.SetBool("Attack", false);
    }

    //Modifies values in other classes before destroying the enemy.
    public IEnumerator Death(Element.ElementType killedByElement, bool killedBySpecial = false)
    {
        if (!killedBySpecial) GameManager.playerRef.RecieveEnergyReward(energyReward);
        GameManager.IncreaseScore(scoreValue);
        Camera.main.GetComponent<GameManager>().IncreaseDifficulty();
        GameObject deathParticle = Camera.main.GetComponent<Element>().SpawnDeathParticle(killedByElement, 
            transform.position);
        GetComponent<Collider2D>().enabled = false;
        isAlive = false;
        yield return new WaitForSeconds(deathParticle.GetComponent<ParticleSystem>().main.duration/2);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(deathParticle.GetComponent<ParticleSystem>().main.duration/2);
        Destroy(deathParticle);
        Destroy(gameObject);
    }

    public void ApplySpeedEffect(float multiplier, bool increase)
    {
        if (increase && speedBuffState != 1)
        {
            speed *= multiplier;
            speedBuffState = 1;
        }
        else if (speedBuffState != -1)
        {
            speed /= multiplier;
            speedBuffState = -1;
        }
    }

    //Chnges the value of the 'element' variable.
    public void setElement(Element.ElementType elmt) //NÃO UTILIZADO NO MOMENTO.
    {
        element = elmt;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
}
