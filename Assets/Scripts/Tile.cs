using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsTowerBuilt{ get; set; }
    public GameObject OwnTower { get; set; }
    private void Awake()
    {
        IsTowerBuilt = false;
        OwnTower = null;
    }
}
