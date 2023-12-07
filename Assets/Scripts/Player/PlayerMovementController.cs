using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementController : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 200f;

    private Animator anim;
    public float x, y;

    public Rigidbody rb;
    public float fuerzaDeSalto = 8.0f;
    public bool puedoSaltar;

    public bool gameOver;

    void Start()
    {
        puedoSaltar = false;
        anim = GetComponent<Animator>();
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

        if(puedoSaltar == true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("Salte", true);
                rb.AddForce(new Vector3(0, fuerzaDeSalto, 0), ForceMode.Impulse);
            }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dead"))
        {
            Debug.Log("Game Over");
            gameOver = true;
        }
    }
}