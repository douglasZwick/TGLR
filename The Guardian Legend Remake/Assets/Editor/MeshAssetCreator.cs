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
    // Longitudinal bands of faces connecting the top and bottom poles
    var sphereDivisions = 24;
    // Latitudinal bands of faces in half the sphere
    var hemisphereSlices = 6;
    var assetName = $"Sphere{sphereDivisions}d{hemisphereSlices}s";
    var totalSlices = 2 * hemisphereSlices;

    // Latitudinal rings of vertices
    var ringCount = totalSlices - 1;
    var verts = new Vector3[ringCount * sphereDivisions + 2];
    
    var trisInPolarCaps = 2 * sphereDivisions;
    // Twice the number of hemisphere slices, minus two for the polar caps
    var quadBandCount = ringCount - 1;
    var totalQuads = sphereDivisions * quadBandCount;
    // The polar caps are made of triangles, and there are two triangles in each quad
    var triCount = trisInPolarCaps + 2 * totalQuads;
    // There are three triangle indices for each triangle in the mesh
    var tris = new int[3 * triCount];

    // Rho is the radius of the sphere, as opposed to the planar radius of any given latitude
    var rho = 0.5f;
    // Theta is the angle measured around the polar axis (the Y axis of the mesh).
    // deltaTheta is thus how much to add on to theta for each step in the inner loop (going from
    // one longitudinal band to the next), calculated as a full circle divided into sphereDivisions
    // parts, so 2 pi over sphereDivisions
    var deltaTheta = 2 * Mathf.PI / sphereDivisions;
    // Phi is the angle measured down from the top pole.
    // deltaPhi is thus how much to add on to phi for each step in the outer loop (going from one
    // latitudinal band to the next), calculated as a half circle divided into totalSlices
    // parts, so pi over totalSlices
    var deltaPhi = Mathf.PI / totalSlices;

    // Vertex landmarks
    var topPoleIndex = 0;
    var bottomPoleIndex = verts.Length - 1;

    verts[topPoleIndex] = Vector3.up * rho;

    // Vertex generation
    for (var slice = 1; slice <= hemisphereSlices; ++slice)
    {
      var phi = deltaPhi * slice;
      var y = rho * Mathf.Cos(phi);
      // r, the planar radius, varies sinusoidally with latitude
      var r = rho * Mathf.Sin(phi);

      // Which upper vertex ring are we on? Indexed from 0 (that is, the top ring is ring 0)
      var upperRingIndex = slice - 1;
      // Which lower vertex ring are we on? We count backwards from the end for this one
      var lowerRingIndex = ringCount - slice;

      for (var division = 0; division < sphereDivisions; ++division)
      {
        var theta = deltaTheta * division;
        var x = r * Mathf.Cos(theta);
        var z = r * Mathf.Sin(theta);
        var upperVert = new Vector3(x, +y, z);
        // The index of the vertex in the top hemisphere is
        //   1 (for the top pole)
        //   + upperRingIndex * sphereDivisions (how many rings' worth of verts to add)
        //   + division (which division in the current ring we're on now)
        var upperVertIndex = 1 + upperRingIndex * sphereDivisions + division;
        verts[upperVertIndex] = upperVert;

        // If we're at the equator, don't proceed
        if (slice == hemisphereSlices) continue;
        
        var lowerVert = new Vector3(x, -y, z);
        // The index of the vertex in the bottom hemisphere is basically calculated the same way as
        //   for the top hemisphere, only we use lowerRingIndex instead of upperRingIndex
        var lowerVertIndex = 1 + lowerRingIndex * sphereDivisions + division;
        verts[lowerVertIndex] = lowerVert;
      }
    }

    verts[bottomPoleIndex] = Vector3.down * rho;

    // Index of the first vert in the topmost vertex ring
    var topRingStart = topPoleIndex + 1;
    // Index of the first vert in the bottommost vertex ring
    var bottomRingStart = topRingStart + sphereDivisions * quadBandCount;

    // Index of the first tri index in the first tri of the top cap (always 0)
    var topCapStart = 0;
    // Index of the first tri index in the first quad (right after the top cap)
    var quadStart = 3 * sphereDivisions;
    // Index of the first tri index in the first tri of the bottom cap. Calculated by starting at
    // the first quad and then adding enough indices to move past the last index in the last quad:
    //   quadStart             -- start at the first quad
    //   + (3                  -- three indices per tri
    //    * 2                  -- two tris per quad
    //    * quadBandCount      -- how many quad bands there are
    //    * sphereDivisions)   -- how many quads are in each band
    var bottomCapStart = quadStart + 3 * 2 * quadBandCount * sphereDivisions;

    // Triangle generation
    for (var slice = 0; slice < totalSlices; ++slice)
    {
      for (var division = 0; division < sphereDivisions; ++division)
      {
        // nextDivision is 0 if we're on the last division, or division + 1 otherwise
        var nextDivision = (division + 1) % sphereDivisions;

        // If we're in the top polar cap...
        if (slice == 0)
        {
          // ...then there are no quads, only tris

          // TRIANGLE INDEX. Start at zero (topCapStart), and then add three times the division
          //   number we're on (for three triangle indices per division)
          var index = topCapStart + 3 * division;

          tris[index + 0] = topPoleIndex;
          tris[index + 1] = topRingStart + nextDivision;
          tris[index + 2] = topRingStart + division;
        }
        // If we're in the quad bands section of the sphere...
        else if (slice < totalSlices - 1)
        {
          // ...then we need to compute indices for two tris per quad

          // Which quad band are we on? Indexed from 0
          var bandIndex = slice - 1;
          // VERTEX INDICES. Each band has two vertex rings, the upper and lower.
          var upperRingStart = topRingStart + bandIndex * sphereDivisions;
          var lowerRingStart = upperRingStart + sphereDivisions;

          // TRIANGLE INDEX. Start at the start of the first quad, and then add enough tris' worth
          // of indices to get to where we need to be for the current division in the current slice:
          //   quadStart             -- start at the first quad
          //   + (3                  -- three indices per tri
          //    * 2                  -- two tris per quad
          //    * (bandIndex         -- which band we're in now
          //     * sphereDivisions   -- how many quads are in each band
          //     + division))        -- which quad we're on in the current band
          var index = quadStart + 3 * 2 * (bandIndex * sphereDivisions + division);

          // Fill in the indices thusly:
          // 
          //   A--C
          //   |\ |
          //   | \|
          //   B--D
          //
          // A: upperRingStart + division
          // B: lowerRingStart + division
          // C: upperRingStart + nextDivision
          // D: lowerRingStart + nextDivision
          //
          // ...so that the two triangles are ADB and ACD

          tris[index + 0] = upperRingStart + division;      // A
          tris[index + 1] = lowerRingStart + nextDivision;  // D
          tris[index + 2] = lowerRingStart + division;      // B

          tris[index + 3] = upperRingStart + division;      // A
          tris[index + 4] = upperRingStart + nextDivision;  // C
          tris[index + 5] = lowerRingStart + nextDivision;  // D
        }
        // If we're in the bottom polar cap...
        else // slice == totalSlices - 1
        {
          // ...then there are no quads, only tris

          // TRIANGLE INDEX. Start at the bottom cap, and then add three times the division
          //   number we're on (for three triangle indices per division)
          var index = bottomCapStart + 3 * division;

          tris[index + 0] = bottomPoleIndex;
          tris[index + 1] = bottomRingStart + division;
          tris[index + 2] = bottomRingStart + nextDivision;
        }
      }
    }

    CreateHelper(assetName, verts, tris);
  }

  
  [MenuItem("Tools/Create Meshes/Create Octahedron")]
  private static void CreateOctahedron()
  {
    const string assetName = "Octahedron";

    var faces = 8;
    var vertsPerFace = 3;
    var verts = new Vector3[faces * vertsPerFace];
    var tris = new int[faces * vertsPerFace];

    var radius = 0.5f;
    var vr = Vector3.right * radius;
    var vl = Vector3.left * radius;
    var vu = Vector3.up * radius;
    var vd = Vector3.down * radius;
    var vf = Vector3.forward * radius;
    var vb = Vector3.back * radius;

    verts[ 0] = vr;
    verts[ 1] = vu;
    verts[ 2] = vf;

    verts[ 3] = vf;
    verts[ 4] = vu;
    verts[ 5] = vl;
    
    verts[ 6] = vl;
    verts[ 7] = vu;
    verts[ 8] = vb;

    verts[ 9] = vb;
    verts[10] = vu;
    verts[11] = vr;

    verts[12] = vr;
    verts[13] = vd;
    verts[14] = vb;

    verts[15] = vb;
    verts[16] = vd;
    verts[17] = vl;

    verts[18] = vl;
    verts[19] = vd;
    verts[20] = vf;

    verts[21] = vf;
    verts[22] = vd;
    verts[23] = vr;

    for (var i = 0; i < tris.Length; ++i)
      tris[i] = i;

    CreateHelper(assetName, verts, tris);
  }


  [MenuItem("Tools/Create Meshes/Create Octahedron Octant")]
  private static void CreateOctahedronOctant()
  {
    const string assetName = "Octant";

    var faces = 4;
    var vertsPerFace = 3;
    var verts = new Vector3[faces * vertsPerFace];
    var tris = new int[faces * vertsPerFace];

    var radius = 0.5f;
    var vr = Vector3.right * radius;
    var vu = Vector3.up * radius;
    var vf = Vector3.forward * radius;
    var vc = Vector3.zero;

    verts[ 0] = vr;
    verts[ 1] = vu;
    verts[ 2] = vf;

    verts[ 3] = vf;
    verts[ 4] = vu;
    verts[ 5] = vc;

    verts[ 6] = vc;
    verts[ 7] = vu;
    verts[ 8] = vr;

    verts[ 9] = vr;
    verts[10] = vf;
    verts[11] = vc;

    for (var i = 0; i < tris.Length; ++i)
      tris[i] = i;

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
