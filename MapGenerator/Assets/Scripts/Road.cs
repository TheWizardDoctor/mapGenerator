using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private static List<Tile> fringe;
    private static readonly int minCost = 3;
    private static readonly Material roadMat = Resources.Load<Material>("Road");

    public static void CreateRoad(City start, City end)
    {
        if (start == null || end == null)
        {
            Debug.Log("NULL ERROR");
            return;
        }

        Tile startTile = Map.tiles[start.x, start.y];
        Tile endTile = Map.tiles[end.x, end.y];
        CreateRoad(startTile, endTile);
    }
    public static void CreateRoad(Tile start, Tile end)
    {
        if (start == null || end == null || Map.tiles==null)
        {
            Debug.Log("NULL ERROR");
            return;
        }
        Tile[,] tiles = Map.tiles;

        foreach (Tile t in tiles)
        {
            t.Explored = false;
            t.previous = null;
            t.GVal = float.MaxValue;
            t.FVal = float.MaxValue;
            t.HVal = minCost * (Mathf.Abs(end.X - t.X) + Mathf.Abs(end.Y - t.Y));
        }

        fringe = new List<Tile>();
        fringe.Add(start);
        fringe[0].Explored = true;

        start.Explored = true;
        start.GVal = 0;
        start.FVal = start.HVal;

        while(fringe.Count != 0)
        {
            Tile current = fringe[0];
            //current.Explored = true;
            fringe.RemoveAt(0);

            if (start.City != null && current.GVal > start.City.wealth)
            {
                Debug.Log("Too Much Money");
                return;
            }

            //print("cur loc:(" + current.Y + "," + current.X + ")");
            //print("\tfVal:" + current.FVal);

            if(current.Equals(end))
            {
                Debug.Log("cost:" + current.GVal);
                Tile temp = current;
                while (temp != null)
                {
                    temp.Road = true;

                    temp.cube.GetComponent<MeshRenderer>().material = roadMat;
                    if (temp.Biome == Biome.Ocean)
                    {
                        temp.cube.transform.localScale = new Vector3(temp.cube.transform.localScale.x, temp.cube.transform.localScale.y + 2, temp.cube.transform.localScale.z);
                    }
                    temp = temp.previous;
                }
                return;
            }

            if (current.up != null)
            {
                if(current.up.Explored == false)
                {
                    current.up.Explored = true;
                    fringe.Add(current.up);
                }

                float neighborCost = CalculateCost(current.up);
                if (current.up.GVal > current.GVal + neighborCost)
                {
                    current.up.GVal = current.GVal + neighborCost;
                    current.up.FVal = current.up.GVal + current.up.HVal;
                    current.up.previous = current;
                }
            }
            

            if (current.left != null)
            {
                if (current.left.Explored == false)
                {
                    current.left.Explored = true;
                    fringe.Add(current.left);
                }

                float neighborCost = CalculateCost(current.left);
                if (current.left.GVal > current.GVal + neighborCost)
                {
                    current.left.GVal = current.GVal + neighborCost;
                    current.left.FVal = current.left.GVal + current.left.HVal;
                    current.left.previous = current;
                }
            }
            

            if (current.right != null)
            {
                if(current.right.Explored == false)
                {
                    current.right.Explored = true;
                    fringe.Add(current.right);
                }

                float neighborCost = CalculateCost(current.right);
                if (current.right.GVal > current.GVal + neighborCost)
                {
                    current.right.GVal = current.GVal + neighborCost;
                    current.right.FVal = current.right.GVal + current.right.HVal;
                    current.right.previous = current;
                }
            }
            

            if (current.down != null)
            {
                if (current.down.Explored == false)
                {
                    fringe.Add(current.down);
                    current.down.Explored = true;
                }

                float neighborCost = CalculateCost(current.down);
                if (current.down.GVal > current.GVal + neighborCost)
                {
                    current.down.GVal = current.GVal + neighborCost;
                    current.down.FVal = current.down.GVal + current.down.HVal;
                    current.down.previous = current;
                }
            }

            fringe.Sort();
        }
    }
    private static float CalculateCost(Tile t)
    { 
        if(t.Road)
        {
            return minCost;
        }

        switch(t.Biome)
        {
            case Biome.BorealForest:
                return 25;
            case Biome.Desert:
                return 5;
            case Biome.Mountain:
                return 30;
            case Biome.Ocean:
                return 40;
            case Biome.Prairie:
                return 5;
            case Biome.Rainforest:
                return 30;
            case Biome.Savanna:
                return 5;
            case Biome.Shrubland:
                return 5;
            case Biome.TemperateForest:
                return 20;
            case Biome.Tundra:
                return 10;
            default:
                return 0;
        }
    }

    public static GameObject RoadSet { get; set; }
}
