using System;
using System.Collections.Generic;
using System.Linq;

namespace NineBitByte.Assets.Editor.AllAssetsWindowEditor
{
  public interface INode
  {
    string Name { get; }

    string FullPath { get; }

    void Draw(AssetWindow window);
  }
}