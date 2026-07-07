using UnityEngine;
using UnityEditor;


public static class MeshAssetCreator
{
  [MenuItem("Tools/Create Meshes/Create Cone")]
  private static void CreateCone()
  {
    const string assetName = "Cone";

    var divisions = 60;
    
    var verts = new Vector3[divisions * 2 + 2];
    var tris = new int[3 * divisions * 2];

    var r = 0.5f;
    var deltaAngle = 2 * Mathf.PI / divisions;
    var tipIndex = 0;
    var sideStartIndex = tipIndex + 1;
    var baseStartIndex = sideStartIndex + divisions;
    var baseCenterIndex = verts.Length - 1;

    verts[tipIndex] = Vector3.up;
    verts[baseCenterIndex] = Vector3.zero;

    for (var i = 0; i < divisions; ++i)
    {
      var angle = deltaAngle * i;
      var x = r * Mathf.Cos(angle);
      var z = r * Mathf.Sin(angle);
      var vert = new Vector3(x, 0, z);
      verts[sideStartIndex + i] = vert;
      verts[baseStartIndex + i] = vert;

      var next = (i + 1) % divisions;
      var prev = (i - 1 + divisions) % divisions;

      var sideTriIndex = 3 * i;
      tris[sideTriIndex + 0] = tipIndex;
      tris[sideTriIndex + 1] = sideStartIndex + next;
      tris[sideTriIndex + 2] = sideStartIndex + i;

      var baseTriIndex = 3 * (divisions + i);
      tris[baseTriIndex + 0] = baseCenterIndex;
      tris[baseTriIndex + 1] = baseStartIndex + prev;
      tris[baseTriIndex + 2] = baseStartIndex + i;
    }

    CreateHelper(assetName, verts, tris);
  }


  [MenuItem("Tools/Create Meshes/Create Pyramid")]
  private static void CreatePyramid()
  {
    const string assetName = "Pyramid";

    var divisions = 60;

    var sideVertCount = 3 * divisions;
    var baseVertCount = divisions + 1;
    
    var verts = new Vector3[sideVertCount + baseVertCount];
    var tris = new int[3 * divisions * 2];

    var r = 0.5f;
    var deltaAngle = 2 * Mathf.PI / divisions;
    var baseStartIndex = sideVertCount;
    var baseCenterIndex = verts.Length - 1;

    verts[baseCenterIndex] = Vector3.zero;

    for (var i = 0; i < divisions; ++i)
    {
      // Vertices
      var tipIndex =     3 * i + 0;
      var rimIndex =     3 * i + 1;
      var rimNextIndex = 3 * i + 2;

      var angle = deltaAngle * i;
      var angleNext = angle + deltaAngle;
      var x = r * Mathf.Cos(angle);
      var z = r * Mathf.Sin(angle);
      var xNext = r * Mathf.Cos(angleNext);
      var zNext = r * Mathf.Sin(angleNext);
      var rimVert = new Vector3(x, 0, z);

      verts[tipIndex] = Vector3.up;
      verts[rimIndex] = rimVert;
      verts[rimNextIndex] = new(xNext, 0, zNext);

      verts[baseStartIndex + i] = rimVert;

      // Triangles
      var prev = (i - 1 + divisions) % divisions;

      var sideTriIndex = 3 * i;
      tris[sideTriIndex + 0] = tipIndex;
      tris[sideTriIndex + 2] = rimIndex;
      tris[sideTriIndex + 1] = rimNextIndex;

      var baseTriIndex = 3 * (divisions + i);
      tris[baseTriIndex + 0] = baseCenterIndex;
      tris[baseTriIndex + 1] = baseStartIndex + prev;
      tris[baseTriIndex + 2] = baseStartIndex + i;
    }

    CreateHelper(assetName, verts, tris);
  }


  private static void CreateHelper(string assetName, Vector3[] verts, int[] tris)
  {
    const string folderPath = "Assets/Meshes";
    var assetPath = $"{folderPath}/{assetName}.asset";

    if (!AssetDatabase.IsValidFolder(folderPath))
    {
      AssetDatabase.CreateFolder("Assets", "Meshes");
    }

    var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
    var createdStr = "created";

    if (mesh == null)
    {
      mesh = new Mesh { name = assetName };
      AssetDatabase.CreateAsset(mesh, assetPath);
    }
    else
    {
      mesh.Clear();
      mesh.name = assetName;
      createdStr = "updated";
    }

    mesh.vertices = verts;
    mesh.triangles = tris;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();

    EditorUtility.SetDirty(mesh);
    AssetDatabase.SaveAssets();

    Debug.Log($"Create {assetName}".B() + $": mesh asset {createdStr} at {assetPath}");
  }
}
