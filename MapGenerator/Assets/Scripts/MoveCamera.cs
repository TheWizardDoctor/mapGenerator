using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //private Transform cam;
    private Transform camPos;
    private float zoomScale = 5;
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

                camPos.position = new Vector3(camPos.position.x, camPos.position.y, camPos.position.z + scrollWheel * zoomScale);

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

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    City.GenerateCities(Map.scanRadius * 5);

        //    for (int i = 0; i < City.cityList.Count; i++)
        //    {
        //        double rand = RandomNum.r.NextDouble();

        //        if(rand < 0.333)
        //        {
        //            City.TradeRouteFood(City.cityList[i]);
        //        }
        //        else if(rand < 0.666)
        //        {
        //            City.TradeRouteLumber(City.cityList[i]);
        //        }
        //        else
        //        {
        //            City.TradeRouteWater(City.cityList[i]);
        //        }
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            Tile one, two;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                one = Map.tiles[RandomNum.r.Next(Map.width), RandomNum.r.Next(Map.height)];
                two = Map.tiles[RandomNum.r.Next(Map.width), RandomNum.r.Next(Map.height)];
                Road.CreateRoad(one, two);
            }
            watch.Stop();
            Debug.Log("Time to create 10 road(s) is:" + watch.ElapsedMilliseconds + "ms");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //very simplistic city creation
            //(currently only checks 8 nearby tiles to get tile's creation value)
            var watch = System.Diagnostics.Stopwatch.StartNew();
            City.GenerateCities(1);
            watch.Stop();
            Debug.Log("Time to create 1 cities is:" + watch.ElapsedMilliseconds + "ms");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            int random = 0, random2 = 0;
            while (random == random2 && City.cityList.Count > 1)
            {
                random = RandomNum.r.Next(City.cityList.Count);
                random2 = RandomNum.r.Next(City.cityList.Count);
            }

            Tile one = Map.tiles[City.cityList[random].x, City.cityList[random].y];
            Tile two = Map.tiles[City.cityList[random2].x, City.cityList[random2].y];

            if (one != null && two != null)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Road.CreateRoad(one, two);
                watch.Stop();
                Debug.Log("Time to create 1 road is:" + watch.ElapsedMilliseconds + "ms");
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                Vector2 tilePos = new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.z);
                TileUI.S.SetTileMenu(tilePos);
            }
            else
            {
                TileUI.S.Disable();
            }
        }
    }
}
