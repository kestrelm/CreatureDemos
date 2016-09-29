using UnityEngine;
using System.Collections;

public class HummerScript : MonoBehaviour {
    CreaturePackRenderer pack_renderer;
    Vector3 target_pos;
    int state_cnter, state_duration;
    int mode;
    float move_ratio;

    // Use this for initialization
    void Start () {
        pack_renderer = GetComponentInParent<CreaturePackRenderer>();
        findNewPos();

        pack_renderer.local_time_scale = (int)Random.Range(55, 70);
    }

    void findNewPos()
    {
        state_cnter = 0;
        // minX: -42.0, maxX: 42.0
        // minY: -18.0, maxY: 30.0
        state_duration = (int)Random.Range(30, 200);
        move_ratio = Random.Range(0.035f,0.015f);

        if (Random.Range(0, 10.0f) > 5.0f)
        {
            mode = 1;
        }
        else
        {
            mode = 0;
        }

        if(mode == 1)
        {
            target_pos.x = Random.Range(-42.0f, 42.0f);
            target_pos.y = Random.Range(-18.0f, 30.0f);
        }
        else
        {
            target_pos = transform.parent.transform.position;
        }

        var cur_pos = transform.parent.transform.position;
        var diff_pos = target_pos - cur_pos;
        var cur_size = 0.7f;
        var flip_cutoff = 5.0f;
        if (diff_pos.x > 0)
        {
            if (Mathf.Abs(diff_pos.x) > flip_cutoff)
            {
                transform.parent.transform.localScale = new Vector3(-cur_size, cur_size, cur_size);
            }
        }
        else
        {
            if (Mathf.Abs(diff_pos.x) > flip_cutoff)
            {
                transform.parent.transform.localScale = new Vector3(cur_size, cur_size, cur_size);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        state_cnter++;

        // Determine position
        if(state_cnter > state_duration)
        {
            findNewPos();
        }

        // Move bird
        var cur_pos = transform.parent.transform.position;
        var diff_pos = target_pos - cur_pos;

        var new_pos = move_ratio * diff_pos + cur_pos;
        new_pos.z = cur_pos.z;

        transform.parent.position = new_pos;

        // Set Animation clip to play
        var diff_length = diff_pos.magnitude;
        if(diff_length <= 5.0f)
        {
            pack_renderer.pack_player.blendToAnimation("hover", 0.1f);
        }
        else
        {
            pack_renderer.pack_player.blendToAnimation("fly", 0.1f);
        }
    }
}
