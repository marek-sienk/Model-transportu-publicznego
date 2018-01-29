using UnityEngine;
using System.Collections;

public class BrakeSectorScript : MonoBehaviour {

    //public GameObject VehiclesList;
    public float maxBrakeTorque;
    public float minVehicleSpeed;
    public float width;
    public float depth;
    public float rotation;
    public float position;
    BoxCollider col;

    void Start()
    {
        col = transform.gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
        
    }

    void Update()
    {
        col.size = new Vector3(width, 5f, depth);
        transform.localEulerAngles = new Vector3(0, rotation, 0);
        col.center = new Vector3(0, 0, position);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<SteerScript>() && other.transform.GetComponent<SteerScript>().pathGroup == transform.root)
        {
            float currentVehicleSpeed = other.transform.GetComponent<SteerScript>().currentSpeed;
            if (currentVehicleSpeed >= minVehicleSpeed)
            {
                other.transform.GetComponent<SteerScript>().inSector = true;
                other.transform.GetComponent<SteerScript>().Brake(maxBrakeTorque);
            }
            else
            {
                other.transform.GetComponent<SteerScript>().inSector = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<SteerScript>() && other.transform.GetComponent<SteerScript>().pathGroup == transform.root)
            other.transform.GetComponent<SteerScript>().inSector = false;
    }
}
