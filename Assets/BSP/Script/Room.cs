using System;
using UnityEngine;
using System.Collections.Generic;

public class Room : Square
{
    
    OutLine _outLine;
    int _roomNum;
     
    public int RoomNum { get => _roomNum; set => _roomNum = value; }
    public OutLine OutLine { get => _outLine; set => _outLine = value; }
    

    public Room(Vector2 downLeft, Vector2 downRight, Vector2 upLeft, Vector2 upRight)
    {
        _downLeft = downLeft;
        _downRight = downRight;
        _upLeft = upLeft;
        _upRight = upRight;
    }  

    public void ReduceRoom(int rate) // 랜덤한 비율로 공간 내에 범위 축소
    {
        int v_reduceDis = GetVerticalDistance - (int)(GetVerticalDistance * rate / 10);
        int h_redueceDis = GetHorizontalDistance - (int)(GetHorizontalDistance * rate / 10);

        _downLeft.x += h_redueceDis;
        _downLeft.y += v_reduceDis;
        _downRight.x -= h_redueceDis;
        _downRight.y += v_reduceDis;
        _upLeft.x += h_redueceDis;
        _upLeft.y -= v_reduceDis;
        _upRight.x -= h_redueceDis;
        _upRight.y -= v_reduceDis;
            
    }

    public void MakeOutLine()
    {
        OutLine = new OutLine(this);
    }
}

