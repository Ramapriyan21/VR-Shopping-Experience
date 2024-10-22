using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodSwitcher : MonoBehaviour
{
    private Grabber grabber;
    private FishingReel fishingReel;
    private LineRenderer lineRenderer;

    void Start()
    {
        // Initialize the Grabber and Fishing Reel components
        grabber = GetComponent<Grabber>();
        fishingReel = GetComponent<FishingReel>();
        lineRenderer = GetComponent<LineRenderer>();

        // Start with Grabber enabled by default
        grabber.enabled = true;
        fishingReel.enabled = false;
        // Assuming the Grabber doesn't use the LineRenderer, disable it
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            // Toggle the techniques
            grabber.enabled = !grabber.enabled;
            fishingReel.enabled = !fishingReel.enabled;

            // Toggle the line renderer if it is being used by the Fishing Reel
            if (lineRenderer != null)
            {
                lineRenderer.enabled = fishingReel.enabled;
            }
        }
    }
}
