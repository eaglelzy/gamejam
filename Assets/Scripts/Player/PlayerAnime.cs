using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnime : MonoBehaviour
{
    private Animator animator;
    private PlayerMove player;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMove>();
    }

    private void CheckState()
    {
        if (player.LastOnGroundTime > 0 && Mathf.Abs(player.RB.velocity.x) > 0.01f)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }
    
    void Update()
    {
        CheckState();
    }
}
