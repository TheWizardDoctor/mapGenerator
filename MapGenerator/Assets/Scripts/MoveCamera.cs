using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Action { displayTileInfo, createCity, createRoad, destroyCity, destroyRoad };

public class MoveCamera : MonoBehaviour
{
    //private Transform cam;
    private Transform camPos;
    private float zoomScale = 5;
    private float scrollWheel;

    private Action userAction;

    private Tile startTile;

    private Vector3 dragOrigin;

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
            userAction = Action.createCity;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            userAction = Action.createRoad;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            TakePicture();
        }
        else if(Input.GetKeyDown(KeyCode.T))
        {
            userAction = Action.destroyRoad;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            userAction = Action.displayTileInfo;
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            userAction = Action.destroyCity;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TileUI.S.Disable();
            dragOrigin = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2Int tilePos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));

                if (userAction == Action.createCity)
                {
                    Debug.Log("Place City");
                    City.PlaceNewCity(Map.tiles[tilePos.x, tilePos.y]);
                }
                else if (userAction == Action.createRoad)
                {
                    if (startTile == null)
                    {
                        startTile = Map.tiles[tilePos.x, tilePos.y];
                    }
                    else
                    {
                        Tile endTile = Map.tiles[tilePos.x, tilePos.y];
                        Road.CreateRoad(startTile, endTile);
                        startTile = null;
                    }
                }
                else if (userAction == Action.destroyRoad)
                {
                    if (hit.collider.gameObject.layer == 7)
                    {
                        Tile roadTile = Map.tiles[tilePos.x, tilePos.y];
                        roadTile.Road = false;
                        Destroy(hit.collider.gameObject);
                    }
                }
                else if(userAction == Action.destroyCity)
                {
                    Tile cityTile = Map.tiles[tilePos.x, tilePos.y];
                    City.RemoveCity(cityTile);
                }
                else
                {
                    TileUI.S.SetTileMenu(tilePos);
                }
            }
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x, 0, pos.y);

        transform.Translate(move, Space.World);
    }

    private void TakePicture()
    {
        ScreenCapture.CaptureScreenshot("Screenshot.png", 4);
        Debug.Log("Took Screenshot");
    }
}
