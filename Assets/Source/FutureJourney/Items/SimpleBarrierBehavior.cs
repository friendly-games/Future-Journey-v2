using System;
using System.Collections.Generic;
using System.Linq;
using NineBitByte.FutureJourney.Programming;
using NineBitByte.FutureJourney.World;

namespace NineBitByte.FutureJourney.Items
{
  /// <summary> A behavior for a Placable where the object simply acts as a barrier </summary>
  public class SimpleBarrierBehavior : PlaceableBehavior, IDamageReceiver
  {
    private Chunk.GridCellReference _cellReference;

    public override void Initialize(Placeable programming, IOwner owner, GridCoordinate coordinate)
    {
      base.Initialize(programming, owner, coordinate);

      _cellReference = owner.AssociatedGrid[coordinate];
    }

    /// <inheritdoc />
    public int Health
    {
      get { return _cellReference.Get().Health; }
      set { _cellReference.UpdateHealth(value); }
    }

    /// <inheritdoc />
    public void OnHealthDepleted() 
      => _cellReference.ClearObject();
  }
}