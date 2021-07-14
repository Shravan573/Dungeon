using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator playeranim;
    public float  movespeed = 5f;
    public float jumpforce = 6f;
    private bool IsJumped = false;
    private bool grounded;
    public LayerMask WhatsGround;
    private BoxCollider2D mycollider;
    private bool doublejump;
    private int doublejumpcount;
    private bool playerisdead = false;
    private int coinscore;
    // Start is called before the first frame update
    void Start()
    {
        mycollider = GetComponent<BoxCollider2D>();
        playeranim = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();




        
    }

    // Update is called once per frame
    void Update()
    {
        float moveforward = Input.GetAxis("Horizontal");


        grounded = Physics2D.IsTouchingLayers(mycollider, WhatsGround);
        if (playerisdead == false)
        {
            transform.position = new Vector2(transform.position.x + (moveforward * movespeed * Time.deltaTime), transform.position.y);
        }

        if(grounded && IsJumped == true)
        {
            playeranim.SetBool("PlayerJump", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            IsJumped = false;
        }

        

        //if (!grounded && doublejump == true)
        //{
        //     playeranim.SetBool("PlayerJump", true);
        //    rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        //    doublejump = false;
        //    IsJumped = false;



        //}
        else
        {

            playeranim.SetBool("PlayerJump", false);

        }

        if (moveforward == 0)
        {
            playeranim.SetBool("PlayerRun", false);
            // playeranim.SetBool("PlayerJump", false);
        }
        else
        {
            if (moveforward < 0.0001f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (moveforward > 0.0001f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            playeranim.SetBool("PlayerRun", true);
            // playeranim.SetBool("PlayerJump", false);
            //PLAY
        }




        if (Input.GetKeyDown(KeyCode.W)   && grounded)
        {
            //playeranim.SetBool("PlayerJump", true);
            //playeranim.SetBool("PlayerRun", false);
            //playeranim.SetBool("PlayerIdle", false);
           // playeranim.SetBool("PlayerJump", true);


            IsJumped = true;
            doublejumpcount++;
            if (doublejumpcount == 2)
            {
                doublejump = true;

                doublejumpcount = 0;
            }



        }

       

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Trap")
        {

            playeranim.SetBool("PlayerDeath",true);
            playerisdead = true;

            StartCoroutine(Waitfordeadanim());

           
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lava")
        {

            playeranim.SetBool("PlayerDeath", true);
            playerisdead = true;

            StartCoroutine(Waitfordeadanim());

        }
        if (collision.gameObject.tag == "Door")
        {

           //next level

        }

        if (collision.gameObject.tag == "Coin")
        {
            GameObject coin = collision.gameObject;
            coinscore++;
            Destroy(coin);
            //next level

        }



    }

    IEnumerator Waitfordeadanim()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        // game over set active true
    }


}
