using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintTerrain : MonoBehaviour
{
    [System.Serializable]
    public class SplatHeights
    {
        public int textureIndex;
        public int startingHeight;
    }

    public Terrain TerrainMain;

    //public SplatHeights[] splatHeights;

    public void StartPaint()
    {
        TerrainData terrainData = TerrainMain.terrainData;

        var splatHeights = new SplatHeights[]
        {
            new SplatHeights() { textureIndex = 0, startingHeight = 0 },
            new SplatHeights() { textureIndex = 1, startingHeight = (int)(HeightMapProvider.SandLevel * terrainData.size.y) },
            new SplatHeights() { textureIndex = 2, startingHeight = (int)(HeightMapProvider.MountainLevel * terrainData.size.y) },
            new SplatHeights() { textureIndex = 3, startingHeight = (int)(HeightMapProvider.RockLevel * terrainData.size.y) },
        };

        float[,,] splatmapData = new float[terrainData.alphamapWidth, 
                                           terrainData.alphamapHeight, 
                                           terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float terrainHeight = terrainData.GetHeight(y, x);

                float[] splat = new float[splatHeights.Length];

                for (int i = 0; i < splatHeights.Length; i++)
                {
                    if (i == splatHeights.Length - 1 && terrainHeight >= splatHeights[i].startingHeight)
                    {
                        splat[i] = 1;
                    }
                    else if (terrainHeight >= splatHeights[i].startingHeight && 
                        terrainHeight <= splatHeights[i+1].startingHeight)
                    {
                        splat[i] = 1;
                    }
                }

                for (int j = 0; j < splatHeights.Length; j++)
                {
                    splatmapData[x, y, j] = splat[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
}
