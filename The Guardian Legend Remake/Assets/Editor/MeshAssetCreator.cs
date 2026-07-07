using UnityEngine;
using UnityEditor;


public static class MeshAssetCreator
{
  [MenuItem("Tools/Create Meshes/Create Cone")]
  private static void CreateCone()
  {
    const string assetName = "Cone";

    var divisions = 24;
    
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

    var divisions = 12;

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


  // [MenuItem("Tools/Create Meshes/Create Teardrop")]
  private static void CreateTeardrop()
  {
    const string assetName = "Pyramid";

    var divisions = 12;
    var slices = 3;

    var tipVertCount = 1;
    var hemisphereVertCount = divisions * slices;
    var poleVertCount = 1;
    var verts = new Vector3[tipVertCount + hemisphereVertCount + poleVertCount];
    
    var coneTriCount = divisions;
    var hemisphereTriCount = divisions * slices * 2;
    var tris = new int[coneTriCount + hemisphereTriCount];



    CreateHelper(assetName, verts, tris);
  }


  [MenuItem("Tools/Create Meshes/Create Sphere")]
  private static void CreateSphere()
  {
    var divisions = 12;
    var slices = 3;
    var assetName = $"Sphere{divisions}d{slices}s";

    var ringCount = 2 * slices - 1;
    var verts = new Vector3[ringCount * divisions + 2];
    
    var tris = new int[3 * divisions * slices * 2 * 2];

    var r = 0.5f;
    var deltaTheta = 2 * Mathf.PI / divisions;
    var deltaRho = Mathf.PI / (2 * slices);
    var topPoleIndex = 0;
    var topRingStartIndex = topPoleIndex + 1;
    var botRingStartIndex = divisions * (ringCount - 1) + 1;
    var botPoleIndex = verts.Length - 1;

    verts[topPoleIndex] = Vector3.up / 2;

    for (var j = 0; j < ringCount; ++j)
    {
      var rho = deltaRho * j;
      var y = r * (1 - Mathf.Cos(rho));

      for (var i = 0; i < divisions; ++i)
      {
        var theta = deltaTheta * i;
        var x = r * Mathf.Cos(theta);
        var z = r * Mathf.Sin(theta);
        var vert = new Vector3(x, y, z);
        var vertIndex = j * divisions + i + 1;
        verts[vertIndex] = vert;

        var next = (i + 1) % divisions;

        if (j == ringCount - 1) continue;

        if (j == 0)
        {
          var topTriIndex = 3 * i;
          tris[topTriIndex + 0] = topPoleIndex;
          tris[topTriIndex + 1] = topRingStartIndex + next;
          tris[topTriIndex + 2] = topRingStartIndex + i;

          var botTriIndex = 3 * (ringCount * divisions + i);
          tris[botTriIndex + 0] = botPoleIndex;
          tris[botTriIndex + 1] = botRingStartIndex + next;
          tris[botTriIndex + 2] = botRingStartIndex + i;
        }

        var quadIndex = 3 * divisions + 6 * (j * divisions + i);
        tris[quadIndex + 0] = topRingStartIndex + j * divisions + i;
        tris[quadIndex + 1] = topRingStartIndex + (j + 1) * divisions + next;
        tris[quadIndex + 2] = topRingStartIndex + (j + 1) * divisions + i;
        tris[quadIndex + 3] = topRingStartIndex + j * divisions + i;
        tris[quadIndex + 4] = topRingStartIndex + j * divisions + next;
        tris[quadIndex + 5] = topRingStartIndex + (j + 1) * divisions + next;
      }
    }

    verts[botPoleIndex] = Vector3.down / 2;

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
