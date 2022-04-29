using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public static void generateBorders(Tile[,] map, int countryNum)
    {
//        Debug.Log(Map.height);
//        Debug.Log(Map.width);
        while (countryNum > 1)
        {
//            Debug.Log("Create Border Start");
            if (genBorder(map))
            {
                countryNum--;
            }
        }
    }

    public static bool genBorder(Tile[,] map)
    {
        if(map != null)
        {
//            Debug.Log("Get Startpoint");
            Tile startTile = genStartTile(map);
            if (startTile == null)
            {
                return false;
            }

//            Debug.Log("Generate Border");
            decideDirection(map, startTile);

//            Debug.Log("Establish Border");
            establishBorder(map, startTile);

            return true;
        }
        return false;
    }

    // Establishes the border, setting all tiles to official (2), and creating a marker
    public static void establishBorder(Tile[,] tiles, Tile startTile)
    {
        startTile.Border = 2;
        genBorderSphere(startTile);
        Tile current = startTile;
        if (current.X == 0 || current.X == Map.width-1 
            || current.Y == 0 || current.Y == Map.height-1)
        {
            return;
        }

        current = tiles[startTile.X - 1, startTile.Y - 1];
        if(current.Border == 1)
        {
            establishBorder(tiles, current);
        }
        current = tiles[startTile.X - 1, startTile.Y];
        if (current.Border == 1)
        {
            establishBorder(tiles, current);
        }
        current = tiles[startTile.X - 1, startTile.Y + 1];
        if (current.Border == 1)
        {
            establishBorder(tiles, current);
        }
        current = tiles[startTile.X, startTile.Y - 1];
        if (current.Border == 1)
        {
            establishBorder(tiles, current);
        }
        current = tiles[startTile.X, startTile.Y + 1];
        if (current.Border == 1)
        {
            establishBorder(tiles, current);
        }
        current = tiles[startTile.X + 1, startTile.Y - 1];
        if (current.Border == 1)
        {
            establishBorder(tiles, current);
        }
        current = tiles[startTile.X + 1, startTile.Y];
        if (current.Border == 1)
        {
            establishBorder(tiles, current);
        }
        current = tiles[startTile.X + 1, startTile.Y + 1];
        if (current.Border == 1)
        {
            establishBorder(tiles, current);
        }
    }

    // Erases prospective border, reseting all prospective (1) tiles to non border (0)
    public static void eraseBorder(Tile[,] tiles, Tile startTile)
    {
        startTile.Border = 0;

        Tile current = tiles[startTile.X - 1, startTile.Y - 1];
        if (current.Border == 1)
        {
            eraseBorder(tiles, current);
        }
        current = tiles[startTile.X - 1, startTile.Y];
        if (current.Border == 1)
        {
            eraseBorder(tiles, current);
        }
        current = tiles[startTile.X - 1, startTile.Y + 1];
        if (current.Border == 1)
        {
            eraseBorder(tiles, current);
        }
        current = tiles[startTile.X, startTile.Y - 1];
        if (current.Border == 1)
        {
            eraseBorder(tiles, current);
        }
        current = tiles[startTile.X, startTile.Y + 1];
        if (current.Border == 1)
        {
            eraseBorder(tiles, current);
        }
        current = tiles[startTile.X + 1, startTile.Y - 1];
        if (current.Border == 1)
        {
            eraseBorder(tiles, current);
        }
        current = tiles[startTile.X + 1, startTile.Y];
        if (current.Border == 1)
        {
            eraseBorder(tiles, current);
        }
        current = tiles[startTile.X + 1, startTile.Y + 1];
        if (current.Border == 1)
        {
            eraseBorder(tiles, current);
        }
    }

    public static void decideDirection(Tile[,] tiles, Tile startTile)
    {
        int direction = Random.Range(0, 4);
        Tile nextTile = startTile;

        switch (direction)
        {
            case 0: // N,S
                nextTile = tiles[startTile.X - 1, startTile.Y];
                nextTile.Border = 1;
                genWeightedBorder(tiles, nextTile);
                nextTile = tiles[startTile.X + 1, startTile.Y];
                nextTile.Border = 1;
                genWeightedBorder(tiles, nextTile);
                break;
            case 1: // NE,SW
                nextTile = tiles[startTile.X - 1, startTile.Y + 1];
                nextTile.Border = 1;
                genWeightedBorder(tiles, nextTile);
                nextTile = tiles[startTile.X + 1, startTile.Y - 1];
                nextTile.Border = 1;
                genWeightedBorder(tiles, nextTile);
                break;
            case 2: // E,W
                nextTile = tiles[startTile.X, startTile.Y + 1];
                nextTile.Border = 1;
                genWeightedBorder(tiles, nextTile);
                nextTile = tiles[startTile.X, startTile.Y - 1];
                nextTile.Border = 1;
                genWeightedBorder(tiles, nextTile);
                break;
            case 3: // NW,SE
                nextTile = tiles[startTile.X+1, startTile.Y - 1];
                nextTile.Border = 1;
                genWeightedBorder(tiles, nextTile);
                nextTile = tiles[startTile.X-1, startTile.Y + 1];
                nextTile.Border = 1;
                genWeightedBorder(tiles, nextTile);
                break;
        }
    }

    // Prairie->Shrubland->TemperateForest->Savannah->BorealForest->Desert->Tundra->Rainforest->Mountain->River->Ocean
    public static Tile genWeightedBorder(Tile[,] tiles, Tile awayTile)
    {
        Tile finishTile = awayTile;
        int burstLength = Map.height/20;
//        Debug.Log(burstLength);

        while(detectBorder(tiles, finishTile) == false
            || finishTile.X < 0
            || finishTile.X > Map.width - 1
            || finishTile.Y > Map.height - 1
            || finishTile.Y < 0)
        {
            Tile prevTile = findPrev(tiles, finishTile);
            int xCord = prevTile.X - finishTile.X;
            int yCord = prevTile.Y - finishTile.Y;
//            Debug.Log("xCord: "+xCord);
//            Debug.Log("yCord: " + yCord);

            if (xCord < 0) // S, SE, SW
            {
                if (yCord < 0) // SE
                {
                    finishTile = judgeSE(tiles, finishTile, burstLength);
                }
                if(yCord == 0) // S
                {
                    finishTile = judgeS(tiles, finishTile, burstLength);
                }
                if (yCord > 0) // SW
                {
                    finishTile = judgeSW(tiles, finishTile, burstLength);
                }
            }
            if(xCord == 0) // E, W
            {
                if (yCord < 0) // E
                {
                    finishTile = judgeE(tiles, finishTile, burstLength);
                }
                if (yCord > 0) // W
                {
                    finishTile = judgeW(tiles, finishTile, burstLength);
                }
            }
            if(xCord > 0) // N, NE, NW
            {
                if (yCord < 0) // NE
                {
                    finishTile = judgeNE(tiles, finishTile, burstLength);
                }
                if (yCord == 0) // N
                {
                    finishTile = judgeN(tiles, finishTile, burstLength);
                }
                if (yCord > 0) // NW
                {
                    finishTile = judgeNW(tiles, finishTile, burstLength);
                }
            }
        }

        return finishTile;
    }

    // Judges the 3 furtherest tiles from the previous border tile for the heaviest weighted tile,
    // The bursts in that direction. Returns burst endpoint tile.
    public static Tile judgeN(Tile[,] map, Tile selected, int burstLength)
    {
        Tile finishTile = selected;
        float left = map[selected.X - 1, selected.Y - 1].NavigationDifficulty;
        float right = map[selected.X - 1, selected.Y + 1].NavigationDifficulty;
        float middle = map[selected.X - 1, selected.Y].NavigationDifficulty;
        //Debug.Log("Weights are"+left+" "+middle+" "+right);

        if (middle == left && middle == right)
        {
            finishTile = burstN(map, finishTile, burstLength);
        }
        if (middle > left && middle > right)
        {
            finishTile = burstN(map, finishTile, burstLength);
        }
        if (middle == left && middle > right)
        {
            finishTile = burstN(map, finishTile, burstLength);
        }
        if (middle > left && middle == right)
        {
            finishTile = burstN(map, finishTile, burstLength);
        }
        if (middle < left && right < left)
        {
            finishTile = burstNW(map, finishTile, burstLength);
        }
        if (middle < right && left < right)
        {
            finishTile = burstNE(map, finishTile, burstLength);
        }
        if (middle < right && left == right)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    finishTile = burstNE(map, finishTile, burstLength);
                    break;
                default:
                    finishTile = burstNW(map, finishTile, burstLength);
                    break;
            }
        }

        return finishTile;
    }
    public static Tile judgeNE(Tile[,] map, Tile selected, int burstLength)
    {
        Tile finishTile = selected;
        float left = map[selected.X - 1, selected.Y].NavigationDifficulty;
        float right = map[selected.X, selected.Y + 1].NavigationDifficulty;
        float middle = map[selected.X - 1, selected.Y + 1].NavigationDifficulty;
        //Debug.Log("Weights are" + left + " " + middle + " " + right);

        if (middle == left && middle == right)
        {
            finishTile = burstNE(map, finishTile, burstLength);
        }
        if (middle > left && middle > right)
        {
            finishTile = burstNE(map, finishTile, burstLength);
        }
        if (middle == left && middle > right)
        {
            finishTile = burstNE(map, finishTile, burstLength);
        }
        if (middle > left && middle == right)
        {
            finishTile = burstNE(map, finishTile, burstLength);
        }
        if (middle < left && right < left)
        {
            finishTile = burstN(map, finishTile, burstLength);
        }
        if (middle < right && left < right)
        {
            finishTile = burstE(map, finishTile, burstLength);
        }
        if (middle < right && left == right)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    finishTile = burstE(map, finishTile, burstLength);
                    break;
                default:
                    finishTile = burstN(map, finishTile, burstLength);
                    break;
            }
        }

        return finishTile;
    }
    public static Tile judgeE(Tile[,] map, Tile selected, int burstLength)
    {
        Tile finishTile = selected;
        float left = map[selected.X - 1, selected.Y + 1].NavigationDifficulty;
        float right = map[selected.X + 1, selected.Y + 1].NavigationDifficulty;
        float middle = map[selected.X, selected.Y + 1].NavigationDifficulty;
        //Debug.Log("Weights are" + left + " " + middle + " " + right);

        // Extends the border in a given general direction for a given length
        if (middle == left && middle == right)
        {
            finishTile = burstE(map, finishTile, burstLength);
        }
        if (middle > left && middle > right)
        {
            finishTile = burstE(map, finishTile, burstLength);
        }
        if (middle == left && middle > right)
        {
            finishTile = burstE(map, finishTile, burstLength);
        }
        if (middle > left && middle == right)
        {
            finishTile = burstE(map, finishTile, burstLength);
        }
        if (middle < left && right < left)
        {
            finishTile = burstNE(map, finishTile, burstLength);
        }
        if (middle < right && left < right)
        {
            finishTile = burstSE(map, finishTile, burstLength);
        }
        if (middle < right && left == right)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    finishTile = burstNE(map, finishTile, burstLength);
                    break;
                default:
                    finishTile = burstSE(map, finishTile, burstLength);
                    break;
            }
        }

        return finishTile;
    }
    public static Tile judgeSE(Tile[,] map, Tile selected, int burstLength)
    {
        Tile finishTile = selected;
        float left = map[selected.X, selected.Y + 1].NavigationDifficulty;
        float right = map[selected.X + 1, selected.Y].NavigationDifficulty;
        float middle = map[selected.X + 1, selected.Y + 1].NavigationDifficulty;
        //Debug.Log("Weights are" + left + " " + middle + " " + right);

        if (middle == left && middle == right)
        {
            finishTile = burstSE(map, finishTile, burstLength);
        }
        if (middle > left && middle > right)
        {
            finishTile = burstSE(map, finishTile, burstLength);
        }
        if (middle == left && middle > right)
        {
            finishTile = burstSE(map, finishTile, burstLength);
        }
        if (middle > left && middle == right)
        {
            finishTile = burstSE(map, finishTile, burstLength);
        }
        if (middle < left && right < left)
        {
            finishTile = burstE(map, finishTile, burstLength);
        }
        if (middle < right && left < right)
        {
            finishTile = burstS(map, finishTile, burstLength);
        }
        if (middle < right && left == right)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    finishTile = burstS(map, finishTile, burstLength);
                    break;
                default:
                    finishTile = burstE(map, finishTile, burstLength);
                    break;
            }
        }

        return finishTile;
    }
    public static Tile judgeS(Tile[,] map, Tile selected, int burstLength)
    {
        Tile finishTile = selected;
        float left = map[selected.X + 1, selected.Y - 1].NavigationDifficulty;
        float right = map[selected.X + 1, selected.Y + 1].NavigationDifficulty;
        float middle = map[selected.X + 1, selected.Y].NavigationDifficulty;
        //Debug.Log("Weights are" + left + " " + middle + " " + right);

        if (middle == left && middle == right)
        {
            finishTile = burstS(map, finishTile, burstLength);
        }
        if (middle > left && middle > right)
        {
            finishTile = burstS(map, finishTile, burstLength);
        }
        if (middle == left && middle > right)
        {
            finishTile = burstS(map, finishTile, burstLength);
        }
        if (middle > left && middle == right)
        {
            finishTile = burstS(map, finishTile, burstLength);
        }
        if (middle < left && right < left)
        {
            finishTile = burstSW(map, finishTile, burstLength);
        }
        if (middle < right && left < right)
        {
            finishTile = burstSE(map, finishTile, burstLength);
        }
        if (middle < right && left == right)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    finishTile = burstSE(map, finishTile, burstLength);
                    break;
                default:
                    finishTile = burstSW(map, finishTile, burstLength);
                    break;
            }
        }

        return finishTile;
    }
    public static Tile judgeSW(Tile[,] map, Tile selected, int burstLength)
    {
        Tile finishTile = selected;
        float left = map[selected.X + 1, selected.Y].NavigationDifficulty;
        float right = map[selected.X, selected.Y - 1].NavigationDifficulty;
        float middle = map[selected.X + 1, selected.Y - 1].NavigationDifficulty;
        //Debug.Log("Weights are" + left + " " + middle + " " + right);

        if (middle == left && middle == right)
        {
            finishTile = burstSW(map, finishTile, burstLength);
        }
        if (middle > left && middle > right)
        {
            finishTile = burstSW(map, finishTile, burstLength);
        }
        if (middle == left && middle > right)
        {
            finishTile = burstSW(map, finishTile, burstLength);
        }
        if (middle > left && middle == right)
        {
            finishTile = burstSW(map, finishTile, burstLength);
        }
        if (middle < left && right < left)
        {
            finishTile = burstS(map, finishTile, burstLength);
        }
        if (middle < right && left < right)
        {
            finishTile = burstW(map, finishTile, burstLength);
        }
        if (middle < right && left == right)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    finishTile = burstS(map, finishTile, burstLength);
                    break;
                default:
                    finishTile = burstW(map, finishTile, burstLength);
                    break;
            }
        }

        return finishTile;
    }
    public static Tile judgeW(Tile[,] map, Tile selected, int burstLength)
    {
        Tile finishTile = selected;
        float left = map[selected.X + 1, selected.Y - 1].NavigationDifficulty;
        float right = map[selected.X - 1, selected.Y - 1].NavigationDifficulty;
        float middle = map[selected.X, selected.Y - 1].NavigationDifficulty;
        //Debug.Log("Weights are" + left + " " + middle + " " + right);

        if (middle == left && middle == right)
        {
            finishTile = burstW(map, finishTile, burstLength);
        }
        if (middle > left && middle > right)
        {
            finishTile = burstW(map, finishTile, burstLength);
        }
        if (middle == left && middle > right)
        {
            finishTile = burstW(map, finishTile, burstLength);
        }
        if (middle > left && middle == right)
        {
            finishTile = burstW(map, finishTile, burstLength);
        }
        if (middle < left && right < left)
        {
            finishTile = burstSW(map, finishTile, burstLength);
        }
        if (middle < right && left < right)
        {
            finishTile = burstNW(map, finishTile, burstLength);
        }
        if (middle < right && left == right)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    finishTile = burstNW(map, finishTile, burstLength);
                    break;
                default:
                    finishTile = burstSW(map, finishTile, burstLength);
                    break;
            }
        }

        return finishTile;
    }
    public static Tile judgeNW(Tile[,] map, Tile selected, int burstLength)
    {
        Tile finishTile = selected;
        float left = map[selected.X, selected.Y - 1].NavigationDifficulty;
        float right = map[selected.X - 1, selected.Y].NavigationDifficulty;
        float middle = map[selected.X - 1, selected.Y - 1].NavigationDifficulty;
        //Debug.Log("Weights are" + left + " " + middle + " " + right);

        if (middle == left && middle == right)
        {
            finishTile = burstNW(map, finishTile, burstLength);
        }
        if (middle > left && middle > right)
        {
            finishTile = burstNW(map, finishTile, burstLength);
        }
        if (middle == left && middle > right)
        {
            finishTile = burstNW(map, finishTile, burstLength);
        }
        if (middle > left && middle == right)
        {
            finishTile = burstNW(map, finishTile, burstLength);
        }
        if (middle < left && right < left)
        {
            finishTile = burstW(map, finishTile, burstLength);
        }
        if (middle < right && left < right)
        {
            finishTile = burstN(map, finishTile, burstLength);
        }
        if (middle < right && left == right)
        {
            int rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    finishTile = burstN(map, finishTile, burstLength);
                    break;
                default:
                    finishTile = burstW(map, finishTile, burstLength);
                    break;
            }
        }

        return finishTile;
    }

    // Extends the border in a given general direction for a given length
    public static Tile burstN(Tile[,] map, Tile selected, int burst)
    {
        if (burst == 0 || detectBorder(map, selected) == true
            || selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            //            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }

        int rand = Random.Range(-1, 2);
        Tile next = map[selected.X - 1, selected.Y + rand];
        //        Debug.Log("Next point is " + next.X + ", " + next.Y);
        burst--;
        Tile endPoint = burstN(map, next, burst);
        if (endPoint == null)
        {
            return null;
        }

        selected.Border = 1;
        return endPoint;
    }
    public static Tile burstNE(Tile[,] map, Tile selected, int burst)
    {
        if (burst == 0 || detectBorder(map, selected) == true
            || selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            //            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }

        int rand = Random.Range(0, 2);
        Tile next;
        switch (rand)
        {
            case 0:
                next = map[selected.X - 1, selected.Y];
                break;
            case 1:
                next = map[selected.X - 1, selected.Y + 1];
                break;
            default:
                next = map[selected.X, selected.Y + 1];
                break;
        }

        //        Debug.Log("Next point is " + next.X + ", " + next.Y);
        burst--;
        Tile endPoint = burstNE(map, next, burst);
        if (endPoint == null)
        {
            return null;
        }

        selected.Border = 1;
        return endPoint;
    }
    public static Tile burstE(Tile[,] map, Tile selected, int burst)
    {
        if (burst == 0 || detectBorder(map, selected) == true
            || selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            //            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }

        int rand = Random.Range(-1, 2);
        Tile next = map[selected.X + rand, selected.Y + 1];

        //        Debug.Log("Next point is " + next.X + ", " + next.Y);
        burst--;
        Tile endPoint = burstE(map, next, burst);
        if (endPoint == null)
        {
            return null;
        }

        selected.Border = 1;
        return endPoint;
    }
    public static Tile burstSE(Tile[,] map, Tile selected, int burst)
    {
        if (burst == 0 || detectBorder(map, selected) == true
            || selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            //            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }

        int rand = Random.Range(0, 2);
        Tile next;
        switch (rand)
        {
            case 0:
                next = map[selected.X + 1, selected.Y];
                break;
            case 1:
                next = map[selected.X + 1, selected.Y+1];
                break;
            default:
                next = map[selected.X, selected.Y + 1];
                break;
        }
        //        Debug.Log("Next point is " + next.X + ", " + next.Y);
        burst--;
        Tile endPoint = burstSE(map, next, burst);
        if (endPoint == null)
        {
            return null;
        }

        selected.Border = 1;
        return endPoint;
    }
    public static Tile burstS(Tile[,] map, Tile selected, int burst)
    {
        if (burst == 0 || detectBorder(map, selected) == true
            || selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            //            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }

        int rand = Random.Range(-1, 2);
        Tile next = map[selected.X + 1, selected.Y + rand];
        //        Debug.Log("Next point is " + next.X + ", " + next.Y);
        burst--;
        Tile endPoint = burstS(map, next, burst);
        if (endPoint == null)
        {
            return null;
        }

        selected.Border = 1;
        return endPoint;
    }
    public static Tile burstSW(Tile[,] map, Tile selected, int burst)
    {
        if (burst == 0 || detectBorder(map, selected) == true
            || selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            //            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }

        int rand = Random.Range(0, 2);
        Tile next;
        switch (rand)
        {
            case 0:
                next = map[selected.X + 1, selected.Y];
                break;
            case 1:
                next = map[selected.X + 1, selected.Y - 1];
                break;
            default:
                next = map[selected.X, selected.Y - 1];
                break;
        }
        //        Debug.Log("Next point is " + next.X + ", " + next.Y);
        burst--;
        Tile endPoint = burstSW(map, next, burst);
        if (endPoint == null)
        {
            return null;
        }

        selected.Border = 1;
        return endPoint;
    }
    public static Tile burstW(Tile[,] map, Tile selected, int burst)
    {
        if (burst == 0 || detectBorder(map, selected) == true
            || selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            //            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }

        int rand = Random.Range(-1, 2);
        Tile next = map[selected.X + rand, selected.Y - 1];
        //        Debug.Log("Next point is " + next.X + ", " + next.Y);
        burst--;
        Tile endPoint = burstW(map, next, burst);
        if (endPoint == null)
        {
            return null;
        }

        selected.Border = 1;
        return endPoint;
    }
    public static Tile burstNW(Tile[,] map, Tile selected, int burst)
    {
        if (burst == 0 || detectBorder(map, selected) == true
            || selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            //            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }

        int rand = Random.Range(0, 2);
        Tile next;
        switch (rand)
        {
            case 0:
                next = map[selected.X - 1, selected.Y];
                break;
            case 1:
                next = map[selected.X - 1, selected.Y - 1];
                break;
            default:
                next = map[selected.X, selected.Y - 1];
                break;
        }
        //        Debug.Log("Next point is " + next.X + ", " + next.Y);
        burst--;
        Tile endPoint = burstNW(map, next, burst);
        if (endPoint == null)
        {
            return null;
        }

        selected.Border = 1;
        return endPoint;
    }

    // Finds the previous border tile
    public static Tile findPrev(Tile[,] tiles, Tile awayTile)
    {
        Tile prevTile = awayTile;

        if (tiles[awayTile.X - 1, awayTile.Y].Border == 1)
        {
            prevTile = tiles[awayTile.X - 1, awayTile.Y];
        }
        if (tiles[awayTile.X + 1, awayTile.Y].Border == 1)
        {
            prevTile = tiles[awayTile.X + 1, awayTile.Y];
        }
        if (tiles[awayTile.X, awayTile.Y - 1].Border == 1)
        {
            prevTile = tiles[awayTile.X, awayTile.Y - 1];
        }
        if (tiles[awayTile.X, awayTile.Y + 1].Border == 1)
        {
            prevTile = tiles[awayTile.X, awayTile.Y + 1];
        }

        if (tiles[awayTile.X - 1, awayTile.Y - 1].Border == 1)
        {
            prevTile = tiles[awayTile.X - 1, awayTile.Y - 1];
        }
        if (tiles[awayTile.X + 1, awayTile.Y - 1].Border == 1)
        {
            prevTile = tiles[awayTile.X + 1, awayTile.Y - 1];
        }
        if (tiles[awayTile.X - 1, awayTile.Y + 1].Border == 1)
        {
            prevTile = tiles[awayTile.X - 1, awayTile.Y + 1];
        }
        if (tiles[awayTile.X + 1, awayTile.Y + 1].Border == 1)
        {
            prevTile = tiles[awayTile.X + 1, awayTile.Y + 1];
        }

        return prevTile;
    }

    // Moves the start point away from the coastline to guarentee some space. Returns the last prospective tile
    public static Tile coastEscape(Tile[,] tiles, Tile startTile)
    {
        int escapeVal = Map.height / 20;
//        Debug.Log("Escape val = "+escapeVal);

        if (tiles[startTile.X + 1, startTile.Y].Biome == Biome.Ocean)
        {
            Debug.Log("Escape North");
            return escapeNorth(tiles, tiles[startTile.X - 1, startTile.Y], escapeVal);
        }
        if (tiles[startTile.X - 1, startTile.Y].Biome == Biome.Ocean)
        {
            Debug.Log("Escape South");
            return escapeSouth(tiles, tiles[startTile.X + 1, startTile.Y], escapeVal);
        }
        if (tiles[startTile.X, startTile.Y - 1].Biome == Biome.Ocean)
        {
            Debug.Log("Escape East");
            return escapeEast(tiles, tiles[startTile.X, startTile.Y + 1], escapeVal);
        }
        if (tiles[startTile.X, startTile.Y + 1].Biome == Biome.Ocean)
        {
            Debug.Log("Escape West");
            return escapeWest(tiles, tiles[startTile.X, startTile.Y - 1], escapeVal);
        }

        return startTile;
    }
    public static Tile escapeNorth(Tile[,] tiles, Tile selected, int escapeVal)
    {
        if (escapeVal == 0)
        {
//            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }
        if (detectBorder(tiles, selected) == true || detectOcean(tiles, selected) > 0)
        {
//            Debug.Log("Away point invalid");

            return null;
        }

        int rand = Random.Range(-1, 2);
        Tile next = tiles[selected.X - 1, selected.Y + rand];
        while (next.Biome == Biome.Ocean)
        {
            rand = Random.Range(-1, 2);
            next = tiles[selected.X - 1, selected.Y + rand];
        }
//        Debug.Log("Next point is " + next.X + ", " + next.Y);
        escapeVal--;
        Tile endPoint = escapeNorth(tiles, next, escapeVal);
        if (endPoint == null)
        {
            return null;
        }

        //selected.Border = 1;
        return endPoint;
    }
    public static Tile escapeSouth(Tile[,] tiles, Tile selected, int escapeVal)
    {
        if (escapeVal == 0)
        {
//            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }
        if (detectBorder(tiles, selected) == true || detectOcean(tiles, selected) > 0)
        {
//            Debug.Log("Away point invalid");

            return null;
        }

        int rand = Random.Range(-1, 2);
        Tile next = tiles[selected.X + 1, selected.Y + rand];
        while (next.Biome == Biome.Ocean)
        {
            rand = Random.Range(-1, 2);
            next = tiles[selected.X + 1, selected.Y + rand];
        }
//        Debug.Log("Next point is " + next.X + ", " + next.Y);
        escapeVal--;
        Tile endPoint = escapeSouth(tiles, next, escapeVal);
        if (endPoint == null)
        {
            return null;
        }

        //selected.Border = 1;
        return endPoint;
    }
    public static Tile escapeEast(Tile[,] tiles, Tile selected, int escapeVal)
    {
        if (escapeVal == 0)
        {
//            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }
        if (detectBorder(tiles, selected) == true || detectOcean(tiles, selected) > 0)
        {
//            Debug.Log("Away point invalid");

            return null;
        }

        int rand = Random.Range(-1, 2);
        Tile next = tiles[selected.X + rand, selected.Y + 1];
        while (next.Biome == Biome.Ocean)
        {
            rand = Random.Range(-1, 2);
            next = tiles[selected.X + rand, selected.Y + 1];
        }
//        Debug.Log("Next point is " + next.X + ", " + next.Y);
        escapeVal--;
        Tile endPoint = escapeEast(tiles, next, escapeVal);
        if (endPoint == null)
        {
            return null;
        }

        //selected.Border = 1;
        return endPoint;
    }
    public static Tile escapeWest(Tile[,] tiles, Tile selected, int escapeVal)
    {
        if (escapeVal == 0)
        {
//            Debug.Log("Away point found at " + selected.X + ", " + selected.Y);
            selected.Border = 1;
            return selected;
        }
        if (detectBorder(tiles, selected) == true || detectOcean(tiles, selected) > 0)
        {
//            Debug.Log("Away point invalid");

            return null;
        }

        int rand = Random.Range(-1, 2);
        Tile next = tiles[selected.X + rand, selected.Y - 1];
        while (next.Biome == Biome.Ocean)
        {
            rand = Random.Range(-1, 2);
            next = tiles[selected.X + rand, selected.Y - 1];
        }
//        Debug.Log("Next point is " + next.X + ", " + next.Y);
        escapeVal--;
        Tile endPoint = escapeWest(tiles, next, escapeVal);
        if (endPoint == null)
        {
            return null;
        }

        //selected.Border = 1;
        return endPoint;
    }

    public static Tile genStartTile(Tile[,] tiles)
    {
//        Debug.Log(Map.height);
//        Debug.Log(Map.width);

        int startX = Random.Range(Map.width / 20, (Map.width - Map.width / 20));
        int startY = Random.Range(Map.height / 20, (Map.height - Map.height / 20));

//        Debug.Log(startX);
//        Debug.Log(startY);

        ref Tile startTile = ref tiles[startX, startY];
        //Debug.Log(side);

        if(startTile.Biome != Biome.Ocean)
        {
            if( escapeEast(tiles, startTile, startX) == null 
                && escapeWest(tiles, startTile, startX) == null 
                && escapeNorth(tiles, startTile, startY) == null 
                && escapeSouth(tiles, startTile, startY) != null)
            {
                return null;
            }
        }

        //Debug.Log("Start tile X:"+startTile.X);
        int oceans = detectOcean(tiles, startTile);
        if (startTile.Border == 2 || oceans > 0 || detectBorder(tiles, startTile) == true)
        {
//            Debug.Log("Start point invalid");
            return null;
        }

//        Debug.Log("Start point found at " + startTile.X + ", " + startTile.Y);
        startTile.Border = 1;

        GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        s.transform.position = new Vector3(startTile.X, (startTile.Elevation / 10) + 2, startTile.Y);
        s.GetComponent<Renderer>().material.color = new Color(1, 1, 0, 1);

        return startTile;
    }

    // Checks a tile's cardinal direction neighbors for the ocean. Returns number of ocean tiles located
    private static int detectOcean(Tile[,] tiles, Tile selected)
    {
        int ocean = 0;

        if(tiles[selected.X-1, selected.Y].Biome == Biome.Ocean)
        {
            ocean += 1;
        }
        if (tiles[selected.X + 1, selected.Y].Biome == Biome.Ocean)
        {
            ocean += 1;
        }
        if (tiles[selected.X, selected.Y-1].Biome == Biome.Ocean)
        {
            ocean += 1;
        }
        if (tiles[selected.X, selected.Y+1].Biome == Biome.Ocean)
        {
            ocean += 1;
        }

        return ocean;
    }

    // Checks a tile's neighbors for an established border tile. Returns true if one is found.
    private static bool detectBorder(Tile[,] tiles, Tile selected)
    {
        if( selected.X <= 0
            || selected.X >= Map.width - 1
            || selected.Y >= Map.height - 1
            || selected.Y <= 0)
        {
            return true;
        }

        if (tiles[selected.X - 1, selected.Y].Border == 2)
        {
            return true;
        }
        if (tiles[selected.X + 1, selected.Y].Border == 2)
        {
            return true;
        }
        if (tiles[selected.X, selected.Y - 1].Border == 2)
        {
            return true;
        }
        if (tiles[selected.X, selected.Y + 1].Border == 2)
        {
            return true;
        }

        if (tiles[selected.X - 1, selected.Y - 1].Border == 2)
        {
            return true;
        }
        if (tiles[selected.X + 1, selected.Y -1].Border == 2)
        {
            return true;
        }
        if (tiles[selected.X - 1, selected.Y + 1].Border == 2)
        {
            return true;
        }
        if (tiles[selected.X + 1, selected.Y + 1].Border == 2)
        {
            return true;
        }

        return false;
    }

    // Creates a marker at Tile's location
    private static void genBorderSphere(Tile selected)
    {
        if(selected.Biome == Biome.Ocean)
        {
            return;
        }
        GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        s.transform.position = new Vector3(selected.X, (selected.Elevation/10)+1, selected.Y);
        s.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1);
    }
}
