using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public int TotalCapybaraSpawnCount;
    public Vector3 initPlayerPosition;
    public GameObject mapPrefab;
}
