using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public static void generateBorders(Tile[,] tiles, int countryNum)
    {
        while (countryNum > 0)
        {
            if (genBorder(tiles))
            {
                countryNum--;
            }
        }
    }

    public static bool genBorder(Tile[,] tiles)
    {
        if(tiles != null)
        {
            Tile startTile = genStartTile(tiles);
            if (startTile == null)
            {
                return false;
            }

            return true;
        }
        return false;
    }

    public static Tile genStartTile(Tile[,] tiles)
    {
        int startX = 0;
        int startY = 0;
        int side = Random.Range(0, 4);
        ref Tile startTile = ref tiles[startX, startY];
        //Debug.Log(side);

        switch (side)
        {
            case 0: //top
                //Debug.Log("from top");
                startX = 0;
                startY = Random.Range(0, Map.width - 1);
                startTile = tiles[startX, startY];

                while (startTile.Biome == Biome.Ocean)
                {
                    int x = startTile.X + 1;
                    if(x >= Map.height)
                    {
                        return null;
                    }
                    startTile = tiles[x, startTile.Y];
                }
                break;
            case 1: //Right
                //Debug.Log("from right");
                startX = Random.Range(0, Map.height - 1);
                startY = Map.width-1;
                startTile = tiles[startX, startY];

                while (startTile.Biome == Biome.Ocean)
                {
                    int y = startTile.Y - 1;
                    if (y < 0)
                    {
                        return null;
                    }
                    startTile = tiles[startTile.X, y];
                }
                break;
            case 2: //Bottom
                //Debug.Log("from bottom");
                startX = Map.height-1;
                startY = Random.Range(0, Map.width - 1);
                startTile = tiles[startX, startY];

                while (startTile.Biome == Biome.Ocean)
                {
                    int x = startTile.X-1;
                    if (x < 0)
                    {
                        return null;
                    }
                    startTile = tiles[x, startTile.Y];
                }
                break;
            default: //Left
                //Debug.Log("from left");
                startX = Random.Range(0, Map.height - 1);
                startY = 0;
                startTile = tiles[startX, startY];

                while (startTile.Biome == Biome.Ocean)
                {
                    int y = startTile.Y + 1;
                    if (y >= Map.width)
                    {
                        return null;
                    }
                    startTile = tiles[startTile.X, y];
                }
                break;
        }

        //Debug.Log("Start tile X:"+startTile.X);
        if(startTile.border == true)
        {
            return null;
        }

        genBorderSphere(startTile);
        startTile.border = true;

        return startTile;
    }

    private static void genBorderSphere(Tile selected)
    {
        GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        s.transform.position = new Vector3(selected.X, (selected.elevation/10)+1, selected.Y);
        s.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1);
    }
}
