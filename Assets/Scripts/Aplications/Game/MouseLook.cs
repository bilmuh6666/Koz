using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity=100f;
    public Transform player;

    public float xRotate = 0;
    
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;

        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, -45f, 45f);
        transform.localRotation=Quaternion.Euler(xRotate,0,0);
        player.Rotate(Vector3.up*mouseX);
    }
}
