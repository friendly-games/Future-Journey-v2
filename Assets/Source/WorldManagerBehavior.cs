using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Items;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NineBitByte
{
  public class WorldManagerBehavior : MonoBehaviour
  {
    public int Width;

    [Tooltip("All of the available tiles for the world generation")]
    public TileType[] AvailableTiles;

    [Tooltip("The layer to which all tiles should be added")]
    public Layer TileLayer;

    public void Start()
    {
      var possibleRotations = new Quaternion[4]
      {
        Quaternion.AngleAxis(0, Vector3.back),
        Quaternion.AngleAxis(90, Vector3.back),
        Quaternion.AngleAxis(180, Vector3.back),
        Quaternion.AngleAxis(270, Vector3.back),
      };

      for (int x = -10; x < 10; x++)
      {
        for (int y = -10; y < 10; y++)
        {
          int rotationIndex = GetRotation(x, y);
          var tileIndex = GetTileIndex(x, y);

          AvailableTiles[tileIndex].Construct(
            new PositionAndRotation(new Vector3(x, y, 0), possibleRotations[rotationIndex])
          );
        }
      }
    }

    private static int GetTileIndex(int x, int y)
    {
      var noise = Mathf.PerlinNoise(
        30 + (10 + x) / 4.0f,
        30 + (10 + y) / 4.0f);
      int tileIndex = noise < 0.70f ? 0 : 1;
      return tileIndex;
    }

    private static int GetRotation(int x, int y)
    {
      var noise = Mathf.PerlinNoise(
        30 + x / 2.0f,
        30 + y / 2.0f);
      return (int)(noise * 4);
    }
  }
}