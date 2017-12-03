using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController_m : MonoBehaviour {
	
	public float maxSpeed = 6f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public float verticalSpeed = 20;
	[HideInInspector]
	public bool lookingRight = true;
	bool doubleJump = false;
	public GameObject Boost;
//	private Animator cloudanim;
	public GameObject Cloud;
	private Rigidbody2D rb2d;
	private Animator anim;
	private bool isGrounded = false;

    public bool jumping = false;
    public float hor = 0f;
    public Vector3 target_position;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		Cloud = GameObject.Find("Cloud");
	}
	
	void OnCollisionEnter2D(Collision2D collision2D) {
        if(collision2D.transform.tag == "Enemy") {
            GameInformation.dead = true;
            Debug.Log("collision with: " + collision2D.transform.name);
        }

        if (collision2D.relativeVelocity.magnitude > 20){
			Boost = Instantiate(Resources.Load("Prefabs/Cloud"), transform.position, transform.rotation) as GameObject;
		}
	}
	
	void Update () {
        foreach(KeyValuePair<string, GameObject> pair in SceneInformation.instance.variables) {
            Debug.Log("key: " + pair.Key + ", value: " + pair.Value);
        }

        foreach(GameObject go in SceneInformation.instance.obstacles) {
            if(go.transform.position.x - transform.position.x <= 4f && transform.position.x < go.transform.position.x) {
                SceneInformation.instance.variables["wall"] = go;
                break;
            }
            else {
                SceneInformation.instance.variables["wall"] = null;
            }
        }

        foreach(GameObject go in SceneInformation.instance.enemies) {
            if(go.transform.position.x - transform.position.x <= 4f && transform.position.x < go.transform.position.x) {
                SceneInformation.instance.variables["red cloud"] = go;
                break;
            }
            else {
                SceneInformation.instance.variables["red cloud"] = null;
            }
        }

        foreach(GameObject go in SceneInformation.instance.collectables) {
            if(go.transform.position.x - transform.position.x <= 4f && transform.position.x < go.transform.position.x) {
                SceneInformation.instance.variables["white cloud"] = go;
                break;
            }
            else {
                SceneInformation.instance.variables["white cloud"] = null;
            }
        }

        if (jumping && (isGrounded || !doubleJump))
		{
			rb2d.AddForce(new Vector2(0,jumpForce));

			if (!doubleJump && !isGrounded)
			{
				doubleJump = true;
				Boost = Instantiate(Resources.Load("Prefabs/Cloud"), transform.position, transform.rotation) as GameObject;
			//	cloudanim.Play("cloud");		
			}

            jumping = false;
		}

		if (CrossPlatformInputManager.GetButtonDown("Boost") && !isGrounded)
		{
			rb2d.AddForce(new Vector2(0,-jumpForce));
			Boost = Instantiate(Resources.Load("Prefabs/Cloud"), transform.position, transform.rotation) as GameObject;
		}

	}

	void FixedUpdate()
	{
		if (isGrounded) 
			doubleJump = false;

        if(transform.position.x < target_position.x) {
            anim.SetFloat("Speed", Mathf.Abs(hor));
            rb2d.velocity = new Vector2(hor * maxSpeed, rb2d.velocity.y);
        }
        else {
            hor = 0;
            anim.SetFloat("Speed", Mathf.Abs(hor));
            rb2d.velocity = new Vector2(hor * maxSpeed, rb2d.velocity.y);
        }

        isGrounded = Physics2D.OverlapCircle (groundCheck.position, 0.15F, whatIsGround);

		anim.SetBool ("IsGrounded", isGrounded);
		 
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
	}
}
