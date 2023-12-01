using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    float walkingSpeed = 12f;

    [SerializeField]
    float sprintSpeed = 18.0f;

    [SerializeField]
    float rotationSpeed = 180.0f;

    [Header("Jump")]
    [SerializeField]
    float jumpForce = 25.0f;

    [SerializeField]
    float gravityMultiplier = 12.5f;

    //Private Floats
    float _inputZ;
    float _gravity;
    float _targetRotation;

    //Private Bools
    bool _isJumpPressed;
    bool _isRunning;
    bool _isMovePressed;
    bool _isFacingRight = true;

    //Private Vector3 
    Vector3 _velocity;
    Vector3 _rotation;

    //Private Misc
    CharacterController _characterController;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _gravity = Physics.gravity.y;
    }

    void Update()
    {
        HandleInputs();
        HandleGravity();
        //HandleRotation();
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    void HandleInputs()
    {
        _inputZ = Input.GetAxis("Horizontal");

        _isJumpPressed = Input.GetButtonDown("Jump");

        _isMovePressed = _inputZ != 0.0f;
        _isRunning = _isMovePressed && Input.GetButton("Fire3");
    }

    void HandleMove()
    {
        Vector3 move = transform.forward * _inputZ;
        move.y = _velocity.y;

        float speed = _isRunning ? sprintSpeed : walkingSpeed;

        _characterController.Move(move * speed * Time.deltaTime);
        Debug.Log(_inputZ);
    }

    void HandleRotation()
    {
        if (_isFacingRight && _inputZ < 0f || !_isFacingRight && _inputZ > 0F)
        {
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0.0F, -180.0F, 0.0F);
        }

    }

    /// <summary>
    /// Jump
    /// </summary>
    void HandleGravity()
    {
        if (IsGrounded())
        {
            if (_velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            if (_isJumpPressed)
            {
                HandleJump();
                StartCoroutine(WaitForGroundedCoroutine());
            }
        }
        _velocity.y += _gravity * gravityMultiplier * Time.deltaTime; //Inmediatamente despues del salto, aplica gravedad
    }

    void HandleJump()
    {
        _isJumpPressed = false;
        _velocity.y = Mathf.Sqrt(jumpForce * -2f * _gravity);
    }

    bool IsGrounded()
    {
        return _characterController.isGrounded;
    }

    IEnumerator WaitForGroundedCoroutine()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(() => IsGrounded());
    }


}
