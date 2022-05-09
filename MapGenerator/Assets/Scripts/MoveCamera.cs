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

    private bool createCity;
    private bool createRoad;

    private Tile startTile;

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
                camPos.position = new Vector3(camPos.position.x, camPos.position.y - scrollWheel * zoomScale, camPos.position.z);
            }

        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            createCity = true;
            createRoad = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            startTile = null;
            createRoad = true;
            createCity = false;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            TakePicture();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TileUI.S.Disable();

            if (createCity)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Vector2 tilePos = new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.z);
                    City.PlaceNewCity(Map.tiles[Mathf.RoundToInt(tilePos.x), Mathf.RoundToInt(tilePos.y)]);
                }

                createCity = false;
                createRoad = false;
            }
            else if (createRoad)
            {


                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Vector2 tilePos = new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.z);

                    if (startTile == null)
                    {
                        startTile = Map.tiles[Mathf.RoundToInt(tilePos.x), Mathf.RoundToInt(tilePos.y)];
                    }
                    else
                    {
                        Tile endTile = Map.tiles[Mathf.RoundToInt(tilePos.x), Mathf.RoundToInt(tilePos.y)];
                        Road.CreateRoad(startTile, endTile);
                        startTile = null;

                        createCity = false;
                        createRoad = false;
                    }
                }
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Vector2 tilePos = new Vector2(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.z);
                    TileUI.S.SetTileMenu(tilePos);
                }
            }
        }
    }

    private void TakePicture()
    {
        ScreenCapture.CaptureScreenshot("Screenshot.png", 4);
        Debug.Log("Took Screenshot");
    }
}
