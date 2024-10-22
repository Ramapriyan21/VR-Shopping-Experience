using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchJoystickMove : MonoBehaviour
{

    public float movementSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(movementSpeed, movementSpeed);
        movement *= OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        movement *= Time.deltaTime;
        transform.Translate(new Vector3(-movement.y, 0.0f, movement.x));

       
    }
}