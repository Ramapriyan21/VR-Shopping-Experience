using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishingReel : MonoBehaviour
{
    public Material laserMaterial;
    public Material indicationMaterial;
    public Material activationMaterial;

    public float vibrationTime = 0.5f;

    private LineRenderer lineRenderer;
    private GameObject indicatedGameObject = null;
    private Material tmpMaterial;

    private bool vibrating = false;
    private float elapsedVibrationTime = 0.0f;

    private GameObject selectedGameObject = null;
    private float speed = 5.0f;
    private float selectionRadius = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = laserMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rayEnd = transform.position + transform.forward * 100f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            rayEnd = hit.point;

            // Check if the object is not ungrabbable
            if (hit.collider.tag != "ungrabbable")
            {
                Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                Rigidbody rb = hit.collider.gameObject.GetComponent<Rigidbody>();

                if (indicatedGameObject == null && selectedGameObject == null)
                {
                    tmpMaterial = renderer.material;

                    renderer.material = indicationMaterial;

                    indicatedGameObject = hit.collider.gameObject;

                    Debug.Log(hit.collider.gameObject.name);
                }

                if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.09f)
                {
                    if (selectedGameObject == null)
                    {
                        selectedGameObject = hit.collider.gameObject;
                        elapsedVibrationTime = vibrationTime;
                        renderer.material = activationMaterial;

                        if (rb != null)
                        {
                            rb.isKinematic = false;
                        }

                        selectionRadius = Vector3.Distance(transform.position, selectedGameObject.transform.position);
                    }
                }
            }
            else if (indicatedGameObject != null && selectedGameObject == null)
            {
                indicatedGameObject.GetComponent<Renderer>().material = tmpMaterial;
                indicatedGameObject = null;
            }
        }
        else if (indicatedGameObject != null && selectedGameObject == null)
        {
            indicatedGameObject.GetComponent<Renderer>().material = tmpMaterial;
            indicatedGameObject = null;
        }

        if (selectedGameObject != null)
        {
            float thumb = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

            if (Mathf.Abs(thumb) > 0.01f)
            {
                selectionRadius += thumb * speed * Time.deltaTime;
                selectionRadius = Mathf.Max(selectionRadius, 0.0f);
            }

            selectedGameObject.transform.position = transform.position + transform.forward * selectionRadius;
        }
        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) < 0.1f && selectedGameObject != null)
        {
            Debug.Log("Releasing Object: " + selectedGameObject.name); // Debug statement

            selectedGameObject.GetComponent<Renderer>().material = tmpMaterial;

            Rigidbody rb = selectedGameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Set to non-kinematic when releasing
            }

            selectedGameObject = null;
        }


        if (elapsedVibrationTime > 0.0f)
        {
            OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);

            elapsedVibrationTime -= Time.deltaTime;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, rayEnd);
    }
}
