using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pigletAgent : MonoBehaviour
{
    public int lifetime = 20;
    private int changeStateCnt = 0;
    private bool allowStateChange = false;
    // Start is called before the first frame update
    void Start()
    {
        ResetCharState();
    }

    public void ResetCharState()
    {
        var rbd = gameObject.GetComponent<Rigidbody2D>();        
        rbd.velocity = new Vector2(Random.Range(30.0f, 60.0f), Random.Range(5.0f, 10.0f));
        rbd.rotation = 0;
        rbd.mass = Random.Range(0.6f, 2.0f);
        rbd.gravityScale = 15.0f;
        rbd.sharedMaterial.bounciness = Random.Range(0.5f, 0.8f);
        transform.localScale = new Vector2(1,1) * Random.Range(0.5f, 1.0f);

        var renderer = gameObject.GetComponentInChildren<CreaturePackRenderer>();
        renderer.pack_player.blendToAnimation("run", 0.1f);
        renderer.local_time_scale = Random.Range(60, 80);
        renderer.pack_player.setRunTime(Random.Range(0, 280), "run");
        allowStateChange = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!allowStateChange)
        {
            if (collision.gameObject.name == "Floor2")
            {
                allowStateChange = true;
            }
        }

        if (allowStateChange)
        {
            var rbd = gameObject.GetComponent<Rigidbody2D>();
            rbd.angularVelocity += Random.Range(-570.0f, -250.0f);
            rbd.velocity += new Vector2(0, Random.Range(10, 20));
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifetime--;
        if(lifetime == 0)
        {
            gameObject.SetActive(false);
        }

        var rbd = gameObject.GetComponent<Rigidbody2D>();
        if ((changeStateCnt <= 0) && allowStateChange)
        {
            var renderer = gameObject.GetComponentInChildren<CreaturePackRenderer>();
            if(Mathf.Abs(rbd.rotation - 0.0f) < 20.0f)
            {
                renderer.pack_player.blendToAnimation("run", 0.1f);
            }
            else
            {
                int rIdx = Random.Range(0, 100);
                rIdx = 60;
                if (rIdx < 50)
                {
                    renderer.pack_player.blendToAnimation("default", 0.1f);
                }
                else
                {
                    renderer.pack_player.blendToAnimation("excited", 0.1f);
                }
            }

            changeStateCnt = 30;
        }
        changeStateCnt--;
    }
}
