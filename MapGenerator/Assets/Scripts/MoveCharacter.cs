using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    float moveX, moveZ, xMouse, yMouse = 0;
    float defSpeed = 5;
    float speed;

    // Start is called before the first frame update
    private void Start()
    {
        speed = defSpeed;
        Tile temp = Map.S.tiles[0, 0];
        if (temp != null)
        {
            Vector3 pos = new Vector3(temp.cube.transform.position.x,
                            temp.cube.transform.position.y + 3f + (0.5f * temp.cube.transform.localScale.y),
                            temp.cube.transform.position.z);

            transform.position = pos;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //movement
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        moveZ = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(moveX, 0, moveZ);

        if (moveX == 0 && moveZ == 0)
        {
            speed = defSpeed;
        }
        else
        {
            speed += 2f * Time.deltaTime;
        }

        //looking around
        xMouse += Input.GetAxis("Mouse X") * defSpeed / 2;
        yMouse -= Input.GetAxis("Mouse Y") * defSpeed / 2;
        yMouse = Mathf.Clamp(yMouse, -90f, 90f);

        transform.localEulerAngles = new Vector3(yMouse, xMouse, 0);

    }
}
