using UnityEngine;
using System.Collections;

public class BatAgent : CreatureGameAgent
{
    const int MOVE_STATE_RIGHT = 0;
    const int MOVE_STATE_LEFT = 1;
    int state = MOVE_STATE_LEFT;
    int changeStateCnter = 0;
    bool facingRight = false;

    public BatAgent()
        : base()
    {
    }


    public override void updateStep()
    {
        if (changeStateCnter <= 0)
        {
            changeStateCnter = Random.Range(30, 120);
            state = Random.Range(0, 2);
        }

        setAnimation();
        setMovement();

        changeStateCnter--;
    }

    private void setMovement()
    {
        var creature_renderer = game_controller.creature_renderer;
        var parent_obj = creature_renderer.gameObject;
        Rigidbody2D parent_rbd = parent_obj.GetComponent<Rigidbody2D>();
        var speed = 15.0f;
        var curVel = parent_rbd.velocity;


        if (state == MOVE_STATE_RIGHT)
        {
            parent_rbd.velocity = new Vector3(speed, curVel.y, 0);
        }
        else if (state == MOVE_STATE_LEFT)
        {
            parent_rbd.velocity = new Vector3(-speed, curVel.y, 0);
        }
    }

    private void setAnimation()
    {
        var creature_renderer = game_controller.creature_renderer;
        var local_size = 0.15f;

        if (state == MOVE_STATE_RIGHT)
        {
            facingRight = true;
        }
        else if (state == MOVE_STATE_LEFT)
        {
            facingRight = false;
        }

        if (facingRight)
        {
            creature_renderer.transform.localScale = new Vector3(-local_size, local_size, local_size);
        }
        else
        {
            creature_renderer.transform.localScale = new Vector3(local_size, local_size, local_size);
        }
    }


    public override void initState()
    {
        base.initState();

        var creature_renderer = game_controller.creature_renderer;
        var parent_obj = creature_renderer.gameObject;
        var self_collider = parent_obj.GetComponent<Collider2D>();
        self_collider.enabled = false;
    }
}
