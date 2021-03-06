﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float whiteoutDuration = 0.15f;
    public float dropMoneyDamageThresh = 0.2f;
    public bool dropMoneyOnDamage = false;
    public float maxHealth;
    [SerializeField]
    private float currentHealth;
    public GameObject money;
    public Image HealthBar;
    private GameObject explosion;
    private bool explodeOnDeath = false;
    EnemyManager Em;
    private float lastCheckmark = 1.0f;
    private bool isWhite = false;
    // Start is called before the first frame update
    void Awake()
    {
        Em = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EnemyManager>();
        Em.addEnemy();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount = currentHealth / maxHealth;
    }

    public void takeDamage(float damageToTake)
    {
        currentHealth -= damageToTake;
        StartCoroutine(FlickerSprite());
        if(dropMoneyOnDamage)
        {
            if(lastCheckmark - (currentHealth / maxHealth) >= dropMoneyDamageThresh)
            {
                Vector3 randomPoint = Random.insideUnitCircle * 1.5f;
                Instantiate(money, transform.position + randomPoint, transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360)));
                lastCheckmark = currentHealth / maxHealth;
            }
        }
        if (currentHealth <= 0)
        {
            
            DIE();
        }
    }

    public void ToggleExplodeOnDeath(GameObject explosion)
    {
        explodeOnDeath = !explodeOnDeath;
        this.explosion = explosion;
    }

    void DIE()
    {
        GetComponent<EnemyListener>().RemoveListener();
        if(explodeOnDeath)
        {
            GameObject boom = Instantiate(explosion);
            boom.transform.position = transform.position;
            boom.GetComponent<Explosion>().Explode();
        }
        
        Instantiate(money, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360)));
        Destroy(gameObject);
    }


    private void OnDestroy()
    {

        Em.minusEnemy();

    }

    private IEnumerator FlickerSprite()
    {
        if(isWhite)
        {
            yield break;
        }

        isWhite = true;
        GetComponent<SpriteRenderer>().color = new Color32(0xFF, 0x73, 0x73, 0xFF);
        yield return new WaitForSeconds(whiteoutDuration);
        GetComponent<SpriteRenderer>().color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        isWhite = false;
    }
}
