using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int health, energy = 0, eqpPrjctlIndex = 0;
    [SerializeField]
    private int fireCost, waterCost, natureCost, maxHealth = 5;
    [SerializeField]
    private float shockwaveRadius, shockwaveForce;

    private Rigidbody2D rb;
    private Element elementInfo;
    private Element.ElementType element;
    private AudioSource castSound;
    private Animator animator;
    [SerializeField]
    private GameObject[] projectiles, specials = new GameObject[3];

    private void Awake()
    {
        energy = 0;
        eqpPrjctlIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        element = Element.ElementType.Fire;
        elementInfo = Camera.main.GetComponent<Element>();
        castSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gamePaused) Turn();
        if (Input.GetMouseButtonDown(0)) StartCastAnimation();
        if (Input.GetMouseButtonDown(1)) ChangeElement();
        if (Input.GetKeyDown(KeyCode.Space)) CastSpecial();
        if (Input.GetKeyDown(KeyCode.Escape)) GameManager.PauseGame();
    }

    //Changes the direction of the player in order to make him constantly look at the mouse.
    private void Turn()
    {
        Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseInWorld - (Vector2)transform.position).normalized;
        transform.right = direction;
    }

    private void StartCastAnimation()
    {
        animator.SetBool("Casting", true);
    }

    //Shoots a spell with the same element the player currently has.
    private void CastSpell()
    {
        Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shotDirection = (mouseInWorld - (Vector2)transform.GetChild(0).transform.position).normalized;
        transform.GetChild(0).transform.right = shotDirection;
        Projectile projectile = Instantiate(projectiles[eqpPrjctlIndex], transform.GetChild(0).transform.position,
            transform.GetChild(0).transform.rotation).GetComponent<Projectile>();
        projectile.setElement(element);
        PlayCastSound();
        animator.SetBool("Casting", false);
    }

    //Changes the element of the player
    private void ChangeElement()
    {
        switch (element)
        {
            case Element.ElementType.Fire:
                element = Element.ElementType.Water;
                eqpPrjctlIndex++;
                break;
            case Element.ElementType.Water:
                element = Element.ElementType.Nature;
                eqpPrjctlIndex++;
                break;
            case Element.ElementType.Nature:
                element = Element.ElementType.Fire;
                eqpPrjctlIndex = 0;
                break;
        }
        GameObject.Find("Canvas").GetComponent<UiManager>().UpdateElementIcon(element);
    }

    private void CastSpecial()
    {
        if(element == Element.ElementType.Fire && energy >= fireCost)
        {
            energy -= fireCost;
            animator.SetInteger("CastSpecial", 1);
        }
        else if(element == Element.ElementType.Water && energy >= waterCost)
        {
            energy -= waterCost;
            animator.SetInteger("CastSpecial", 2);
        }
        else if(energy >= natureCost)
        {
            energy -= natureCost;
            animator.SetInteger("CastSpecial", 3);
        }
        UiManager.UpdateEnergyText(energy);
    }

    private void FireSpecial()
    {
        PlaySpecialSound();
        Instantiate(specials[0], transform.position, Quaternion.identity);
        animator.SetInteger("CastSpecial", 0);
    }

    private void WaterSpecial()
    {
        Instantiate(specials[1], transform.position, Quaternion.identity);
        animator.SetInteger("CastSpecial", 0);
    }

    private void NatureSpecial()
    {
        Instantiate(specials[2], transform.position, Quaternion.identity);
        animator.SetInteger("CastSpecial", 0);
    }

    private void PlayCastSound()
    {
        castSound.clip = elementInfo.GetCastSoundByElement(element);
        castSound.pitch = Random.Range(0.9f, 1.25f);
        castSound.Play();
    }

    private void PlaySpecialSound()
    {
        castSound.clip = elementInfo.GetSpecialSoundByElement(element);
        castSound.Play();
    }

    private void CastCounterShockwave(bool barrierCall = false)
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            if (Vector2.Distance(enemy.transform.position, transform.position) < shockwaveRadius)
            {
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                enemy.GetComponent<Rigidbody2D>().AddForce(direction * shockwaveForce);
            }
    }

    public void RecieveDamage()
    {
        health--;
        UiManager.UpdateHealthText(health);
        CastCounterShockwave();
        if (health <= 0)
        {
            GameManager.DeactivatePlayerInfo();
            Destroy(gameObject);
        }
    }

    //Increases the 'energy' value by the amout specified in the 'reward' parameter.
    public void RecieveEnergyReward(int reward)
    {
        energy += reward;
        UiManager.UpdateEnergyText(energy);
    }
}
