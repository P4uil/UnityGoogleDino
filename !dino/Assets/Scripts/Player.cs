using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject CurrentScore, HighScore, GameOverText, Restart;
    int Score = 0;
    public Animator anim;
    public float jumpPower = 100f;
    Rigidbody2D myRigidbody;
    bool isGround = false;
    bool isGameOver = false;
    public float xPos = -11f;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        HighScore.GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        anim.SetTrigger("Start");
    }

    void FixedUpdate()
    {
        if (!isGameOver)
        {
            Score++;
            CurrentScore.GetComponent<Text>().text = Score.ToString();
        }

        if(Score>PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", Score);
            HighScore.GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore").ToString();
        }
        if (isGameOver) return;

        if (transform.position.x < xPos)
        {
            GameOver();
        }
        if (Input.GetKey(KeyCode.Space) && isGround) 
        {
            anim.SetBool("Jump",true);
            myRigidbody.AddForce(Vector3.up * jumpPower,ForceMode2D.Impulse);
       
        }
        if (Input.GetKey(KeyCode.DownArrow) && isGround)
        {
            myRigidbody.AddForce(Vector3.down * myRigidbody.gravityScale * myRigidbody.mass * jumpPower);
            anim.SetBool("Down", true);
        }
        else
        {
            anim.SetBool("Down", false);
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    { 
        if (other.collider.tag == "Ground")
        {
            anim.SetBool("Jump",false);
            isGround = true;
        }
        if (other.collider.tag == "Challenges")
        {
            GameOver();
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGround = false;
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGround = true;
        }
    }

    public void GameOver()
    {
        myRigidbody.gravityScale = 0f;
        myRigidbody.bodyType = RigidbodyType2D.Static;
        isGameOver = true;
        GameOverText.SetActive(true);
        Restart.SetActive(true);
        FindObjectOfType<ChalengeScroller>().GameOver();
        FindObjectOfType<Scroll>().xVel = 0f;
        FindObjectOfType<ScrollClouds>().xVel = 0f;
        anim.SetBool("GameOver", true);
    }

    public void Reload()
    {
        SceneManager.LoadScene("SampleScene");
    }
}