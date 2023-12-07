//
// Dedicated to all my Patrons on Patreon,
// as a thanks for your continued support 💖
//
// Source code © Freya Holmér, 2019
// This code is provided exclusively to supporters,
// under the Attribution Assurance License
// "https://tldrlegal.com/license/attribution-assurance-license-(aal)"
// 
// You can basically do whatever you want with this code,
// as long as you include this license and credit me for it,
// in both the source code and any released binaries using this code
//
// Thank you so much again <3
//
// Freya
//

using System.Linq;
using UnityEngine;

// This is the parent container for all the road segments
[ExecuteInEditMode]
public class RoadChain : MonoBehaviour {

	public Mesh2D mesh2D = null; // The 2D shape to be extruded
	public bool loop = false; // Whether or not the last segment should connect to the first
	public float edgeLoopsPerMeter = 2; // Triangle density, in loops per meter!
	public UVMode uvMode = UVMode.TiledDeltaCompensated; // More info on what this is in the enum!

	// Regenerate mesh on instantiation.
	// If you save the mesh in the scene you don't have to do this, but, it's pretty fast anyway so whatevs!
	void Awake() => UpdateMeshes();

#if UNITY_EDITOR
	// For a proper version, make sure you don't do this every frame, this is pretty dang expensive~
	// You'd want to only run this code when the shape is actually changed
	void Update() => UpdateMeshes();
#endif

	public RoadSegment[] GetRoadSegments()
	{
		var allSegments = GetComponentsInChildren<RoadSegment>();
		var segmentsWithMesh = allSegments.Where( s => s.HasValidNextPoint ).ToArray();

		return segmentsWithMesh;
	}

	// Iterates through all children / road segments, and updates their meshes!
	public void UpdateMeshes() {

		RoadSegment[] allSegments = GetComponentsInChildren<RoadSegment>();
		RoadSegment[] segmentsWithMesh = allSegments.Where( s => s.HasValidNextPoint ).ToArray();
		RoadSegment[] segmentsWithoutMesh = allSegments.Where( s => s.HasValidNextPoint == false ).ToArray();

		// We calculate the total length of the road, in order for us to be able to supply a normalized
		// coordinate for how far along the track you are, where
		// 0 = start of the track
		// 0.5 = halfway through the track
		// 1.0 = end of the track
		float[] lengths = segmentsWithMesh.Select( x => x.GetBezierRepresentation( Space.Self ).GetArcLength() ).ToArray();
		float totalRoadLength = lengths.Sum();

		float startDist = 0f;
		for( int i = 0; i < segmentsWithMesh.Length; i++ ) {
			float endDist = startDist + lengths[i];
			Vector2 uvzStartEnd = new Vector2(
				startDist / totalRoadLength,		// Percentage along track start
				endDist / totalRoadLength			// Percentage along track end
			);
			segmentsWithMesh[i].UpdateMesh( uvzStartEnd );
			startDist = endDist;
		}

		// Clear all segments without meshes
		foreach( RoadSegment seg in segmentsWithoutMesh ) {
			seg.UpdateMesh( Vector2.zero );
		}


	}
}
