using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : GameSys
{
    public Rigidbody2D Rigidbody { get; private set; }
    public SpriteRenderer sprite;
    public bool Moving { get; set; } = false;
    public bool Used { get; set; } = false;

    PlatformParent Parent;
    Coroutine movingCor;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        Parent = GetComponentInParent<PlatformParent>();
    }

    private void OnEnable()
    {
        movingCor = StartCoroutine(CycleMoving());
    }

    IEnumerator CycleMoving()
    {
        while (Moving)
        {
            Rigidbody.velocity = transform.right;
            yield return new WaitForSeconds(2f);
            Rigidbody.velocity = -transform.right;
            yield return new WaitForSeconds(2f);
        }

        Rigidbody.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Edge") //When platform is behind the screen rect
        {
            transform.up = Vector2.up;
            gameObject.SetActive(false);
            Parent.SpawnPlatform();
            
        }
        else if (other.tag == "Player")
        {
            StopAllCoroutines();
            Rigidbody.velocity = Vector2.zero;
            //Used = true;
            sprite.color = Color.gray;
            Moving = false;
        }
    }

    public override void GameOver()
    {
        StopAllCoroutines();
        Rigidbody.velocity = Vector2.zero;
    }
}
