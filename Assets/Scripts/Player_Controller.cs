using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    
    //player movent variables 
    private float move_speed; 
    private float jump_force;
    private bool  jump_state;
    private float move_horizontal; 
    private float move_vertical; 
    
    
    //Player animation variables
    Animator _animator;
    string _currenState;
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_RUN = "Run";
    bool Facing_right = true; 

    
    
    // Start is called before the first frame update
    void Start()
    {
        // getting our players rigidbody component
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        // setting up the animator
        _animator = gameObject.GetComponent<Animator>();
        
        move_speed = 0.5f; 
        jump_force = 3f; 
        jump_state = false;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        move_horizontal = Input.GetAxisRaw("Horizontal");
        move_vertical = Input.GetAxisRaw("Vertical");
        
    }

    private void FixedUpdate()
    {
        if (move_horizontal != 0 )
        {
            rb2D.AddForce(new Vector2(move_horizontal*move_speed,0), ForceMode2D.Impulse);
            ChangeAnimationState(PLAYER_RUN);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
        
        if (move_horizontal > 0 && !Facing_right)
        {
          Flip();  
        } 
        
        if (move_horizontal < 0 && Facing_right)
        { 
          Flip();
        }
            
        if(!jump_state && move_vertical > 0.1f)
        {
            rb2D.AddForce(new Vector2(0,move_vertical*jump_force), ForceMode2D.Impulse);
        }

        
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jump_state = false;
        }
    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jump_state = true;
        }
    }

    // method checking if the current animation is running
    private void ChangeAnimationState(string newState)
    {
        if (newState == _currenState)
        {
            return;
        }
        
        _animator.Play(newState);
        _currenState = newState; 
    }

    
    // method checks if animation has finished playing
    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        Facing_right = !Facing_right; 

    }

}
