using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Keyboard Keyboard;
    private Vector3 RotationValue;
    private float Speed;
    public GameManager Game_Manager;
    private bool TouchingGround, TouchingWall;

    private int WallIsToTheLeft = -1;
    private Rigidbody2D Rb;
    public PlayerControls Player_Controls;

    void Awake()
    {
        Player_Controls = new PlayerControls();
    }

    private void OnEnable()
    {
        Player_Controls.Enable();
    }

    private void OnDisable()
    {
        Player_Controls.Disable();
    }
    void Start()
    {
        Keyboard = Keyboard.current;
        RotationValue = new Vector3 (0, 0, 600);
        Speed = 0.1f;
        TouchingGround = false;
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {            
        if ( Game_Manager.GetWinState() == 0)
        {
            if ( !TouchingGround)
            {
                gameObject.transform.position = (new Vector2(transform.position.x, transform.position.y - 0.125f));
            }
                
            if ( Keyboard != null )
            {
                if ( ( Keyboard.aKey.isPressed || Keyboard.leftArrowKey.isPressed ) )
                {
                    if ( TouchingWall && WallIsToTheLeft == 1 )
                    {
                        gameObject.transform.Rotate( RotationValue * Time.deltaTime );
                        gameObject.transform.position = new Vector2 (transform.position.x - Speed, transform.position.y);
                        Rb.MovePosition(new Vector2(transform.position.x, transform.position.y));
                    }
                    else if ( !TouchingWall )
                    {
                        gameObject.transform.Rotate( RotationValue * Time.deltaTime );
                        gameObject.transform.position = new Vector2 (transform.position.x - Speed, transform.position.y);
                        Rb.MovePosition(new Vector2(transform.position.x, transform.position.y));
                    }
                    
                }
                if ( ( Keyboard.dKey.isPressed || Keyboard.rightArrowKey.isPressed ) )
                {
                    if ( TouchingWall && WallIsToTheLeft == 0 )
                    {
                        gameObject.transform.Rotate( -RotationValue * Time.deltaTime );
                        gameObject.transform.position = new Vector2 (transform.position.x + Speed, transform.position.y);
                    }
                    else if ( !TouchingWall )
                    {
                        gameObject.transform.Rotate( -RotationValue * Time.deltaTime );
                        gameObject.transform.position = new Vector2 (transform.position.x + Speed, transform.position.y);
                    }
                    
                }
                if ( TouchingGround && ( Keyboard.wKey.isPressed || Keyboard.upArrowKey.isPressed ) && !TouchingWall )
                {
                    StartCoroutine("Jump");
                }
            }
        }
        
        
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other != null )
        {
            if ( other.CompareTag("Bamboo") )
            {
                Game_Manager.ChangeBamboo(1);
                Game_Manager.PlayEatingSoundFX();
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if ( other != null )
        {
            if ( other.gameObject.CompareTag("Ground") )
            {
                TouchingGround = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ( other != null )
        {
            if ( other.gameObject.CompareTag("Wall"))
            {
                TouchingWall = true;
                if ( other.gameObject.transform.position.x < gameObject.transform.position.x )
                    WallIsToTheLeft = 0;
                else
                    WallIsToTheLeft = 1;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if ( other != null )
        {
            if ( other.gameObject.CompareTag("Ground") )
            {
                TouchingGround = false;
            }
            else if ( other.gameObject.CompareTag("Wall"))
            {
                TouchingWall = false;
                WallIsToTheLeft = -1;
            }
        }
    }

    private IEnumerator Jump()
    {
        for (int i = 0; i < 60 && gameObject.transform.position.y < 18f; i++)
        {
            gameObject.transform.position = new Vector2 (transform.position.x, transform.position.y + Speed);
            yield return null;
        }
        yield return null;
    }
}
