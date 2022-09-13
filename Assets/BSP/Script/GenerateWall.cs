using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateWall
{
    public static void MakeWall(GameObject WallPrefab, List<Vector2> verticalL,List<Vector2> verticalR, List<Vector2> horizontalU,List<Vector2> horizontalD,GameObject room)
    {
        // vertical 일 때 rotation Y축으로 90 , position x=-1
        for(int i = 0; i<verticalL.Count; i++)
        {
            GameObject.Instantiate(WallPrefab, new Vector3(verticalL[i].x , 0, verticalL[i].y+1), Quaternion.Euler(0,-90,0), room.transform.parent).transform.parent=room.transform;
        }
        for (int i = 0; i < verticalR.Count; i++)
        {
            GameObject.Instantiate(WallPrefab, new Vector3(verticalR[i].x, 0, verticalR[i].y+1), Quaternion.Euler(0, 270, 0), room.transform.parent).transform.parent = room.transform;
        }
        for (int i = 0; i < horizontalU.Count; i++)
        {
            GameObject.Instantiate(WallPrefab, new Vector3(horizontalU[i].x+1, 0, horizontalU[i].y),Quaternion.Euler(0,0,0), room.transform.parent).transform.parent = room.transform;
        }
        for (int i = 0; i < horizontalD.Count; i++)
        {
            GameObject.Instantiate(WallPrefab, new Vector3(horizontalD[i].x+1, 0, horizontalD[i].y), Quaternion.Euler(0, 0, 0), room.transform.parent).transform.parent = room.transform;
        }
    }
    
}
