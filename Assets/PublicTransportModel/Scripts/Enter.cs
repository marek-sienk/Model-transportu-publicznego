using UnityEngine;
using System.Collections;

public class Enter : MonoBehaviour
{

    private GameObject vehicle = null;

    private Camera vehicleCamera = null;

    private int isEnter = -1;

    public Camera PlayerCamera;

    public GameObject Vehicles;

    private float MinDistance = 100; //distance from the closest vechicle

    public MonoBehaviour MovementScript;

    private bool I_am_in = false;

    Animator anim;


    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.transform == Vehicles.transform)
        {
            vehicle = other.transform.parent.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!I_am_in)
        {
            vehicle = null;
        }
    }

    void Update()
    {

        //print(vehicle);
        //print(anim.GetCurrentAnimatorStateInfo(0).IsName("Enter"));

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isEnter = isEnter * -1;
        }

        if (isEnter == 1)
        {

            if (!I_am_in)
            {
                if (vehicle != null && !vehicle.GetComponent<SteerScript>().IsClosed())
                {

                    int counter = 0;
                    Transform seat;
                    do
                    {
                        seat = vehicle.GetComponent<SteerScript>().ChooseSeat(counter).transform;
                        print(seat.name);
                        if (vehicle.GetComponent<SteerScript>().getSeatStatus(counter))
                        {
                            counter++;
                        }
                        else
                        {
                            //anim.SetFloat("Enter", 1);
                            //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Enter"))
                            //{
                            vehicleCamera = vehicle.GetComponent<SteerScript>().getCamera();
                            vehicleCamera.enabled = true;
                            PlayerCamera.enabled = false;
                            GetComponent<Collider>().enabled = false;
                            GetComponent<Rigidbody>().isKinematic = true;
                            transform.parent = seat;
                            vehicle.GetComponent<SteerScript>().setSeatStatus(counter, true);
                            transform.localPosition = new Vector3(0, 0.12f, 0.1f);
                            transform.localRotation = new Quaternion(0, 0, 0, 0);
                            MovementScript.enabled = false;
                            I_am_in = true;
                            anim.SetFloat("Speed", 0);
                            anim.SetFloat("Direction", 0);
                            anim.SetFloat("Enter", 1);
                            break;
                            //}
                            //anim.SetFloat("Enter", 0);
                        }
                    } while (vehicle.GetComponent<SteerScript>().ChooseSeat(counter));

                }
                else
                {
                    if (!I_am_in)
                        isEnter = -1;
                }
            }
        }
        else
        {
            if (!I_am_in || !vehicle.GetComponent<SteerScript>().IsClosed())
            {
                GetComponent<Collider>().enabled = true;
                GetComponent<Rigidbody>().isKinematic = false;
                if (vehicleCamera != null)
                    vehicleCamera.enabled = false;
                PlayerCamera.enabled = true;
                if (transform.parent != null)
                {
                    vehicle.GetComponent<SteerScript>().setSeatStatus(int.Parse(transform.parent.name) - 1, false);
                    transform.position = vehicle.GetComponent<SteerScript>().getDoor().transform.position;
                    transform.Translate(2, -2.4f, 0);
                }
                transform.parent = null;
                I_am_in = false;
                anim.SetFloat("Enter", -1);
                MovementScript.enabled = true;
            }
        }

    }

}
