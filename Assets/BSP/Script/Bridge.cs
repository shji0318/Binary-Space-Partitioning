using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Square
{
    OutLine _outLine;
    bool _howDivide;
    int[] _connect;

    public bool HowDivide { get => _howDivide; set => _howDivide = value; }
    public int[] Connect { get => _connect; set => _connect = value; }
    public OutLine OutLine { get => _outLine; set => _outLine = value; }

    public Bridge(Room leftRoom, Room rightRoom, bool howDivide)//howDivide false 위아래, true 좌우
    {
        HowDivide = howDivide;
        Connect = new int[] {leftRoom.RoomNum, rightRoom.RoomNum };
        if(!howDivide)
        {
            int maxLeftX = (int)Math.Max(leftRoom.DownLeft.x, rightRoom.UpLeft.x);
            int minRightX = (int)Math.Min(leftRoom.DownRight.x, rightRoom.UpRight.x);
            int avgX = (int)Math.Abs((minRightX-maxLeftX))/2;
            UpLeft = new Vector2(maxLeftX+avgX-2, leftRoom.DownLeft.y);
            UpRight = new Vector2(maxLeftX + avgX+2, leftRoom.DownLeft.y);
            DownLeft = new Vector2(maxLeftX + avgX-2, rightRoom.UpLeft.y);
            DownRight = new Vector2(maxLeftX + avgX+2, rightRoom.UpLeft.y);
            
        }
        else
        {
            int minUpY = (int)(Math.Min(leftRoom.UpRight.y, rightRoom.UpLeft.y));
            int maxDownY = (int)(Math.Max(leftRoom.DownRight.y, rightRoom.DownLeft.y));
            int avgY = (int)Math.Abs(minUpY-maxDownY)/2;
            UpLeft = new Vector2(leftRoom.DownRight.x, maxDownY+avgY+2);
            UpRight = new Vector2(rightRoom.DownLeft.x, maxDownY+avgY+2);
            DownLeft = new Vector2(leftRoom.DownRight.x, maxDownY+avgY-2);
            DownRight = new Vector2(rightRoom.DownLeft.x, maxDownY+avgY-2);
            
        }
    }

 

    public void MakeOutLine()
    {
        OutLine = new OutLine(this);
    }
}
