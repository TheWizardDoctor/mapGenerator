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

        if(Input.GetKeyDown(KeyCode.T))
        {
            City start = City.cityList[0];

            City.TradeRouteFood(start);
        }
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            Tile one, two;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            one = Map.tiles[RandomNum.r.Next(100), RandomNum.r.Next(100)];
            two = Map.tiles[RandomNum.r.Next(100), RandomNum.r.Next(100)];
            Road.CreateRoad(one, two);
            watch.Stop();
            Debug.Log("Time to create 1 road(s) is:" + watch.ElapsedMilliseconds + "ms");
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            //very simplistic city creation
            //(currently only checks 8 nearby tiles to get tile's creation value)
            var watch = System.Diagnostics.Stopwatch.StartNew();
            City.GenerateCities(1);
            watch.Stop();
            Debug.Log("Time to create 1 cities is:" + watch.ElapsedMilliseconds + "ms");
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            int random=0, random2=0;
            while(random==random2 && City.cityList.Count>1)
            {
                random = RandomNum.r.Next(City.cityList.Count);
                random2 = RandomNum.r.Next(City.cityList.Count);
            }

            Tile one = Map.tiles[City.cityList[random].x, City.cityList[random].y];
            Tile two = Map.tiles[City.cityList[random2].x, City.cityList[random2].y];

            if (one != null && two != null)
            {
                Road.CreateRoad(one, two);
            }
        }
    }
}
