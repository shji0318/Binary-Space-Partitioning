using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public List<GameObject> wallList = new List<GameObject>();
    public void Awake()
    {
        int ranNum = 0;
        var wallUp = Instantiate(wallList[ranNum], this.gameObject.transform.position + new Vector3(0, 2, 0), this.gameObject.transform.rotation, this.gameObject.transform);
        ranNum = 0;
        var wallMiddle = Instantiate(wallList[ranNum], this.gameObject.transform.position + new Vector3(0, 1, 0), this.gameObject.transform.rotation, this.gameObject.transform);
        ranNum = 0;
        var wallDown = Instantiate(wallList[ranNum], this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform);
    }
}
