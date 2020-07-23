using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*public CharacterController controller;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;*/

    private string MoveInputAxis = "Vertical";
    private string TurnInputAxis = "Horizontal";
    private float rotationRate;
    private float moveSpeed;

    /*Vector3 velocity;
    float gravity = -9.81f;
    bool isGrounded;*/

    void Start()
    {
        rotationRate = 90;
        moveSpeed = 0.1f;
    }

    void Update()
    {
        /*isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log(isGrounded);
        if (isGrounded && velocity.y < 0f) velocity.y = 1f;*/

        float moveAxis = Input.GetAxis(MoveInputAxis);
        Move(moveAxis);     

        float turnAxis = Input.GetAxis(TurnInputAxis);
        Turn(turnAxis);

        //Apply gravity to player;
        /*velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);*/
    }

    private void Move(float input)
    {
        transform.Translate(Vector3.forward * input * moveSpeed);
    }

    private void Turn(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    }
}
