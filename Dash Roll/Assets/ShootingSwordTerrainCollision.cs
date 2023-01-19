using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSwordTerrainCollision : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite embeddedSprite;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] ParticleSystem disintegrate, flight;
    [SerializeField] int timer;
    [SerializeField] CapsuleCollider2D[] capsuleCollider;
    [SerializeField] ShootingSword shootingSword;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer == 1)
        {
            trailRenderer.emitting = true;
        }
        if (timer == 200)
        {
            spriteRenderer.enabled = false;
            disintegrate.Play();
        }
        if (timer == 350)
        {
            Destroy(shootingSword.gameObject);
        }
        timer++;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            TestTerrainContact();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            TestTerrainContact();
        }
    }

    void TestTerrainContact()
    {
        if (timer > 0)
        {
            shootingSword.Embed();
            timer = 199;
            spriteRenderer.sprite = embeddedSprite;
            capsuleCollider[0].enabled = false;
            capsuleCollider[1].enabled = false;
            flight.Stop();
            trailRenderer.emitting = false;
        }
    }
}
