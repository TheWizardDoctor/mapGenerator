using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateTerrain : MonoBehaviour
{
	//height y, width x
    public static List<Tile> createInitialMountains(Tile[,] tiles, int width, int height){
		List<Tile> unsetTiles = new List<Tile>();
		Tile thisTile;
		int initialX; 
		int initialY; 
		//Debug.Log("x: " + initialX.ToString() + "    y: " + initialY.ToString());
		for(int i = 0; i < 5; i++) {
			initialX = Random.Range((int)(width*.25), (int)(width*.75)); 
			initialY = Random.Range((int)(height*.25), (int)(height*.75));
			thisTile = tiles[initialX, initialY];
			if(thisTile.Elevation <= 0){
				foreach(Tile t in getNeighbors(thisTile)){
					if(!unsetTiles.Contains(t) & t.Elevation <= 0){
						t.previous = thisTile;
						unsetTiles.Add(t);
					}
				}
				unsetTiles = setMountainRange(unsetTiles, tiles, thisTile, Random.Range(18, 25));
			}
		}	
		/*
		Debug.Log("count: " + unsetTiles.Count.ToString());

		foreach (Tile t in unsetTiles.ToArray()){
			t.Elevation = 30;
			Debug.Log("x: " + t.X.ToString() + "    y: " + t.Y.ToString());
			unsetTiles.Remove(t);
		}
		
		*/	
		return unsetTiles;
	}
	
	private static List<Tile> setMountainRange(List<Tile> unsetTiles, Tile[,] tiles, Tile thisTile, int rangeLength){
		List<Tile> neighbors = getNeighbors(thisTile);
		thisTile.Elevation = Random.Range(55f, 60f);
		unsetTiles.Remove(thisTile);
		if(rangeLength > 0 & neighbors.Count > 0){
			int index = Random.Range((int)0, (int)neighbors.Count);
			for(int i = 0; i < neighbors.Count; i++){
				Tile nextTile = neighbors[index];
				List<Tile> nextNeighbors = getNeighbors(nextTile); 
				if(nextNeighbors.Count >= 3){
					foreach(Tile t in nextNeighbors){
						if(!unsetTiles.Contains(t) & t.Elevation <= 0){
							t.previous = thisTile;
							unsetTiles.Add(t);
						}
					}
					return setMountainRange(unsetTiles, tiles, nextTile, rangeLength - 1);
				} else {
					index++;
					if(index >= neighbors.Count){
						index = 0;
					}
				}
			}
		}
		return unsetTiles;
	}
	
	public static void setElevations(List<Tile> unsetTiles){
		int scale = 1;
		Tile thisTile = unsetTiles[0];
		unsetTiles.Remove(thisTile);
		List<Tile> neighbors = getNeighbors(thisTile);
		foreach(Tile t in neighbors){
			if(!unsetTiles.Contains(t) & t.Elevation <= 0){
				t.previous = thisTile;
				unsetTiles.Add(t);
			}
		}
		List<float> slopes = new List<float>();
		slopes.Add(estSlope(thisTile));
		Tile prevTile = thisTile.previous;
		slopes.Add(calculateSlopeLeft(prevTile));
		slopes.Add(calculateSlopeLeft(prevTile));
		slopes.Add(calculateSlopeUp(prevTile));
		slopes.Add(calculateSlopeDown(prevTile));
		float averageSlope = averageSlopes(slopes, slopes.Count);
		float elevation = prevTile.Elevation;
		elevation = averageSlope * scale + elevation;
		elevation = Random.Range(elevation - 7.5f, elevation + 2.5f);
		if(elevation <= 0){
			elevation = 0;
		}
		if(elevation >= 70){
			elevation = 70;
		}
		thisTile.Elevation = elevation;
	}
	
	private static float estSlope(Tile thisTile){
		Tile prevTile = thisTile.previous;
		double e = prevTile.Elevation;
		e = 0.5 + 0.044*e - System.Math.Pow(0.000*e, 2) - System.Math.Pow(0.00001*e, 3);
		float slope = (float)e;
		if(float.IsNaN(slope)){
			slope = 0;
		}
		return slope * -1;
	}
	
	private static float calculateSlopeLeft(Tile c){
		//calculate slope coming from left going right
		//tile arangement looks like this
		// b1 <-> a1 <-> c <-> a2 <-> b2
		Tile a1 = c.left;
		Tile b1 = a1.left;
		Tile a2 = c.right;
		Tile b2 = a2.right;
		List<float> slopes = new List<float>();
		if(a1.Elevation > 0){
			slopes.Add(pointSlope(0, -1, c.Elevation, a1.Elevation));
		}
		if(b1.Elevation > 0){
			slopes.Add(pointSlope(0, -2, c.Elevation, b1.Elevation));
		}
		if(a2.Elevation > 0){
			slopes.Add(pointSlope(0, 1, c.Elevation, a2.Elevation));
		}
		if(b2.Elevation > 0){
			slopes.Add(pointSlope(0, 2, c.Elevation, b2.Elevation));
		}
		return averageSlopes(slopes, slopes.Count);
	}

	private static float calculateSlopeRight(Tile c){
		//calculate slope coming from right going left
		//tile arangement looks like this
		// b2 <-> a2 <-> c <-> a1 <-> b1
		Tile a1 = c.right;
		Tile b1 = a1.right;
		Tile a2 = c.left;
		Tile b2 = a2.left;
		List<float> slopes = new List<float>();
		if(a1.Elevation > 0){
			slopes.Add(pointSlope(0, 1, c.Elevation, a1.Elevation));
		}
		if(b1.Elevation > 0){
			slopes.Add(pointSlope(0, 2, c.Elevation, b1.Elevation));
		}
		if(a2.Elevation > 0){
			slopes.Add(pointSlope(0, -1, c.Elevation, a2.Elevation));
		}
		if(b2.Elevation > 0){
			slopes.Add(pointSlope(0, -2, c.Elevation, b2.Elevation));
		}
		return averageSlopes(slopes, slopes.Count);
	}
	
	private static float calculateSlopeUp(Tile c){
		//calculate slope coming from up going down
		//tile arangement looks like this
		// b2 <-> a2 <-> c <-> a1 <-> b1
		List<float> slopes = new List<float>();
		if(c.up != null){
			Tile a1 = c.up;
			if(a1.Elevation > 0){
				slopes.Add(pointSlope(0, 1, c.Elevation, a1.Elevation));
			}
			if(a1.up != null){
				Tile b1 = a1.up;
				if(b1.Elevation > 0){
					slopes.Add(pointSlope(0, 2, c.Elevation, b1.Elevation));
				}
			}
		}
		if(c.down != null){
			Tile a2 = c.down;
			if(a2.Elevation > 0){
				slopes.Add(pointSlope(0, -1, c.Elevation, a2.Elevation));
			}
			if(a2.down != null){
				Tile b2 = a2.down;
				if(b2.Elevation > 0){
					slopes.Add(pointSlope(0, -2, c.Elevation, b2.Elevation));
				}
			}
		}
		return averageSlopes(slopes, slopes.Count);
	}
	
	private static float calculateSlopeDown(Tile c){
		//calculate slope coming from left going right
		//tile arangement looks like this
		// b1 <-> a1 <-> c <-> a2 <-> b2
		List<float> slopes = new List<float>();
		if(c.up != null){
			Tile a2 = c.up;
			if(a2.Elevation > 0){
				slopes.Add(pointSlope(0, 1, c.Elevation, a2.Elevation));
			}
			if(a2.up != null){
				Tile b2 = a2.up;
				if(b2.Elevation > 0){
					slopes.Add(pointSlope(0, 2, c.Elevation, b2.Elevation));
				}
			}
		}
		if(c.down != null){
			Tile a1 = c.down;
			if(a1.Elevation > 0){
				slopes.Add(pointSlope(0, -1, c.Elevation, a1.Elevation));
			}
			if(a1.down != null){
				Tile b1 = a1.down;
				if(b1.Elevation > 0){
					slopes.Add(pointSlope(0, -2, c.Elevation, b1.Elevation));
				}
			}
		}
		return averageSlopes(slopes, slopes.Count);
	}
	
	private static float pointSlope(float x1, float x2, float y1, float y2){
		float m = (y2 - y1)/(x2 - x1);
		return m;
	}
	
	private static float averageSlopes(List<float> slopes, int count){
		float sum = 0;
		if(count != 0){
			foreach(float f in slopes){
				sum += f;
			}
			return (sum/count);
		} else {
			return 0;
		}
	}
	
	public static List<Tile> getNeighbors(Tile center){
		List<Tile> unsetNeighbors = new List<Tile>();
		if(center.left != null){
			if(center.left.Elevation < 0){
				unsetNeighbors.Add(center.left);
			}
		}
		if(center.up != null){
			if(center.up.Elevation < 0){
				unsetNeighbors.Add(center.up);
			}
		}
		if(center.right != null){
			if(center.right.Elevation < 0){
				unsetNeighbors.Add(center.right);
			}
		}
		if(center.down != null){
			if(center.down.Elevation < 0){
				unsetNeighbors.Add(center.down);
			}
		}
		
		return unsetNeighbors;
	}
	
}
