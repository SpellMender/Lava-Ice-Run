using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool jumping = false;
    private bool falling = false;
    public float jumpPower = 1.0f;
    public float minJump = 3.0f;
    private float neutral;
    private float neutralOffset;
   

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        neutral = transform.position.x;
        neutralOffset = neutral;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.gameOn)
            if (!jumping) {falling = false;}
            else if (rb2d.velocity.y <= 0) {falling = true;}
            
            if(Input.GetMouseButtonDown(0) && !jumping)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
            }
            if(!Input.GetMouseButton(0) && !falling && transform.position.y > minJump)
            {
                rb2d.velocity = Vector2.zero;
                falling = true;
            }
            if (transform.position.y <= -3)
            {
                GameController.instance.PlayerDied();
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb2d.constraints &= ~RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        neutralOffset = transform.position.x;
        if (transform.position.x == neutral)
        {
            jumping = false;
        }
        else jumping = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (transform.position.x == neutral)
        {
            jumping = false;
        }
        else jumping = true;
        if(!GameController.instance.gameOn)
        {
            rb2d.constraints &= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        jumping = true;
    }
}
