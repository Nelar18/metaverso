using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public float rotateSpeed, speed, sprintSpeed;
    [HideInInspector] public bool limitView;

    public Animator anim;
    public GameObject cam, Paco, cabezaDePaco;
    public AudioSource StepSound, runSound;


    private Rigidbody rb;
    private CharacterController cc;
    private Vector3 inputMov, mouseMov;
    private bool isRunning;

    private void Awake()
    {
        limitView = true;
        inputMov = Vector3.zero;
        mouseMov = Vector3.zero;
        //cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        GetInput();
        Animate();
        Sound();
    }

    private void FixedUpdate()
    {
        Move();
        MoveCam();
    }

    public void GetInput()
    {
        inputMov.x = Input.GetAxisRaw("Horizontal");
        inputMov.z = Input.GetAxisRaw("Vertical");

        GetMouseInput();

        if (Input.GetKey(KeyCode.LeftShift))
            isRunning = true;
        else
            isRunning = false;
    }

    public void Move()
    {
        RotatePlayerModel();

        Vector3 movDirAxis = new Vector3(1,0,1);
        if (isRunning)
        {
            //cc.Move(Vector3.Scale(movDirNormalized, inputMov)*  Time.fixedDeltaTime * sprintSpeed);
            rb.AddRelativeForce(Vector3.Scale(movDirAxis, inputMov) * sprintSpeed * Time.deltaTime);
        }
        else
        {
            //cc.Move(Vector3.Scale(movDirNormalized, inputMov) *  Time.fixedDeltaTime * speed);
            rb.AddRelativeForce(Vector3.Scale(movDirAxis, inputMov) * speed * Time.deltaTime);
        }

        RotateFullPlayer();
       
    }


    private void RotatePlayerModel()
    {
        Vector3 posToLookAt = Vector3.zero;
        posToLookAt.x += inputMov.x;
        posToLookAt.z += inputMov.z + 0.1f;
        Vector3 currPosTr = transform.position;
        transform.Translate(posToLookAt);
        Vector3 relPosToLookAt = transform.position - currPosTr;
        transform.position = currPosTr;

        Quaternion targetRotation = Quaternion.LookRotation(relPosToLookAt);
        Paco.transform.rotation = Quaternion.Slerp(Paco.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void RotateFullPlayer()
    {
        transform.Rotate(Vector3.up * mouseMov.x * Time.deltaTime);
    }

    public void Animate()
    {
        Debug.Log(inputMov.magnitude);
        if (inputMov.magnitude > 0.6f)
            anim.SetBool("isWalking", true);

        if (inputMov.magnitude < 0.4f)
            anim.SetBool("isWalking", false);

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
            Cursor.visible = false;
            mouseMov.x = Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseMov.y = Input.GetAxis("Mouse Y") * mouseSensitivity;
        }
        else
            Cursor.visible = true;


        float eulerX = cam.transform.localEulerAngles.x;

        if (limitView)
            LimitView(60, 20);
        else
            LimitView(80, 80);

    }

    private void LimitView(float max, float min)
    {
        float eulerX = cam.transform.localEulerAngles.x;

        if (eulerX > max && eulerX < max+30 && mouseMov.y < 0)
            mouseMov.y = 0;

        min = 360 -min;
        if (eulerX < min && eulerX > min-30 && mouseMov.y > 0)
            mouseMov.y = 0;
    }

    public void Sound()
    {
        StepSound.mute = true;
        runSound.mute = true;

        if (inputMov.magnitude > 0.6f)
            StepSound.mute = false;

        if ( isRunning )
            StepSound.mute = true;
            runSound.mute = false;
    }

}