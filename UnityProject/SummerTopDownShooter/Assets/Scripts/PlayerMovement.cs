using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MaxMovespeed = 5.0f;
    public float AccelerationSpeed = 30.0f;

    Rigidbody2D rb;    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = Vector2.zero;

        //Read for key input to change our direction
        if(Input.GetKey(KeyCode.A))
        {
            dir += Vector2.left;
        }
        if(Input.GetKey(KeyCode.D))
        {
            dir += Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += Vector2.down;
        }

        //Store our current velocity so its easier to mess with
        Vector2 vel = rb.velocity;

        //If the player is trying to stand still, we need to decelerate towards the zero vector
        if (dir == Vector2.zero)
        {
            //Calculate deceleration based off of acceleration speed in the negative direction of our velocity
            Vector2 deceleration = -vel.normalized * AccelerationSpeed * Time.deltaTime;

            //If applying this deceleration would cause too big of a change, just hardset to the zero vector
            if (deceleration.magnitude > vel.magnitude)
            {
                vel = Vector2.zero;
            }
            else
            {
                vel += deceleration;
            }
        }
        else
        {
            //Might work idk (attempt to account for unwanted movement direction)
            Vector2 change = dir.normalized - vel.normalized;
            dir.Normalize();
            dir += change;
            dir.Normalize();

            //Accelerate in the new direction the player wants to move
            vel += dir * AccelerationSpeed * Time.deltaTime;
            //If we are over our max movespeed, keep going in same direction but change speed
            if (vel.magnitude > MaxMovespeed)
            {
                vel = vel.normalized * MaxMovespeed;
            }
        }
        
        

        rb.velocity = vel;        
    }
}
