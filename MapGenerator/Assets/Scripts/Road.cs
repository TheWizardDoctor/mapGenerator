using UnityEngine;
using Priority_Queue;

public class Road
{
    private static SimplePriorityQueue<Tile> fringe;
    private static readonly int minCost = 5;
    private static readonly GameObject roadPrefab = Resources.Load<GameObject>("Road");
    private static readonly GameObject oceanPathPrefab = Resources.Load<GameObject>("OceanPath");
    private static float[,] gVals;
    private static float[,] hVals;
    private static float[,] fVals;
    private static bool[,] added;
    private static bool[,] closed;

    public static void CreateRoad(City start, City end)
    {
        if (start == null || end == null || Map.tiles == null)
        {
            return;
        }

        Tile startTile = Map.tiles[start.X, start.Y];
        Tile endTile = Map.tiles[end.X, end.Y];
        CreateRoad(startTile, endTile);
    }
    public static void CreateRoad(Tile start, Tile end)
    {
        if (start == null || end == null || Map.tiles == null)
        {
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
            if (t == null)
            {
                continue;
            }

            SetAdded(t, false);
            t.previous = null;
            SetClosed(t, false);
            SetGVal(t, float.MaxValue);
            SetFVal(t, float.MaxValue);
            SetHVal(t, minCost * Mathf.Sqrt(Mathf.Pow(end.X - t.X, 2) + Mathf.Pow(end.Y - t.Y, 2)));
        }

        fringe = new SimplePriorityQueue<Tile>();

        if (fringe == null)
        {
            return;
        }

        SetAdded(start, true);
        SetGVal(start, 0);
        SetFVal(start, GetHVal(start));
        fringe.Enqueue(start, GetFVal(start));

        while (fringe.Count != 0)
        {
            Tile current = fringe.Dequeue();

            if (current == null)
            {
                continue;
            }

            //if (start.City != null && GetGVal(current) > start.City.Wealth)
            //{
            //    Debug.Log("Too Much Money");
            //    return;
            //}

            if (current.Equals(end))
            {
                Tile temp = current;
                while (temp != null)
                {
                    if (temp.Road == false)
                    {
                        temp.Road = true;

                        Vector3 pos = new Vector3(temp.cube.transform.position.x,
                            temp.cube.transform.position.y + 0.025f + (0.5f * temp.cube.transform.localScale.y),
                            temp.cube.transform.position.z);
                        if (temp.Biome != Biome.Ocean)
                        {
                            GameObject newRoad = Object.Instantiate(roadPrefab, pos, temp.cube.transform.rotation);
                            if (newRoad)
                            {
                                newRoad.transform.SetParent(Map.RoadTiles.transform);
                            }
                        }
                        else
                        {
                            GameObject newRoad = GameObject.Instantiate<GameObject>(oceanPathPrefab, pos, temp.cube.transform.rotation);
                            if (newRoad)
                            {
                                newRoad.transform.SetParent(Map.RoadTiles.transform);
                            }
                        }
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
        if (t == null)
        {
            return 100;
        }

        return t.Road && t.Biome != Biome.Ocean ? minCost : t.Biome switch
        {
            Biome.BorealForest => 25,
            Biome.Desert => 10,
            Biome.Mountain => 33,
            Biome.Ocean => 50,
            Biome.Prairie => 17,
            Biome.Rainforest => 31,
            Biome.Savanna => 9,
            Biome.Shrubland => 15,
            Biome.TemperateForest => 27,
            Biome.Tundra => 8,
            _ => 100,
        };
    }

    private static void SetAdded(Tile t, bool b)
    {
        if (t != null)
        {
            added[t.X, t.Y] = b;
        }
    }
    private static bool GetAdded(Tile t)
    {
        return t != null && added[t.X, t.Y];
    }
    private static void SetClosed(Tile t, bool b)
    {
        if (t != null)
        {
            closed[t.X, t.Y] = b;
        }
    }
    private static bool GetClosed(Tile t)
    {
        return t != null && closed[t.X, t.Y];
    }
    private static void SetGVal(Tile t, float val)
    {
        if (t != null)
        {
            gVals[t.X, t.Y] = val;
        }
    }
    private static float GetGVal(Tile t)
    {
        return t != null ? gVals[t.X, t.Y] : float.MaxValue;
    }

    private static void SetHVal(Tile t, float val)
    {
        if (t != null)
        {
            hVals[t.X, t.Y] = val;
        }
    }
    private static float GetHVal(Tile t)
    {
        return t != null ? hVals[t.X, t.Y] : float.MaxValue;
    }

    private static void SetFVal(Tile t, float val)
    {
        if (t != null)
        {
            fVals[t.X, t.Y] = val;
        }
    }
    private static float GetFVal(Tile t)
    {
        return t != null ? fVals[t.X, t.Y] : float.MaxValue;
    }
}
