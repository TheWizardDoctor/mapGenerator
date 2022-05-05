using UnityEngine;
using Priority_Queue;

public class Road
{
    private static SimplePriorityQueue<Tile> fringe;
    private static readonly int minCost = 5;
    private static readonly Material roadMat = Resources.Load<Material>("Road");
    private static readonly Material OceanPathMat = Resources.Load<Material>("OceanPath");
    private static float[,] gVals;
    private static float[,] hVals;
    private static float[,] fVals;
    private static bool[,] added;
    private static bool[,] closed;

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

        gVals = new float[Map.width, Map.height];
        hVals = new float[Map.width, Map.height];
        fVals = new float[Map.width, Map.height];
        added = new bool[Map.width, Map.height];
        closed = new bool[Map.width, Map.height];

        foreach (Tile t in tiles)
        {
            SetAdded(t, false);
            t.previous = null;
            SetClosed(t, false);
            SetGVal(t, float.MaxValue);
            SetFVal(t, float.MaxValue);
            SetHVal(t, minCost * Mathf.Sqrt(Mathf.Pow(end.X - t.X, 2) + Mathf.Pow(end.Y - t.Y, 2)));
            //t.HVal = minCost * (Mathf.Abs(end.X - t.X) + Mathf.Abs(end.Y - t.Y));
            //t.HVal = minCost * Mathf.Sqrt(Mathf.Pow(end.X - t.X, 2) + Mathf.Pow(end.Y - t.Y, 2));
        }

        fringe = new SimplePriorityQueue<Tile>();
        SetAdded(start, true);
        SetGVal(start, 0);
        SetFVal(start, GetHVal(start));
        fringe.Enqueue(start, GetFVal(start));

        while (fringe.Count != 0)
        {
            Tile current = fringe.Dequeue();

            if (start.City != null && GetGVal(current) > start.City.wealth)
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
                        temp.cube.transform.SetParent(Map.RoadTiles.transform);
                        temp.cube.GetComponent<MeshRenderer>().material = OceanPathMat;
                    }
                    
                    temp = temp.previous;
                }
                return;
            }

            if (current.up != null && !GetClosed(current.up))
            {
                float neighborCost = CalculateCost(current.up);

                if (!GetAdded(current.up))
                {
                    SetAdded(current.up, true);
                    SetGVal(current.up, GetGVal(current) + neighborCost);
                    SetFVal(current.up, GetGVal(current.up) + GetHVal(current.up));
                    current.up.previous = current;
                    fringe.Enqueue(current.up, GetFVal(current.up));

                }
                else
                {
                    if (GetGVal(current.up) > GetGVal(current) + neighborCost)
                    {
                        SetGVal(current.up, GetGVal(current) + neighborCost);
                        SetFVal(current.up, GetGVal(current.up) + GetHVal(current.up));
                        current.up.previous = current;
                        fringe.UpdatePriority(current.up, GetFVal(current.up));
                    }
                }
            }


            if (current.left != null && !GetClosed(current.left))
            {
                float neighborCost = CalculateCost(current.left);

                if (!GetAdded(current.left))
                {
                    SetAdded(current.left, true);
                    SetGVal(current.left, GetGVal(current) + neighborCost);
                    SetFVal(current.left, GetGVal(current.left) + GetHVal(current.left));
                    current.left.previous = current;
                    fringe.Enqueue(current.left, GetFVal(current.left));

                }
                else
                {
                    if (GetGVal(current.left) > GetGVal(current) + neighborCost)
                    {
                        SetGVal(current.left, GetGVal(current) + neighborCost);
                        SetFVal(current.left, GetGVal(current.left) + GetHVal(current.left));
                        current.left.previous = current;
                        fringe.UpdatePriority(current.left, GetFVal(current.left));
                    }
                }
            }

            if (current.right != null && !GetClosed(current.right))
            {
                float neighborCost = CalculateCost(current.right);

                if (!GetAdded(current.right))
                {
                    SetAdded(current.right, true);
                    SetGVal(current.right, GetGVal(current) + neighborCost);
                    SetFVal(current.right, GetGVal(current.right) + GetHVal(current.right));
                    current.right.previous = current;
                    fringe.Enqueue(current.right, GetFVal(current.right));

                }
                else
                {
                    if (GetGVal(current.right) > GetGVal(current) + neighborCost)
                    {
                        SetGVal(current.right, GetGVal(current) + neighborCost);
                        SetFVal(current.right, GetGVal(current.right) + GetHVal(current.right));
                        current.right.previous = current;
                        fringe.UpdatePriority(current.right, GetFVal(current.right));
                    }
                }
            }

            if (current.down != null && !GetClosed(current.down))
            {
                float neighborCost = CalculateCost(current.down);

                if (!GetAdded(current.down))
                {
                    SetAdded(current.down, true);
                    SetGVal(current.down, GetGVal(current) + neighborCost);
                    SetFVal(current.down, GetGVal(current.down) + GetHVal(current.down));
                    current.down.previous = current;
                    fringe.Enqueue(current.down, GetFVal(current.down));

                }
                else
                {
                    if (GetGVal(current.down) > GetGVal(current) + neighborCost)
                    {
                        SetGVal(current.down, GetGVal(current) + neighborCost);
                        SetFVal(current.down, GetGVal(current.down) + GetHVal(current.down));
                        current.down.previous = current;
                        fringe.UpdatePriority(current.down, GetFVal(current.down));
                    }
                }
            }
            SetClosed(current, true);
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
                return 33;
            case Biome.Ocean:
                return 50;
            case Biome.Prairie:
                return 17;
            case Biome.Rainforest:
                return 31;
            case Biome.Savanna:
                return 9;
            case Biome.Shrubland:
                return 15;
            case Biome.TemperateForest:
                return 27;
            case Biome.Tundra:
                return 8;
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

    private static void SetAdded(Tile t, bool b)
    {
        added[t.X, t.Y] = b;
    }
    private static bool GetAdded(Tile t)
    {
        return added[t.X, t.Y];
    }
    private static void SetClosed(Tile t, bool b)
    {
        closed[t.X, t.Y] = b;
    }
    private static bool GetClosed(Tile t)
    {
        return closed[t.X, t.Y];
    }
    private static void SetGVal(Tile t, float val)
    {
        gVals[t.X, t.Y] = val;
    }
    private static float GetGVal(Tile t)
    {
        return gVals[t.X, t.Y];
    }

    private static void SetHVal(Tile t, float val)
    {
        hVals[t.X, t.Y] = val;
    }
    private static float GetHVal(Tile t)
    {
        return hVals[t.X, t.Y];
    }

    private static void SetFVal(Tile t, float val)
    {
        fVals[t.X, t.Y] = val;
    }
    private static float GetFVal(Tile t)
    {
        return fVals[t.X, t.Y];
    }
}
