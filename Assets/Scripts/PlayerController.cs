using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public float rotateSpeed, speed, sprintSpeed;

    public Animator anim;
    public GameObject cam, Paco, cabezaDePaco;

    private float xRotation = 0f;
    private Rigidbody rb;
    private Vector2 inputMov, mouseMov;
    private bool isRunning;

    private void Awake()
    {
        //isJumping = false;
        isRunning = false;
        inputMov = new Vector2();
        mouseMov = new Vector2();
        rb = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        GetInput();
        Animate();
    }

    private void FixedUpdate()
    {
        Move();
        MoveCam();
    }

    public void GetInput()
    {
        inputMov.x = Input.GetAxisRaw("Horizontal");
        inputMov.y = Input.GetAxisRaw("Vertical");

        GetMouseInput();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        else
        {
            isRunning = false;
        }
    }

    public void Move()
    {
        Vector3 posToLookAt = Vector3.zero;
        posToLookAt.x += inputMov.x;
        posToLookAt.z += inputMov.y + 0.1f;
        Vector3 currPosTr = transform.position;
        transform.Translate(posToLookAt);
        Vector3 relPosToLookAt = transform.position - currPosTr;
        transform.position = currPosTr;

        Quaternion targetRotation = Quaternion.LookRotation(relPosToLookAt);
        Paco.transform.rotation = Quaternion.Slerp(Paco.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (isRunning)
        {
            rb.AddRelativeForce(Vector3.right * inputMov.x * sprintSpeed * Time.deltaTime);
            rb.AddRelativeForce(Vector3.forward * inputMov.y * sprintSpeed * Time.deltaTime);
        }
        else
        {
            rb.AddRelativeForce(Vector3.right * inputMov.x * speed * Time.deltaTime);
            rb.AddRelativeForce(Vector3.forward * inputMov.y * speed * Time.deltaTime);
        }


        xRotation -= mouseMov.y * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseMov.x * Time.deltaTime);
    }

    public void Animate()
    {
        if (inputMov.magnitude > 0.6f)
        {
            anim.SetBool("isWalking", true);
        }

        if (inputMov.magnitude < 0.4f)
        {
            anim.SetBool("isWalking", false);
        }

        anim.SetBool("isRunning", isRunning);
    }


    public void MoveCam()
    {
        cam.transform.RotateAround(cabezaDePaco.transform.position, transform.right, -1 * mouseMov.y * Time.deltaTime);
    }

    private void GetMouseInput()
    {
        mouseMov.x = 0;
        mouseMov.y = 0;
        if (Input.GetMouseButton(1))
        {
            mouseMov.x = Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseMov.y = Input.GetAxis("Mouse Y") * mouseSensitivity;
        }

        float eulerX = cam.transform.localEulerAngles.x;

        if (eulerX > 60 && eulerX < 90 && mouseMov.y < 0)
        {
            mouseMov.y = 0;
        }

        if (eulerX < 340 && eulerX > 300 && mouseMov.y > 0)
        {
            mouseMov.y = 0;
        }
    }

}