using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMove : MonoBehaviour
{
    [FormerlySerializedAs("Controller")] public CharacterController controller;
    public float speed = 15;

    private Vector3 velocity;
    public float gravity = -9.81f;

    public Transform isGroundedObject;
    public float groundDistance;
    public LayerMask groundMask;
    public bool isGrounded;

    public float jumpHeight = 3;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(isGroundedObject.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float mouseX = Input.GetAxis("Horizontal");
        float mouseZ = Input.GetAxis("Vertical");

        var transform1 = transform;
        Vector3 moveVector = transform1.right * mouseX + transform1.forward * mouseZ;

        controller.Move(moveVector * (speed * Time.deltaTime));

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InHouse"))
        {
            KozEventServices.GameAction.PlayerInHouse?.Invoke();
        }
    }
}