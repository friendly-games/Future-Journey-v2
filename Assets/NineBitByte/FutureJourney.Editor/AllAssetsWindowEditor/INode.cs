using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.FutureJourney.Editor.AllAssetsWindowEditor
{
  public interface INode
  {
    string Name { get; }

    string RelativePath { get; }

    void Draw(AssetWindow window);
  }
}