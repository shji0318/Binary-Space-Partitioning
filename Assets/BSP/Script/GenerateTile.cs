using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateTile
{
    
    public static void MakeTile(Square room,List<GameObject> tilePrefab)
    {
        int count = tilePrefab.Count;
        int select = 0;
        for (int i = (int) room.DownLeft.y;i<(int)room.UpLeft.y;i++) //vertical
        {
            for (int j = (int)room.DownLeft.x; j < (int)room.DownRight.x; j++)// horizontal
            {
                GameObject go = GameObject.Instantiate(tilePrefab[select],new Vector3(j+1,0,i),Quaternion.Euler(0,0,0), room.TileGo.transform);                
                
            }
        }
        
    }
    
}
