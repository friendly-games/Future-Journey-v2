using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.Common;
using NineBitByte.FutureJourney.Items;
using NineBitByte.FutureJourney.Programming;
using UnityEngine;

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
          int rotationIndex = UnityEngine.Random.Range(0, 4);

          AvailableTiles[0].Construct(
            new PositionAndRotation(new Vector3(x, y, 0), possibleRotations[rotationIndex])
          );
        }
      }

    }

    public void Update()
    {
    }
  }
}