using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCities : MonoBehaviour
{
    const int scanSize = 1;
    const int oceanVal = 30;


    public static void generateCities(Tile[,] t, int num)
    {
        while(num>0)
        {
            generateCity(t);
            num--;
        }
    }

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
        city.transform.position = new Vector3(bestTile.X, 50, bestTile.Y);
    }

    public static int calculateValue(Tile[,] tiles, Tile tile)
    {
        int scan = scanNearby(tiles, tile);

        return scan;
    }

    public static int scanNearby(Tile[,] tiles, Tile tile)
    {
        int value = 0;

        Tile scan = tile.left;
        value += biomeValue(scan);

        scan = tile.up;
        value += biomeValue(scan);

        scan = tile.right;
        value += biomeValue(scan);

        scan = tile.down;
        value += biomeValue(scan);

        return value;
    }

    public static int biomeValue(Tile tile)
    {
        switch (tile.Biome)
        {
            case Biome.TemperateForest:
                return 100;
            case Biome.Ocean:
                return 30;
            case Biome.BorealForest:
                return 20;
            case Biome.Desert:
                return -20;
            case Biome.Mountain:
                return -10;
            default:
                return 0;
        }
    }
}
