using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateBiomes : MonoBehaviour
{
    public static List<Tile> createInitialWetZones(){
		List<Tile> unsetTiles = new List<Tile>();
		Tile thisTile;
		int initialX; 
		int initialY; 
		//Debug.Log("x: " + initialX.ToString() + "    y: " + initialY.ToString());
		int r = Random.Range(30, 50);
		for(int i = 0; i < r; i++) {
			initialX = Random.Range((int)(Map.width*.1), (int)(Map.width*.9)); 
			initialY = Random.Range((int)(Map.height*.1), (int)(Map.height*.9));
			thisTile = Map.tiles[initialX, initialY];
			if(thisTile.Precipitation <= 0){
				foreach(Tile t in getNeighbors(thisTile)){
					if(!unsetTiles.Contains(t) & t.Precipitation <= 0){
						t.previous = thisTile;
						unsetTiles.Add(t);
					}
				}
				unsetTiles = setWetZone(unsetTiles, thisTile, Random.Range(30, 45));
			}
		}	
				
		return unsetTiles;
	}
	
	private static List<Tile> setWetZone(List<Tile> unsetTiles, Tile thisTile, int rangeLength){
		List<Tile> neighbors = getNeighbors(thisTile);
		thisTile.Precipitation = Random.Range(250f, 275);
		unsetTiles.Remove(thisTile);
		if(rangeLength > 0 & neighbors.Count > 0){
			int index = Random.Range((int)0, (int)neighbors.Count);
			for(int i = 0; i < neighbors.Count; i++){
				Tile nextTile = neighbors[index];
				List<Tile> nextNeighbors = getNeighbors(nextTile); 
				if(nextNeighbors.Count >= 3){
					foreach(Tile t in nextNeighbors){
						if(!unsetTiles.Contains(t) & t.Precipitation <= 0){
							t.previous = thisTile;
							unsetTiles.Add(t);
						}
					}
					return setWetZone(unsetTiles, nextTile, rangeLength - 1);
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
	
	public static void setPrecipitations(List<Tile> unsetTiles){
		int scale = 1;
		int rand = Random.Range((int)0, (int)unsetTiles.Count);
		
		Tile thisTile = unsetTiles[rand];
		unsetTiles.RemoveAt(rand);
		List<Tile> neighbors = getNeighbors(thisTile);
		foreach(Tile t in neighbors){
			if(!unsetTiles.Contains(t) & t.Precipitation <= 0){
				t.previous = thisTile;
				unsetTiles.Add(t);
			}
		}
		List<float> slopes = new List<float>();
		thisTile.Precipitation = estSlope(thisTile);
		
		slopes.Add(calculateSlopeLeft(thisTile));
		slopes.Add(calculateSlopeRight(thisTile));
		
		slopes.Add(calculateSlopeUp(thisTile));
		slopes.Add(calculateSlopeDown(thisTile));
		
		//Debug.Log("Est: " + estSlope(thisTile).ToString() + "	Left: " + .ToString() + "	Right: " + .ToString() + "	Up: " + .ToString() + "	Down: " + .ToString() + calculateSlopeDown(prevTile).ToString());
		float averageSlope = averageSlopes(slopes, slopes.Count);
		float Precipitation = thisTile.Precipitation;
		Precipitation = averageSlope * scale + Precipitation;
		//float wobble = Precipitation + (float)System.Math.Pow(6, 1 - 0.0025 * Precipitation) - 3.5f;
		Precipitation = Random.Range(Precipitation - 10f, Precipitation + 10f);
		
		if(Precipitation <= -10){
			Precipitation = -10;
		}
		if(Precipitation >= 300){
			Precipitation = 300;
		}
		thisTile.Precipitation = Precipitation;
	}
	
	private static float estSlope(Tile thisTile){
		Tile prevTile = thisTile.previous;
		double e = prevTile.Precipitation;
		e = 0.95*e - System.Math.Pow(0.00003*e, 2);
		float newPrecipitation = (float)e;
		if(float.IsNaN(newPrecipitation)){
			newPrecipitation = 0;
		}
		//Debug.Log("y1: " + prevTile.Precipitation.ToString() + "    y2: " + newPrecipitation.ToString());
		return newPrecipitation;
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
		if(a1.Precipitation > 0){
			slopes.Add(pointSlope(0, c.Precipitation, 1, a1.Precipitation));
		}
		if(b1.Precipitation > 0){
			slopes.Add(pointSlope(0, c.Precipitation, 2, b1.Precipitation));
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
		if(a1.Precipitation > 0){
			slopes.Add(pointSlope(0, c.Precipitation, 1, a1.Precipitation));
		}
		if(b1.Precipitation > 0){
			slopes.Add(pointSlope(0, c.Precipitation, 2, b1.Precipitation));
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
			if(a1.Precipitation > 0){
				slopes.Add(pointSlope(0, c.Precipitation, 1, a1.Precipitation));
			}
			if(a1.up != null){
				Tile b1 = a1.up;
				if(b1.Precipitation > 0){
					slopes.Add(pointSlope(0, c.Precipitation, 2, b1.Precipitation));
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
		if(c.down != null){
			Tile a1 = c.down;
			if(a1.Precipitation > 0){
				slopes.Add(pointSlope(0, c.Precipitation, 1, a1.Precipitation));
			}
			if(a1.down != null){
				Tile b1 = a1.down;
				if(b1.Precipitation > 0){
					slopes.Add(pointSlope(0, c.Precipitation, 2, b1.Precipitation));
				}
			}
		}
		return averageSlopes(slopes, slopes.Count);
	}
	
	private static float pointSlope(float x1, float y1, float x2, float y2){
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
			if(center.left.Precipitation <= 0){
				unsetNeighbors.Add(center.left);
			}
		}
		if(center.up != null){
			if(center.up.Precipitation <= 0){
				unsetNeighbors.Add(center.up);
			}
		}
		if(center.right != null){
			if(center.right.Precipitation <= 0){
				unsetNeighbors.Add(center.right);
			}
		}
		if(center.down != null){
			if(center.down.Precipitation <= 0){
				unsetNeighbors.Add(center.down);
			}
		}
		
		return unsetNeighbors.OrderBy(t => Random.Range(0, 10)).ToList();
	}
	
}
