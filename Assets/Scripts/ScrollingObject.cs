using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rb2d;
    //public float scrollSpeed = 1.0f;

    private Vector2 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        //lastPosition = (Vector2) transform.position;
        //rb2d.velocity = new Vector2(-scrollSpeed, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.velocity = Vector2.zero;
        if (GameController.instance.gameOn)
        {
        rb2d.velocity = new Vector2(GameController.instance.scrollSpeed, 0);
        }
        if (!GameController.instance.gameOn)
        {
            rb2d.velocity = Vector2.zero;
        }
    }
}
