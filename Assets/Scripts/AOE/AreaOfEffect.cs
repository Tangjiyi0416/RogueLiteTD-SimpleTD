using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName="new Area of effect",menuName="Hex Map/Area of effect")]
public class AreaOfEffect : ScriptableObject
{
    [System.Serializable]
    public class Point{
        [SerializeField]
        private int q,r;

    }
    public List<Point> axialCoordinates = new List<Point>();

}

   
