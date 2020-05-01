using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [Header("PlayerConfig")]
    public float speed = 5f;
    public float speedRun = 7f;
    public float speedWalk = 3f;
    public float sensibility = 3f;
    public float jumpHeight = 5f;
    public bool enableMouse = true;
    public Camera cam;
    public GameObject Spray;
    public Animator anim;

    [Space]
    [Header("SystemConfig")]
    public GameObject leftFoot;
    public GameObject rightFoot;
    private float foots = 0f;

    //Private
    private PlayerMotor motor;
    private float RealSpeed = 0f;
    public bool hold = false;
    private float waitTime = 0f;
    private bool sprayExist = false;
    
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        
    }

   

    void Update()
    {
        //MouseLock
        if (enableMouse == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //Speed Controller
        if (Input.GetButton("Run") == true && Input.GetMouseButton(1) == false)
        {
            RealSpeed = speedRun;
        }
        else if (Input.GetButton("Walk") == true)
            RealSpeed = speedWalk;
        else
            RealSpeed = speed;

        //Moviment
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _MoveHorizontal = transform.right * _xMov;
        Vector3 _MoveVertical = transform.forward * _zMov;
        if (enableMouse == true)
        {
            if (_xMov != 0 || _zMov != 0)
                hold = true;
            else
                hold = false;
        }

        if(_zMov != 0)
        {
            anim.SetBool("walk", true);
            if (RealSpeed == speedRun)
                anim.SetBool("run", true);
            else
                anim.SetBool("run", false);
        }
        else
        {
            anim.SetBool("walk", false);
        }

        if (hold == true)
        {
            waitTime += 1;
        }

        if (waitTime > 10 && hold == true)
        {

            if (foots > 10 && foots < 15)
            {
                if (Input.GetButton("Walk") == false)
                    Instantiate(rightFoot, transform.position, transform.rotation);

                foots += 1 * RealSpeed;
            }
            else if (foots > 30)
            {
                if (Input.GetButton("Walk") == false)
                    Instantiate(rightFoot, transform.position, transform.rotation);
                foots = 0;
            }
            else
            {

                foots += 1 * RealSpeed;
            }
            waitTime = 0;
        }
        //Place Moviment
        Vector3 _velocity = (_MoveHorizontal + _MoveVertical).normalized * RealSpeed;


        if (enableMouse == true )
            motor.Move(_velocity);

        //Rotation
        float _yMouse = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yMouse, 0f) * sensibility;

        if (enableMouse == true)
        {
            motor.Rotate(_rotation);
        }

        //Rotation Camera
        float _xMouse = Input.GetAxisRaw("Mouse Y");
        Vector3 _cameraRotation = new Vector3(_xMouse, 0f, 0f) * sensibility;

        if (enableMouse == true)
        {
            motor.RotationCamera(_cameraRotation);
        }

        if (Input.GetButton("Jump") == true)
            motor.Jump(jumpHeight);
        else
            motor.Jump(0);

        if (enableMouse == true && Input.GetButton("Spray"))
        {
            if (sprayExist == false)
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5))
                {
                    Instantiate(Spray, hit.point, Quaternion.LookRotation(hit.normal));
                }
                sprayExist = true;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5))
                {
                    SprayScript go = (FindObjectOfType(typeof(SprayScript)) as SprayScript);
                    go.gameObject.transform. position = hit.point;
                    go.gameObject.transform.rotation = Quaternion.LookRotation(hit.normal);
                }
            }
            }
        }
    }
