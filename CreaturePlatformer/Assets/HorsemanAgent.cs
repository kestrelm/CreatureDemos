using UnityEngine;
using System.Collections;

public class HorsemanAgent : CreatureGameAgent
{
    const int MOVE_STATE_IDLE = 0;
    const int MOVE_STATE_RIGHT = 1;
    const int MOVE_STATE_LEFT = 2;

    int state = MOVE_STATE_IDLE;
    int changeStateCnter = 0;
    bool facingRight = false;

    public HorsemanAgent()
        : base()
    {
    }

    public override void updateStep()
    {
        if(changeStateCnter <= 0)
        {
            changeStateCnter = Random.Range(30, 120);
            state = Random.Range(0, 3);
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
        var local_size = 0.3f;

        if (state == MOVE_STATE_RIGHT)
        {
            facingRight = true;
            creature_renderer.BlendToAnimation("run");
        }
        else if(state == MOVE_STATE_LEFT)
        {
            facingRight = false;
            creature_renderer.BlendToAnimation("run");
        }
        else if(state == MOVE_STATE_IDLE)
        {
            creature_renderer.BlendToAnimation("default");
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

    private void turnOffOtherObjectCollisions(GameObject[] objectList)
    {
        var creature_renderer = game_controller.creature_renderer;
        var parent_obj = creature_renderer.gameObject;
        var self_collider = parent_obj.GetComponent<Collider2D>();

        foreach (var curObject in objectList)
        {
            var object_collider = curObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(self_collider, object_collider);
        }
    }

    public override void initState()
    {
        base.initState();

        turnOffOtherObjectCollisions(GameObject.FindGameObjectsWithTag("horseman_tag"));
        turnOffOtherObjectCollisions(GameObject.FindGameObjectsWithTag("bat_tag"));
    }
}
