using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [Header("System")]
    public Camera cam;
    

    //Privates
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private Vector3 jumpheight = Vector3.zero;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody> ();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = -_velocity;
       
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotationCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    public void Jump(float _jumpHeight)
    {
        jumpheight = new Vector3 (0f, _jumpHeight, 0f);
    }
    private void FixedUpdate()
    {
        PlayRotation();
        PlayMoviment();
    }

    void PlayRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if(cam != null)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }

    void PlayMoviment()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (jumpheight != Vector3.zero)
        {
            rb.MovePosition (rb.position + jumpheight * Time.fixedDeltaTime);
            jumpheight = Vector3.zero;
        }
            
    }

    
}
