using UnityEngine;
using System.Collections;

public class Forward : MonoBehaviour {

    //float CurrentSpeed = 0;
    //public float turnSpeed = 200f;

    Animator anim;


    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;


	// Use this for initialization
	void Start () {

        //Screen.showCursor = false;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;


        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {


        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }


        //transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed, Space.Self);

        float move = Input.GetAxis("Vertical");
        float direction = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", -move);
        anim.SetFloat("Direction", direction);

        //anim.SetFloat("Enter", 0);

        //if(Input.GetKeyDown(KeyCode.E))
        //    anim.SetFloat("Enter", 1);


        

        //if (CurrentSpeed < 0 && Input.GetAxis("Vertical") == 0)
        //{
        //    CurrentSpeed += 2f * Time.deltaTime;
        //}
        //else if (CurrentSpeed > 0 && Input.GetAxis("Vertical") == 0)
        //{
        //    CurrentSpeed -= 2f * Time.deltaTime;
        //}

        //if (move > 0)
        //{
        //    anim.SetFloat("Speed", -move);
        //    //if (CurrentSpeed < 2f)
        //    //    CurrentSpeed += 1.5f * Time.deltaTime;
        //}
        //if (move < 0)
        //{
        //    anim.SetFloat("Speed", -move);
        //    //if (CurrentSpeed > -2f)
        //    //    CurrentSpeed -= 1.5f * Time.deltaTime;
        //}
        //if (move == 0 && CurrentSpeed > -0.1 && CurrentSpeed < 0.1)
        //    anim.SetFloat("Speed", 0);

        //if(direction < 0)
        //    anim.SetFloat("Direction", 1);
        //if (direction > 0)
        //    anim.SetFloat("Direction", -1);
        //if(direction == 0)
        //    anim.SetFloat("Direction", 0);


        //if (Input.GetAxis("Horizontal") < 0)
        //    anim.SetFloat("Direction", 1);
        //else if (Input.GetAxis("Horizontal") < 0)
        //    anim.SetFloat("Direction", -1);
        
        
        //if (Input.GetAxis("Horizontal") < 0) 
        //    transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        //else if (Input.GetAxis("Horizontal") > 0)
        //    transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
	}

}
