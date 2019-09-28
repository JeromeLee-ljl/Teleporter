using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camera;
    public float moveSpeed;
    public float rotateSpeed;
    public float upRotateRange = 80;
    public float downRotateRange = 60;

    [HideInInspector] public Rigidbody Rbody;

    // Start is called before the first frame update
    void Start()
    {
        Rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.None) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = transform.TransformDirection(new Vector3(h, 0, v)).normalized;
        Rbody.velocity = moveSpeed * dir;
        // Rbody.MovePosition(transform.position + Time.deltaTime * moveSpeed * dir);


        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Time.deltaTime * rotateSpeed * mouseX * Vector3.up);
        float rotateX = camera.localEulerAngles.x;
        rotateX = rotateX > 180 ? rotateX - 360 : rotateX;
        if (mouseY > 0 && rotateX < -upRotateRange ||
            mouseY < 0 && rotateX > downRotateRange) return;
        camera.Rotate(Time.deltaTime * rotateSpeed * mouseY * Vector3.left);
    }

    private void FixedUpdate()
    {

    }

}