using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    float moveX, moveY, moveZ, xMouse, yMouse = 0;
    float speed = 8;

    // Start is called before the first frame update
    void Start()
    {
        Tile temp = Map.tiles[0, 0];
        if (temp != null)
        {
            Vector3 pos = new Vector3(temp.cube.transform.position.x,
                            temp.cube.transform.position.y + 3f + (0.5f * temp.cube.transform.localScale.y),
                            temp.cube.transform.position.z);

            transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        moveZ = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(moveX, 0, moveZ);

        //looking around
        xMouse += Input.GetAxis("Mouse X") * speed;
        yMouse -= Input.GetAxis("Mouse Y") * speed;
        yMouse = Mathf.Clamp(yMouse, -90f, 90f);

        transform.localEulerAngles = new Vector3(yMouse, xMouse, 0);

    }
}
