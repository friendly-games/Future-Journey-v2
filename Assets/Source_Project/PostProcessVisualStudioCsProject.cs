using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace NineBitByte.Source_Project
{
  public class PostProcessVisualStudioCsProject : AssetPostprocessor
  {
    //public static void OnGeneratedCSProjectFiles()
    //{
    //  // Open the solution file
    //  string projectDirectory = Directory.GetParent(Application.dataPath).FullName;
    //  string projectName = Path.GetFileName(projectDirectory);
    //  var pathProject = Path.Combine(projectDirectory, $"{projectName}.csproj");
    //  var pathClean = Path.Combine(projectDirectory, $"{projectName}.Simple.csproj");
    //  var pathComplex = Path.Combine(projectDirectory, $"{projectName}.Complex.csproj");

    //  try
    //  {
    //    File.Copy(pathProject, pathComplex, true);
    //    File.Copy(pathClean, pathProject, true);
    //  }
    //  catch (Exception e)
    //  {
    //    Debug.Log("The file could not be read:");
    //    Debug.Log(e.Message);
    //  }
    // }
  }
}