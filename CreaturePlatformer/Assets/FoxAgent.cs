using UnityEngine;
using System.Collections;

public class FoxAgent : CreatureGameAgent
{
    int moveState = 0;
    bool facingRight = true;
    int jump_countdown = 0;

    const int MOVE_STATE_IDLE = 0;
    const int MOVE_STATE_LEFT = -1;
    const int MOVE_STATE_RIGHT = 1;
    const int MOVE_STATE_JUMP = 2;
    const int MOVE_STATE_POST_JUMP = 3;

    public FoxAgent()
        : base()
    {
        moveState = MOVE_STATE_IDLE;
    }

    public override void updateStep()
    {
        base.updateStep();

        bool left_down = Input.GetKeyDown(KeyCode.A);
        bool right_down = Input.GetKeyDown(KeyCode.D);
        bool left_up = Input.GetKeyUp(KeyCode.A);
        bool right_up = Input.GetKeyUp(KeyCode.D);
        bool space_down = Input.GetKeyDown(KeyCode.Space);

        if (left_down)
        {
            facingRight = false;
            moveState = MOVE_STATE_LEFT;
        }
        else if (right_down)
        {
            facingRight = true;
            moveState = MOVE_STATE_RIGHT;
        }
        else if (left_up || right_up)
        {
            moveState = MOVE_STATE_IDLE;
        }
        else if (space_down)
        {
            moveState = MOVE_STATE_JUMP;
        }

        setMovement();
        setAnimation();
    }

    private void setMovement()
    {
        var creature_renderer = game_controller.creature_renderer;
        var parent_obj = creature_renderer.gameObject;
        Rigidbody2D parent_rbd = parent_obj.GetComponent<Rigidbody2D>();
        var speed = 25.0f;
        var curVel = parent_rbd.velocity;

        if (moveState == MOVE_STATE_LEFT)
        {
            parent_rbd.velocity = new Vector3(-speed, curVel.y, 0);
        }
        else if (moveState == MOVE_STATE_RIGHT)
        {
            parent_rbd.velocity = new Vector3(speed, curVel.y, 0);
        }
        else if (moveState == MOVE_STATE_IDLE)
        {
            parent_rbd.velocity = new Vector3(0, curVel.y, 0);
        }
        else if (moveState == MOVE_STATE_JUMP)
        {
            var groundObjs = GameObject.FindGameObjectsWithTag("ground_tag");
            foreach (var ground in groundObjs)
            {
                var ground_collider = ground.GetComponent<Collider2D>();
                if (parent_rbd.IsTouching(ground_collider))
                {
                    var jumpForceX = 30.0f;
                    if (!facingRight)
                    {
                        jumpForceX = -jumpForceX;
                    }
                    parent_rbd.AddForce(new Vector3(jumpForceX, 150, 0), ForceMode2D.Impulse);

                    break;
                }

            }
            moveState = MOVE_STATE_POST_JUMP;
            jump_countdown = 10;
        }
        else if (moveState == MOVE_STATE_POST_JUMP)
        {
            if (jump_countdown <= 0)
            {
                var groundList = GameObject.FindGameObjectsWithTag("ground_tag");
                foreach (var ground in groundList)
                {
                    var ground_collider = ground.GetComponent<Collider2D>();
                    if (parent_rbd.IsTouching(ground_collider))
                    {
                        moveState = MOVE_STATE_IDLE;
                    }
                }
            }
            else
            {
                jump_countdown--;
            }
        }
    }

    private void setAnimation()
    {
        var creature_renderer = game_controller.creature_renderer;
        var local_size = 0.2f;
        creature_renderer.blend_rate = 0.2f;

        if (facingRight)
        {
            creature_renderer.transform.localScale = new Vector3(-local_size, local_size, local_size);
        }
        else
        {
            creature_renderer.transform.localScale = new Vector3(local_size, local_size, local_size);
        }

        if ((moveState == MOVE_STATE_RIGHT) || (moveState == MOVE_STATE_LEFT))
        {
            creature_renderer.BlendToAnimation("run");
        }
        else if ((moveState == MOVE_STATE_JUMP)
            || (moveState == MOVE_STATE_POST_JUMP))
        {
            creature_renderer.BlendToAnimation("jump");
        }
        else if (moveState == MOVE_STATE_IDLE)
        {
            creature_renderer.creature_manager.ResetBlendTime("jump");
            creature_renderer.BlendToAnimation("default");
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