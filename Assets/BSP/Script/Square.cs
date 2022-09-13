using UnityEngine;
using System;
public class Square
{
    GameObject _go;
    GameObject tileGo;

    protected Vector2 _downLeft;
    protected Vector2 _downRight;
    protected Vector2 _upLeft;
    protected Vector2 _upRight;

    public Vector2 DownLeft { get => _downLeft; set => _downLeft = value; }
    public Vector2 DownRight { get => _downRight; set => _downRight = value; }
    public Vector2 UpLeft { get => _upLeft; set => _upLeft = value; }
    public Vector2 UpRight { get => _upRight; set => _upRight = value; }

    public GameObject Go { get => _go; set => _go = value; }
    public GameObject TileGo { get => tileGo; set => tileGo = value; }
    public int GetHorizontalDistance { get => (int)Math.Abs(_downRight.x - _downLeft.x); }
    public int GetVerticalDistance { get => (int)Math.Abs(_upLeft.y - _downLeft.y); }
}