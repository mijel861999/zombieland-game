using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private SpriteRenderer playerRender;

    private const string horizontal = "Horizontal";
    private const string desplazamiento = "Desplazamiento";
    private const string WALKING = "isWalking";

    public float jumpForce;
    public float fireJumpForce;
    public float speed;
    public float forceDes = 20f;
    public float punchForce = 6f;

    private int des = 0;
    private int dire;
    private float couldown = 3.0f;
    public float fireCouldown = 6.0f;

    private bool grounded;
    public bool onFire;


    public static PlayerController sharedInstanceController;
    

    private void Awake()
    {
        if (sharedInstanceController == null) {
            sharedInstanceController = this;
        }   
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        playerRigidbody.velocity = new Vector2(Input.GetAxis(horizontal) * speed, playerRigidbody.velocity.y);

        ComprobarCaminado();

        if (Input.GetButtonDown("Jump")) {

            if (onFire==true) {
                FireJump();
            }

            if (onFire == false) {
                Jump();
            }           
        }

        ComprobarSalto();

        if (onFire == true) {
            fireCouldown = fireCouldown - Time.deltaTime;
            Debug.Log(fireCouldown);
        }

        Displacement();



        if (Input.GetButtonDown("Fire1"))
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(Input.GetAxis(horizontal), 0) * punchForce, ForceMode2D.Impulse);
            playerAnimator.SetTrigger("jab");
        }

        /*
        if (Input.GetKeyDown(KeyCode.F)) {
            playerAnimator.SetTrigger("cross");
        }
        */

    }


    void ComprobarSalto() {
        //Velocidad en y
        if (playerRigidbody.velocity.y > 0)
        {
            playerAnimator.SetInteger("VelocityY", 1);
        }
        else if (playerRigidbody.velocity.y < 0)
        {
            playerAnimator.SetInteger("VelocityY", -1);
        }
        else
        {
            playerAnimator.SetInteger("VelocityY", 0);
        }

        if (grounded == true)
        {
            playerAnimator.SetBool("IsOnTheGround", true);
        }
        else
        {
            playerAnimator.SetBool("IsOnTheGround", false);
        }
    }


    void DoubleDisplacement() {
        if (couldown == 3.0f)
        {

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (dire == -1)
                {
                    des = 0;
                }
                dire = 1;
                des++;
                if (dire == 1 && des == 2)
                {
                    playerRigidbody.AddForce(Vector2.right * forceDes, ForceMode2D.Impulse);
                    des = 0;
                    couldown = 0f;
                }

            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {


                if (dire == 1)
                {
                    des = 0;
                }

                dire = -1;
                des++;
                if (dire == -1 && des == 2)
                {
                    playerRigidbody.AddForce(Vector2.left * forceDes, ForceMode2D.Impulse);
                    des = 0;
                    couldown = 0f;
                }
            }

        }
        else
        {
 
            if (couldown < 3.0f)
            {
                couldown = couldown + Time.deltaTime;
            }
            else if (couldown > 3.0000f)
            {
                couldown = 3.0f;
            }


        }
    }

    void Displacement() {
        if (couldown == 3.0f)
        {

            if (Input.GetButtonDown(desplazamiento)) {
                playerRigidbody.AddForce(new Vector2(Input.GetAxis(horizontal), 0) * forceDes, ForceMode2D.Impulse);
                des = 0;
                couldown = 0f;
            }

        }
        else
        {

            if (couldown < 3.0f)
            {
                couldown = couldown + Time.deltaTime;
            }
            else if (couldown > 3.0000f)
            {
                couldown = 3.0f;
            }


        }

    }


    void Jump() {

        if (grounded == true) {
            playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            grounded = false;
        }
        
    }

    void FireJump() {
        if (fireCouldown <= 6.0f && fireCouldown > 0.0f) {
            playerRigidbody.AddForce(Vector2.up * fireJumpForce, ForceMode2D.Impulse);
        } else if (fireCouldown < 0.0000f) {
            onFire = false;
        }
        
    }

    void ComprobarCaminado() {
        if (Input.GetAxis(horizontal) == 0)
        {
            playerAnimator.SetBool(WALKING, false);
        }
        else if (Input.GetAxis(horizontal) > 0)
        {
            playerRender.flipX = false;
            playerAnimator.SetBool(WALKING, true);
        }
        else {
            playerRender.flipX = true;
            playerAnimator.SetBool(WALKING, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            grounded = true;
        }
    }

}
