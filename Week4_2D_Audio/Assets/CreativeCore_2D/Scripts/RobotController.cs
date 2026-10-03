using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotController : MonoBehaviour
{

    // ========= MOVEMENT =================
    public float speed = 4;
    public InputAction moveAction;

    // =========== MOVEMENT ==============
    Rigidbody2D rigidbody2d;
    Vector2 currentInput;


    // ==== ANIMATION =====
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // ================= SOUNDS =======================
    AudioSource audioSource;

    void Start()
    {
        // =========== MOVEMENT ==============
        rigidbody2d = GetComponent<Rigidbody2D>();
        moveAction.Enable();

        // ==== ANIMATION =====
        animator = GetComponent<Animator>();

        // ==== AUDIO =====
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ============== MOVEMENT ======================
        Vector2 move = moveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        currentInput = move;

        // ============== ANIMATION =======================
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);


    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        position = position + currentInput * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
    
}