using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private static List<Tile> fringe;
    static private GameObject roadSet;
    public static void createRoad(Tile[,] tiles, Tile start, Tile end)
    {
        roadSet = new GameObject("Roads");

        foreach (Tile t in tiles)
        {
            t.Explored = false;
            t.previous = null;
            t.GVal = float.MaxValue;
            t.HVal = calculateCost(t);
        }

        fringe = new List<Tile>();
        fringe.Add(start);

        start.Explored = true;
        start.GVal = 0;
        start.HVal = 0;

        while(fringe.Count != 0)
        {
            Tile current = fringe[0];
            current.Explored = true;

            fringe.RemoveAt(0);

            if(current.Equals(end))
            {
                Tile temp = current;
                while(temp!=null)
                {
                    //temp.Road = true break causes a stack overflow for some reason
                    //temp.Road = true;
                    GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    c.transform.SetParent(roadSet.transform);
                    c.transform.position = new Vector3(temp.X, 10, temp.Y);
                    temp = temp.previous;
                }
                return;
            }

            if (current.up != null && current.up.Explored == false)
            {
                fringe.Add(current.up);
                current.up.Explored = true;
            }
            if (current.up != null && current.up.GVal > current.GVal + current.up.HVal)
            {
                current.up.GVal = current.GVal + current.up.HVal;
                current.up.previous = current;
            }

            if (current.left != null && current.left.Explored == false)
            {
                fringe.Add(current.left);
                current.left.Explored = true;
            }
            if (current.left != null && current.left.GVal > current.GVal + current.left.HVal)
            {
                current.left.GVal = current.GVal + current.left.HVal;
                current.left.previous = current;
            }

            if (current.right != null && current.right.Explored == false)
            {
                fringe.Add(current.right);
                current.right.Explored = true;
            }
            if (current.right != null && current.right.GVal > current.GVal + current.right.HVal)
            {
                current.right.GVal = current.GVal + current.right.HVal;
                current.right.previous = current;
            }

            if (current.down != null && current.down.Explored == false)
            {
                fringe.Add(current.down);
                current.down.Explored = true;
            }
            if (current.down != null && current.down.GVal > current.GVal + current.down.HVal)
            {
                current.down.GVal = current.GVal + current.down.HVal;
                current.down.previous = current;
            }

            fringe.Sort();
        }
    }
    private static float calculateCost(Tile t)
    { 
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
            case Biome.Savannah:
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
    
}
