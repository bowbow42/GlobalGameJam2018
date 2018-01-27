using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public GameObject lastSpawn;

    // Is checked in GameManager to Reset the whole level
    public int life = 5;
    private int startLife;
    private float invincibleSince = 0f;
    private bool invincible;

    private void Start()
    {
        startLife = life;
    }

    void Update () {
	    if(invincible)
        {
            invincibleSince += Time.deltaTime;
            if (invincibleSince > 1)
            {
                invincible = false;
                invincibleSince = 0f;
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            DamagePlayer();
        if (collision.gameObject.name == "Pit")
            life -= startLife;
    }

    public void DamagePlayer()
    {
        if (!invincible)
        {
            invincible = true;
            life--;
        }
    }

    // Is only called in GameManager to Reset the whole level
    public void Respawn()
    {
        transform.position = lastSpawn.transform.position;
        life = startLife;
    }
}
