using UnityEngine;
using Priority_Queue;

public class Road
{
    private static SimplePriorityQueue<Tile> fringe;
    private static readonly int minCost = 5;
    private static readonly Material roadMat = Resources.Load<Material>("Road");
    private static readonly Material OceanPathMat = Resources.Load<Material>("OceanPath");

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
        if (start == null || end == null || Map.tiles == null)
        {
            Debug.Log("NULL ERROR");
            return;
        }
        Tile[,] tiles = Map.tiles;

        foreach (Tile t in tiles)
        {
            t.Explored = false;
            t.previous = null;
            t.closed = false;
            t.GVal = float.MaxValue;
            t.FVal = float.MaxValue;
            //t.HVal = minCost * (Mathf.Abs(end.X - t.X) + Mathf.Abs(end.Y - t.Y));
            t.HVal = minCost * Mathf.Sqrt(Mathf.Pow(end.X - t.X, 2) + Mathf.Pow(end.Y - t.Y, 2));
        }

        fringe = new SimplePriorityQueue<Tile>();
        start.Explored = true;
        start.GVal = 0;
        start.FVal = start.HVal;
        fringe.Enqueue(start, start.FVal);

        while (fringe.Count != 0)
        {
            Tile current = fringe.Dequeue();

            if (start.City != null && current.GVal > start.City.wealth)
            {
                Debug.Log("Too Much Money");
                return;
            }

            //print("cur loc:(" + current.Y + "," + current.X + ")");
            //print("\tfVal:" + current.FVal);

            if (current.Equals(end))
            {
                //Debug.Log("cost:" + current.GVal);
                Tile temp = current;
                while (temp != null)
                {
                    temp.Road = true;

                    if(temp.Biome!=Biome.Ocean)
                    {
                        temp.cube.GetComponent<MeshRenderer>().material = roadMat;
                    }
                    else
                    {
                        temp.cube.GetComponent<MeshRenderer>().material = OceanPathMat;
                    }
                    
                    temp = temp.previous;
                }
                return;
            }

            if (current.up != null && !current.up.closed)
            {
                float neighborCost = CalculateCost(current.up);

                if (current.up.Explored == false)
                {
                    current.up.Explored = true;
                    current.up.GVal = current.GVal + neighborCost;
                    current.up.FVal = current.up.GVal + current.up.HVal;
                    current.up.previous = current;
                    fringe.Enqueue(current.up, current.up.FVal);

                }
                else
                {
                    if (current.up.GVal > current.GVal + neighborCost)
                    {
                        current.up.GVal = current.GVal + neighborCost;
                        current.up.FVal = current.up.GVal + current.up.HVal;
                        current.up.previous = current;
                        fringe.UpdatePriority(current.up, current.up.FVal);
                    }
                }


            }


            if (current.left != null && !current.left.closed)
            {
                float neighborCost = CalculateCost(current.left);

                if (current.left.Explored == false)
                {
                    current.left.Explored = true;
                    current.left.GVal = current.GVal + neighborCost;
                    current.left.FVal = current.left.GVal + current.left.HVal;
                    current.left.previous = current;
                    fringe.Enqueue(current.left, current.left.FVal);

                }
                else
                {
                    if (current.left.GVal > current.GVal + neighborCost)
                    {
                        current.left.GVal = current.GVal + neighborCost;
                        current.left.FVal = current.left.GVal + current.left.HVal;
                        current.left.previous = current;
                        fringe.UpdatePriority(current.left, current.left.FVal);
                    }
                }


            }


            if (current.right != null && !current.right.closed)
            {
                float neighborCost = CalculateCost(current.right);

                if (current.right.Explored == false)
                {
                    current.right.Explored = true;
                    current.right.GVal = current.GVal + neighborCost;
                    current.right.FVal = current.right.GVal + current.right.HVal;
                    current.right.previous = current;
                    fringe.Enqueue(current.right, current.right.FVal);

                }
                else
                {
                    if (current.right.GVal > current.GVal + neighborCost)
                    {
                        current.right.GVal = current.GVal + neighborCost;
                        current.right.FVal = current.right.GVal + current.right.HVal;
                        current.right.previous = current;
                        fringe.UpdatePriority(current.right, current.right.FVal);
                    }
                }


            }


            if (current.down != null && !current.down.closed)
            {
                float neighborCost = CalculateCost(current.down);

                if (current.down.Explored == false)
                {
                    current.down.Explored = true;
                    current.down.GVal = current.GVal + neighborCost;
                    current.down.FVal = current.down.GVal + current.down.HVal;
                    current.down.previous = current;
                    fringe.Enqueue(current.down, current.down.FVal);

                }
                else
                {
                    if (current.down.GVal > current.GVal + neighborCost)
                    {
                        current.down.GVal = current.GVal + neighborCost;
                        current.down.FVal = current.down.GVal + current.down.HVal;
                        current.down.previous = current;
                        fringe.UpdatePriority(current.down, current.down.FVal);
                    }
                }

                
            }

            current.closed = true;
        }
    }
    private static float CalculateCost(Tile t)
    {
        if (t.Road)
        {
            return minCost;
        }

        switch (t.Biome)
        {
            case Biome.BorealForest:
                return 25;
            case Biome.Desert:
                return 10;
            case Biome.Mountain:
                return 30;
            case Biome.Ocean:
                return 40;
            case Biome.Prairie:
                return 10;
            case Biome.Rainforest:
                return 30;
            case Biome.Savanna:
                return 10;
            case Biome.Shrubland:
                return 10;
            case Biome.TemperateForest:
                return 20;
            case Biome.Tundra:
                return 10;
            default:
                return 10;
        }
    }

    //private static void FringeSortedAdd(List<Tile> fringe, Tile tileToAdd)
    //{
    //    if (fringe.Count == 0) //|| tileToAdd.FVal >= fringe[fringe.Count - 1].FVal)
    //    {
    //        fringe.Add(tileToAdd);
    //        return;
    //    }

    //    int low = 0;
    //    int high = fringe.Count - 1;

    //    while (low <= high)
    //    {
    //        int mid = (low + high) / 2;

    //        if (tileToAdd.FVal == fringe[mid].FVal)
    //        {
    //            fringe.Insert(mid, tileToAdd);
    //            return;
    //        }
    //        else if (mid != 0 && fringe[mid - 1].FVal <= tileToAdd.FVal && fringe[mid].FVal > tileToAdd.FVal)
    //        {
    //            fringe.Insert(mid, tileToAdd);
    //            return;
    //        }
    //        else if (fringe[mid].FVal < tileToAdd.FVal)
    //        {
    //            low = mid + 1;
    //        }
    //        else
    //        {
    //            high = mid - 1;
    //        }
    //    }
    //}
    /*private static void FringeSortedAdd(LinkedList<Tile> fringe, Tile tileToAdd)
    {
        float val = tileToAdd.FVal;
        LinkedListNode<Tile> current = fringe.First;

        while (current != null && current.Value.FVal < val)
        {
            current = current.Next;
        }

        if (current == null)
        {
            fringe.AddLast(tileToAdd);
        }
        else
        {
            fringe.AddBefore(current, tileToAdd);
        }
    }*/

    //public static GameObject RoadSet { get; set; }
}
