/**** 
 * Created by: Bob Baloney
 * Date Created: April 20, 2022
 * 
 * Last Edited by: Qadeem Qureshi
 * Last Edited: April 28, 2022
 * 
 * Description: Controls the ball and sets up the intial game behaviors. 
****/

/*** Using Namespaces ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject paddle; //ref to our paddle
    public int number_of_balls; // how many lives we have
    public int current_balls; // store our max live
    public int score; // our current score
    public Text ball_lives; // visualise our lives
    public Text ball_score; // visualise our score

    [Header("Ball Settings")]
    public bool isInPlay; // if we need to move the ball now
    public Vector3 initialForce; // some initial force to apply
    public float speed; // constant speed while moving
    private Rigidbody rb; // ref rigid body
    private AudioSource audioSource; // ref to sound source

 


    //Awake is called when the game loads (before Start).  Awake only once during the lifetime of the script instance.
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Set our rigid body ref
        audioSource = GetComponent<AudioSource>(); // Set our audio ref
    }//end Awake()


        // Start is called before the first frame update
    void Start()
    {
        current_balls = number_of_balls;
        SetStartingPos(); //set the starting position
    }//end Start()


    // Update is called once per frame
    void Update()
    {
        ball_lives.text = "Balls: " + current_balls; // Update ui lives
        ball_score.text = "Score: " + score; // Update UI score

        // Set txt of score and lives
        if(!isInPlay) // We are not playing yet
        {
            SetStartingPos(); // Set position to paddle
            if (Input.GetKeyUp(KeyCode.Space)) // We want to playu
            { 
                isInPlay = true;
                Move(); // Apply inital force
            }
        }
    }//end Update()


    private void LateUpdate()
    {
        rb.velocity = speed * rb.velocity.normalized; // Keep constant velocity
    }//end LateUpdate()


    void SetStartingPos()
    {
        isInPlay = false;//ball is not in play
        rb.velocity = Vector3.zero;//set velocity to keep ball stationary

        Vector3 pos = new Vector3();
        pos.x = paddle.transform.position.x; //x position of paddel
        pos.y = paddle.transform.position.y + paddle.transform.localScale.y + .25f; //Y position of paddle plus it's height, plus padding for visualization

        transform.position = pos;//set starting position of the ball 
    }//end SetStartingPos()

    void Move()
    {
        rb.AddForce(initialForce); // Apply the force initial
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play(); // We hit something so play the sound
        if(collision.gameObject.tag == "Brick")
        {
            score += 100; // Update our score
            Destroy(collision.gameObject); // Delete the brick
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "OutBounds")
        {
            if (current_balls == 0) // If we ran out of lives lose our score
            {
                current_balls = number_of_balls;
                score = 0;
            }
            else
                current_balls--; // Lose a life

            Invoke(nameof(SetStartingPos), 2); // Either way, we want to restart the ball on the paddle
        }
    }
}
