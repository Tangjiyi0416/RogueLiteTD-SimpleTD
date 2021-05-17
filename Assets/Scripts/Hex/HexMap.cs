using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HexMap
{
    ///<summary>The key (int, int) is the tuple of (coordinate q, coordiante r)</summary>
    //Dictionary is a special data structure that can use a "Key" to access specific item in the storage.
    //In this case, I utilize its "Key" as coordinates for our map, so that we can easily edit the targeted
    //map tile by calling "HexMap.map[(q,r)]".
    //Notice that the "(int, int)" is a "tuple", which is, yet again, a data structure, look it up on Google if you are interested.
    public Dictionary<(int, int), GameObject> map;
    private float hexSize;
    ///<param name="maxWidth">How many tiles a row can fit.</param>
    ///<param name="maxHeight">How many tiles a col can fit.</param>
    public HexMap(int maxWidth, int maxHeight, float hexSize)
    {
        this.hexSize = hexSize;
        map = new Dictionary<(int, int), GameObject>();
        for (int r = 0; r < maxHeight; r++)
        {
            //Because we're using Axial coordinates(rhombus) to store our "semi-kinda-nearly-close-to-rectangular map", we need 
            //to offset the coordinate q to cancel out(offset) the distortion.
            //And we done that by making additional offset every two row(r).
            //"shfit right" is faster than "divided by 2" while keeping the same outcome.
            //Beware the "-" sign, if left without parentheses,it will change the r variable to negative number before shifting,
            //result in weird edge on your map.
            int offset = -(r >> 1);
            for (int q = offset; q < maxWidth + offset; q++)
            {
                map.Add((q, r), new GameObject($"{q}, {r}"));
                map[(q, r)].GetComponent<Transform>().position = GetWorldCoordFromMapCoord(q, r);
            }
        }
    }
    public Vector3 GetWorldCoordFromMapCoord(int mapCoordQ, int mapCoordR)
    {
        return new Vector3((mapCoordQ + mapCoordR / 2f) * 1.7320508f * hexSize, mapCoordR * 1.5f * hexSize);
    }
    public (int, int) GetMapCoordFromWorldCoord(float worldCoordX, float worldCoordY)
    {
        return HexRounding(((worldCoordX * 0.5773502f - 1f / 3 * worldCoordY) / hexSize, 2f / 3 * worldCoordY / hexSize));
    }

    private (int, int) HexRounding((float, float) co)
    {
        int x = Mathf.RoundToInt(co.Item1);
        int y = Mathf.RoundToInt(co.Item2);
        int z = Mathf.RoundToInt(-co.Item1 - co.Item2);

        float diffX = Mathf.Abs(x - co.Item1);
        float diffY = Mathf.Abs(y - co.Item2);
        float diffZ = Mathf.Abs(z + co.Item1 + co.Item2);

        if (diffX > diffY && diffX > diffZ) x = -y - z;
        else if (diffY > diffZ) y = -x - z;
        return (x,y);
    }
}
//customizable size through level editor
//auto snapping
//acts like array
//contents neighborhood info
//world to map & map to world