using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateTerrain : MonoBehaviour
{
    //height y, width x
    public static List<Tile> CreateInitialMountains()
    {
        List<Tile> unsetTiles = new List<Tile>();
        Tile thisTile;
        int initialX;
        int initialY;
        int r = Random.Range(7, 10);
        for (int i = 0; i < r; i++)
        {
            initialX = Random.Range((int)(Map.S.width * .25), (int)(Map.S.width * .75));
            initialY = Random.Range((int)(Map.S.height * .25), (int)(Map.S.height * .75));
            thisTile = Map.S.tiles[initialX, initialY];
            if (thisTile.Elevation <= 0)
            {
                foreach (Tile t in GetNeighbors(thisTile))
                {
                    if (!unsetTiles.Contains(t) & t.Elevation <= 0)
                    {
                        t.previous = thisTile;
                        unsetTiles.Add(t);
                    }
                }
                unsetTiles = SetMountainRange(unsetTiles, thisTile, Random.Range(30, 45));
            }
        }

        return unsetTiles;
    }

    private static List<Tile> SetMountainRange(List<Tile> unsetTiles, Tile thisTile, int rangeLength)
    {
        List<Tile> neighbors = GetNeighbors(thisTile);
        thisTile.Elevation = Random.Range(55f, 60f);
        unsetTiles.Remove(thisTile);
        if (rangeLength > 0 & neighbors.Count > 0)
        {
            int index = Random.Range((int)0, (int)neighbors.Count);
            for (int i = 0; i < neighbors.Count; i++)
            {
                Tile nextTile = neighbors[index];
                List<Tile> nextNeighbors = GetNeighbors(nextTile);
                if (nextNeighbors.Count >= 3)
                {
                    foreach (Tile t in nextNeighbors)
                    {
                        if (!unsetTiles.Contains(t) & t.Elevation <= 0)
                        {
                            t.previous = thisTile;
                            unsetTiles.Add(t);
                        }
                    }
                    return SetMountainRange(unsetTiles, nextTile, rangeLength - 1);
                }
                else
                {
                    index++;
                    if (index >= neighbors.Count)
                    {
                        index = 0;
                    }
                }
            }
        }
        return unsetTiles;
    }

    public static void SetElevations(List<Tile> unsetTiles)
    {
        int scale = 1;
        int rand = Random.Range((int)0, (int)unsetTiles.Count);

        Tile thisTile = unsetTiles[rand];
        unsetTiles.RemoveAt(rand);
        List<Tile> neighbors = GetNeighbors(thisTile);
        foreach (Tile t in neighbors)
        {
            if (!unsetTiles.Contains(t) & t.Elevation <= 0)
            {
                t.previous = thisTile;
                unsetTiles.Add(t);
            }
        }
        List<float> slopes = new List<float>();
        thisTile.Elevation = EstSlope(thisTile);

        slopes.Add(CalculateSlopeLeft(thisTile));
        slopes.Add(CalculateSlopeRight(thisTile));

        slopes.Add(CalculateSlopeUp(thisTile));
        slopes.Add(CalculateSlopeDown(thisTile));

        //Debug.Log("Est: " + estSlope(thisTile).ToString() + "	Left: " + .ToString() + "	Right: " + .ToString() + "	Up: " + .ToString() + "	Down: " + .ToString() + calculateSlopeDown(prevTile).ToString());
        float averageSlope = AverageSlopes(slopes, slopes.Count);
        float elevation = thisTile.Elevation;
        elevation = (averageSlope * scale) + elevation;
        //float wobble = elevation + (float)System.Math.Pow(6, 1 - 0.0025 * elevation) - 3.5f;
        elevation = Random.Range(elevation - 2.5f, elevation + 2.5f);

        if (elevation <= -10)
        {
            elevation = -10;
        }
        if (elevation >= 70)
        {
            elevation = 70;
        }
        thisTile.Elevation = elevation;
    }

    private static float EstSlope(Tile thisTile)
    {
        Tile prevTile = thisTile.previous;
        double e = prevTile.Elevation;
        e = 0.9 * e - System.Math.Pow(0.002 * e, 2);
        float newElevation = (float)e;
        if (float.IsNaN(newElevation))
        {
            newElevation = 0;
        }
        //Debug.Log("y1: " + prevTile.Elevation.ToString() + "    y2: " + newElevation.ToString());
        return newElevation;
    }

    private static float CalculateSlopeLeft(Tile c)
    {
        //calculate slope coming from left going right
        //tile arangement looks like this
        // b1 <-> a1 <-> c <-> a2 <-> b2
        Tile a1 = c.left;
        Tile b1 = a1.left;
        Tile a2 = c.right;
        Tile b2 = a2.right;
        List<float> slopes = new List<float>();
        if (a1.Elevation > 0)
        {
            slopes.Add(PointSlope(0, c.Elevation, 1, a1.Elevation));
        }
        if (b1.Elevation > 0)
        {
            slopes.Add(PointSlope(0, c.Elevation, 2, b1.Elevation));
        }
        return AverageSlopes(slopes, slopes.Count);
    }

    private static float CalculateSlopeRight(Tile c)
    {
        //calculate slope coming from right going left
        //tile arangement looks like this
        // b2 <-> a2 <-> c <-> a1 <-> b1
        Tile a1 = c.right;
        Tile b1 = a1.right;
        Tile a2 = c.left;
        Tile b2 = a2.left;
        List<float> slopes = new List<float>();
        if (a1.Elevation > 0)
        {
            slopes.Add(PointSlope(0, c.Elevation, 1, a1.Elevation));
        }
        if (b1.Elevation > 0)
        {
            slopes.Add(PointSlope(0, c.Elevation, 2, b1.Elevation));
        }
        return AverageSlopes(slopes, slopes.Count);
    }

    private static float CalculateSlopeUp(Tile c)
    {
        //calculate slope coming from up going down
        //tile arangement looks like this
        // b2 <-> a2 <-> c <-> a1 <-> b1
        List<float> slopes = new List<float>();
        if (c.up != null)
        {
            Tile a1 = c.up;
            if (a1.Elevation > 0)
            {
                slopes.Add(PointSlope(0, c.Elevation, 1, a1.Elevation));
            }
            if (a1.up != null)
            {
                Tile b1 = a1.up;
                if (b1.Elevation > 0)
                {
                    slopes.Add(PointSlope(0, c.Elevation, 2, b1.Elevation));
                }
            }
        }
        return AverageSlopes(slopes, slopes.Count);
    }

    private static float CalculateSlopeDown(Tile c)
    {
        //calculate slope coming from left going right
        //tile arangement looks like this
        // b1 <-> a1 <-> c <-> a2 <-> b2
        List<float> slopes = new List<float>();
        if (c.down != null)
        {
            Tile a1 = c.down;
            if (a1.Elevation > 0)
            {
                slopes.Add(PointSlope(0, c.Elevation, 1, a1.Elevation));
            }
            if (a1.down != null)
            {
                Tile b1 = a1.down;
                if (b1.Elevation > 0)
                {
                    slopes.Add(PointSlope(0, c.Elevation, 2, b1.Elevation));
                }
            }
        }
        return AverageSlopes(slopes, slopes.Count);
    }

    private static float PointSlope(float x1, float y1, float x2, float y2)
    {
        float m = (y2 - y1) / (x2 - x1);
        return m;
    }

    private static float AverageSlopes(List<float> slopes, int count)
    {
        float sum = 0;
        if (count != 0)
        {
            foreach (float f in slopes)
            {
                sum += f;
            }
            return (sum / count);
        }
        else
        {
            return 0;
        }
    }

    public static List<Tile> GetNeighbors(Tile center)
    {
        List<Tile> unsetNeighbors = new List<Tile>();
        if (center.left != null)
        {
            if (center.left.Elevation < 0)
            {
                unsetNeighbors.Add(center.left);
            }
        }
        if (center.up != null)
        {
            if (center.up.Elevation < 0)
            {
                unsetNeighbors.Add(center.up);
            }
        }
        if (center.right != null)
        {
            if (center.right.Elevation < 0)
            {
                unsetNeighbors.Add(center.right);
            }
        }
        if (center.down != null)
        {
            if (center.down.Elevation < 0)
            {
                unsetNeighbors.Add(center.down);
            }
        }

        return unsetNeighbors.OrderBy(t => Random.Range(0, 10)).ToList();
    }

    public static void UnclutterOcean()
    {
        for (int j = 0; j < Map.S.height; j++)
        {
            for (int i = 0; i < Map.S.width; i++)
            {
                //Debug.Log("biome: " + Map.S.tiles[i, j].Biome.ToString());
                int oceanCount = 0;
                if (Map.S.tiles[i, j].Elevation >= 5)
                {
                    for (int x = -3; x <= 3; x++)
                    {
                        for (int y = -3; y <= 3; y++)
                        {
                            try
                            {
                                if (Map.S.tiles[i + x, j + y].Elevation < 5)
                                {
                                    oceanCount++;
                                }
                            }
                            catch (System.IndexOutOfRangeException ex)
                            {
                                System.IndexOutOfRangeException e = ex;
                                oceanCount++;
                            }
                        }
                    }
                    if (oceanCount >= 39)
                    {
                        //Debug.Log("x: " + i.ToString() + "	y: " + j.ToString());
                        Map.S.tiles[i, j].Elevation = 4;
                    }
                }
            }
        }
    }
}
