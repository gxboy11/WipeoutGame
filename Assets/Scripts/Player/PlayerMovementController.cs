using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200f;

    private Animator anim;
    private float x, y;

    public Rigidbody rb;
    public float fuerzaDeSalto = 8.0f;
    public bool puedoSaltar;

    public bool gameOver;
    public Vector3 spawnPosition;

    void Start()
    {
        anim = GetComponent<Animator>();
        spawnPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.Rotate(0, x * Time.deltaTime * velocidadRotacion, 0);
        transform.Translate(0, 0, y * Time.deltaTime * velocidadMovimiento);
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        if (puedoSaltar && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Salte", true);
            rb.AddForce(new Vector3(0, fuerzaDeSalto, 0), ForceMode.Impulse);
            puedoSaltar = false; // Disable jumping until the next grounded state
        }

        if (puedoSaltar)
        {
            anim.SetBool("TocarSuelo", true);
        }
        else
        {
            EstoyCayendo();
        }
    }

    public void EstoyCayendo()
    {
        anim.SetBool("TocarSuelo", false);
        anim.SetBool("Salte", false);
    }



    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            puedoSaltar = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            anim.SetBool("Salte", false);
            anim.SetBool("TocarSuelo", true);
            EnableJump();
        }
        else if (collision.gameObject.CompareTag("Dead"))
        {
            Debug.Log("Game Over");
            gameOver = true;
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            anim.SetBool("Salte", false);
            Respawn();
            CoinCollector.scoreCount = 0;
        }
        else
        {
            anim.SetBool("Salte", false);
            anim.SetBool("TocarSuelo", true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.tag);

        if (other.CompareTag("Water"))
        {
            Debug.Log("Touching Water");
            Respawn();
        }
        else if (other.CompareTag("Ground") || other.CompareTag("MovingPlatform"))
        {
            Debug.Log("Touching Ground or MovingPlatform");
            anim.SetBool("Salte", false);
            EnableJump();
        }

        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            CoinCollector.scoreCount += 1;
        }
    }


    private void EnableJump()
    {
        anim.SetBool("Salte", false);
        puedoSaltar = true;
        anim.SetBool("TocarSuelo", true);
        Debug.Log("Jump Enabled");
    }


    private void Respawn()
    {
        transform.position = spawnPosition;
        anim.SetBool("TocarSuelo", true);
        anim.SetBool("Salte", false); // Reset the jumping animation
        puedoSaltar = true; // Allow jumping again
        gameOver = false;

        EnableJump(); // Reset jump availability
        Debug.Log("Respawned");
    }

}
