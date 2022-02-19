using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrain : MonoBehaviour
{
	//height y, width x
    public static void createInitialMountains(Tile[,] tiles, int width, int height){
		List<Tile> unsetTiles = new List<Tile>();
		Tile thisTile;
		int initialX; 
		int initialY; 
		//Debug.Log("x: " + initialX.ToString() + "    y: " + initialY.ToString());
		
		for(int i = 0; i < 5; i++) {
			initialX = Random.Range((int)(width*.25), (int)(width*.75)); 
			initialY = Random.Range((int)(height*.25), (int)(height*.75));
			thisTile = tiles[initialX, initialY];
			if(thisTile.Elevation < 0){
				setMountainRange(thisTile, Random.Range(18, 25));
			}
		}	
		
	}
	
	public static Tile[] getNeighbors(Tile center){
		List<Tile> unsetNeighbors = new List<Tile>();
		if(center.left.Elevation < 0){
			unsetNeighbors.Add(center.left);
		}
		if(center.up.Elevation < 0){
			unsetNeighbors.Add(center.up);
		}
		if(center.right.Elevation < 0){
			unsetNeighbors.Add(center.right);
		}
		if(center.down.Elevation < 0){
			unsetNeighbors.Add(center.down);
		}
		
		return unsetNeighbors.ToArray();
	}
	
	private static void setMountainRange(Tile thisTile, int rangeLength){
		Tile[] neighbors = getNeighbors(thisTile);
		thisTile.Elevation = Random.Range(60f, 65f);
		if(rangeLength > 0 & neighbors.Length > 0){
			int index = Random.Range((int)0, (int)neighbors.Length);
			for(int i = 0; i < neighbors.Length; i++){
				Tile nextTile = neighbors[index];
				if(getNeighbors(nextTile).Length >= 3){
					setMountainRange(nextTile, rangeLength - 1);
					break;
				} else {
					index++;
					if(index >= neighbors.Length){
						index = 0;
					}
				}
			}
		}
	}
	
	
}
