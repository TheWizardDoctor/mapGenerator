using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCities : MonoBehaviour
{
    const int scanSize = 1;
    const int oceanVal = 30;

    //create num number of cities
    public static void generateCities(Tile[,] t, int num)
    {
        while(num>0)
        {
            generateCity(t);
            num--;
        }
    }

    //create a single city
    public static void generateCity(Tile[,] tiles)
    {
        int bestVal = int.MinValue;
        ref Tile bestTile = ref tiles[0, 0];

        foreach (Tile tile in tiles)
        {
            if(tile.City==true || tile.Biome==Biome.Ocean)
            {
                continue;
            }

            int currentValue = calculateValue(tiles, tile);
            if (currentValue > bestVal)
            {
                bestVal = currentValue;
                bestTile = tile;
            }
        }

        Debug.Log("Best City Location (X:" + bestTile.X + " Y:" + bestTile.Y + ")");

        bestTile.City = true;
        GameObject city = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        city.transform.position = new Vector3(bestTile.X, ((bestTile.Elevation+10)/2)+1, bestTile.Y);
    }

    //get value of current tile based on neighboring tiles
    public static int calculateValue(Tile[,] tiles, Tile tile)
    {
        int scan = scanNearby(tiles, tile);

        return scan;
    }

    public static int scanNearby(Tile[,] tiles, Tile tile)
    {
        int value = 0;

        //left
        Tile scan = tile.left;
        value += biomeValue(scan);
        value += hasCity(scan);

        //left up diag
        scan = scan.up;
        value += biomeValue(scan);
        value += hasCity(scan);

        //up
        scan = tile.up;
        value += biomeValue(scan);
        value += hasCity(scan);

        //right diag up
        scan = scan.right;
        value += biomeValue(scan);
        value += hasCity(scan);

        //right
        scan = tile.right;
        value += biomeValue(scan);
        value += hasCity(scan);

        //right diag down
        scan = scan.down;
        value += biomeValue(scan);
        value += hasCity(scan);

        //down
        scan = tile.down;
        value += biomeValue(scan);
        value += hasCity(scan);

        //left diag down
        scan = scan.left;
        value += biomeValue(scan);
        value += hasCity(scan);

        return value;
    }

    public static int biomeValue(Tile tile)
    {
        switch (tile.Biome)
        {
            case Biome.BorealForest:
                return 20;
            case Biome.Desert:
                return -20;
            case Biome.Mountain:
                return -10;
            case Biome.Ocean:
                return 30;
            case Biome.Prairie:
                return 20;
            case Biome.Rainforest:
                return 25;
            case Biome.Savannah:
                return 20;
            case Biome.Shrubland:
                return 10;
            case Biome.TemperateForest:
                return 25;
            case Biome.Tundra:
                return 5;
            default:
                return 0;
        }
    }

    public static int hasCity(Tile tile)
    {
        if(tile.City==true)
        {
            return -1000;
        }
        else
        {
            return 0;
        }
    }
}
