
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public string grabbableTag = "ungrabbable";
    private GameObject grabbed = null;
    private Vector3 grabbedOffset = Vector3.zero;
    private bool rGrabbed = false;
    private bool lGrabbed = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed != null)
        {
            if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) < 0.05 && rGrabbed == true)
            {
                grabbed.GetComponent<Rigidbody>().isKinematic = false;
                grabbed = null;
                rGrabbed = false;
                transform.parent.GetComponent<BoxCollider>().enabled = true;
                grabbed.GetComponent<BoxCollider>().enabled = true;


            }
            if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) < 0.05 && lGrabbed == true)
            {
                grabbed.GetComponent<Rigidbody>().isKinematic = false;
                grabbed = null;
                lGrabbed = false;
                transform.parent.GetComponent<BoxCollider>().enabled = true;
                grabbed.GetComponent<BoxCollider>().enabled = true;

            }
            else
            {
                grabbed.transform.position = transform.position + grabbedOffset;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (grabbed == null)
        {
            if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9f && other.gameObject.tag != grabbableTag)
            {
                rGrabbed = true;
                grabbed = other.gameObject;
                grabbed.GetComponent<Rigidbody>().isKinematic = false;
                grabbedOffset = grabbed.transform.position - transform.position;

                grabbed.GetComponent<Rigidbody>().isKinematic = true;

                transform.parent.GetComponent<BoxCollider>().enabled = false;
                grabbed.GetComponent<BoxCollider>().enabled = false;

            }
            if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.9f && other.gameObject.tag != grabbableTag)
            {
                lGrabbed = true;
                grabbed = other.gameObject;
                grabbedOffset = grabbed.transform.position - transform.position;

                grabbed.GetComponent<Rigidbody>().isKinematic = true;
                transform.parent.GetComponent<BoxCollider>().enabled = false;
                grabbed.GetComponent<BoxCollider>().enabled = false;


            }
        }
    }
}