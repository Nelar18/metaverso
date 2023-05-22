using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Animator anim;
    private float xRotation = 0f;

    public GameObject Paco, cabezaDePaco;
    public float rotateSpeed;

    public float speed;
    public float sprintSpeed;
    private Rigidbody rb;
    Vector2 inputMov;
    Vector2 mouseMov;
    private bool isRunning;
    private bool isJumping;
    public GameObject cam;

    private void Awake()
    {
        isRunning = false;
        inputMov = new Vector2();
        mouseMov = new Vector2();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
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

        mouseMov.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseMov.y = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
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
        cam.transform.RotateAround( cabezaDePaco.transform.position, transform.right, -1 *mouseMov.y * Time.deltaTime);
    }

}