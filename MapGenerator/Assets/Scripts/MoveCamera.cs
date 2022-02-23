using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //private Transform cam;
    private Transform camPos;
    private float zoomScale=5;
    private float scrollWheel;

    private void Awake()
    {
        camPos = gameObject.GetComponent<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        scrollWheel = Input.mouseScrollDelta.y;

        if (scrollWheel != 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {

                camPos.position = new Vector3(camPos.position.x, camPos.position.y, camPos.position.z - scrollWheel * zoomScale);

            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                camPos.position = new Vector3(camPos.position.x - scrollWheel * zoomScale, camPos.position.y, camPos.position.z);
            }
            else
            {
                //used for orthographic Camera
                //cam.orthographicSize -= scrollWheel*zoomScale;

                //used for perspective camera
                camPos.position = new Vector3(camPos.position.x, camPos.position.y - scrollWheel * zoomScale, camPos.position.z);
            }
            
        }
    }
}
