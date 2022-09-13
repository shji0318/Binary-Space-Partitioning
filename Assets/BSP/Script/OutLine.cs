using System.Collections.Generic;
using UnityEngine;
using System;
public class OutLine
{
    List<Vector2> _verticalLineL = new List<Vector2>();
    List<Vector2> _verticalLineR = new List<Vector2>();
    List<Vector2> _horizontalLineU = new List<Vector2>();
    List<Vector2> _horizontalLineD = new List<Vector2>();
    public List<Vector2> VerticalLineL { get => _verticalLineL; set => _verticalLineL = value; }
    public List<Vector2> VerticalLineR { get => _verticalLineR; set => _verticalLineR = value; }
    public List<Vector2> HorizontalLineU { get => _horizontalLineU; set => _horizontalLineU = value; }
    public List<Vector2> HorizontalLineD { get => _horizontalLineD; set => _horizontalLineD = value; }


    public OutLine(Square room)
    {
        for(int i =0; i<room.GetVerticalDistance; i++)
        {
            VerticalLineL.Add(new Vector2(room.DownLeft.x, room.DownLeft.y + i));// 왼쪽 벽 라인
            VerticalLineR.Add(new Vector2(room.DownRight.x, room.DownRight.y + i));// 오른쪽 벽 라인
        }
        for(int i = 0; i<room.GetHorizontalDistance;i++)
        {
            HorizontalLineD.Add(new Vector2(room.DownLeft.x + i,  room.DownLeft.y));//아래쪽 벽 라인
            HorizontalLineU.Add(new Vector2(room.UpLeft.x + i,  room.UpLeft.y));//위쪽 벽 라인
        }
        
    }

   
    public  void DeleteBridgeWall(Bridge bridge) // ROoms between bridge's wall delete
    {
        Room room1 = MapCreator.Map.RoomInfo[bridge.Connect[0]].Room;
        Room room2 = MapCreator.Map.RoomInfo[bridge.Connect[1]].Room;

        
        if(!bridge.HowDivide)
        {
            int maxX1 = (int)Math.Max(room1.DownLeft.x, bridge.UpLeft.x);
            int minX1 = (int)Math.Min(room1.DownRight.x, bridge.UpRight.x);
            int maxX2 = (int)Math.Max(room2.UpLeft.x, bridge.DownLeft.x);
            int minX2 = (int)Math.Min(room2.UpRight.x, bridge.DownRight.x);

            for(int i= maxX1; i<minX1;i++)
            {
                room1.OutLine.HorizontalLineD.Remove(new Vector2(i, room1.DownLeft.y));
                bridge.OutLine.HorizontalLineU.Remove(new Vector2(i, bridge.UpLeft.y));
            }
            for(int i = maxX2; i<minX2; i++)
            {
                room2.OutLine.HorizontalLineU.Remove(new Vector2(i, room2.UpLeft.y));
                bridge.OutLine.HorizontalLineD.Remove(new Vector2(i, bridge.DownLeft.y));
            }
        }
        else
        {
            int maxY1 = (int)Math.Max(room1.DownRight.y, bridge.DownLeft.y);
            int minY1 = (int)Math.Min(room1.UpRight.y, bridge.UpLeft.y);
            int maxY2 = (int)Math.Max(room2.DownLeft.y, bridge.DownRight.y);
            int minY2 = (int)Math.Min(room2.UpLeft.y, bridge.UpRight.y);
            for(int i = maxY1; i<minY1;i++)
            {
                room1.OutLine.VerticalLineR.Remove(new Vector2(room1.DownRight.x, i));
                bridge.OutLine.VerticalLineL.Remove(new Vector2(bridge.DownLeft.x, i));
            }
            for(int i = maxY2; i<minY2;i++)
            {
                room2.OutLine.VerticalLineL.Remove(new Vector2(room2.DownLeft.x, i));
                bridge.OutLine.VerticalLineR.Remove(new Vector2(bridge.DownRight.x, i));
            }
        }
            

    }
    
}