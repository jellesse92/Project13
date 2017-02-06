using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnvironmentBuilder : MonoBehaviour {
    //Put this script on the parent of your ground and wall sprites
    public List<List<Vector2>> bounds;
    PolyCombine combiner;
    public PhysicsMaterial2D groundMat;
    public PhysicsMaterial2D wallMat;
    public GameObject polygonHelper;
    public int tolerance;

	void Start () {
        //Get All Ground Objects
        var polygons = GameObject.FindGameObjectsWithTag("Ground");
        var walls = GameObject.FindGameObjectsWithTag("Wall");
        bounds = new List<List<Vector2>>();
        combiner = new PolyCombine();
        foreach( GameObject poly in polygons)
        {
            bounds.Add(TransformLocalToLocalVector2(poly.GetComponent<PolygonCollider2D>()));
            Destroy(poly.GetComponent<PolygonCollider2D>());
        }
        var newBounds = combiner.UniteCollisionPolygons(bounds);
        combiner.CreateLevelCollider(gameObject, newBounds);
        ApplyGroundColliderSettings(gameObject.GetComponent<PolygonCollider2D>());
        ApplyWallColliderSettings(walls);
        SetGroundColliderTolerance();

    }

    void SetGroundColliderTolerance()
    {
        gameObject.GetComponent<Collider2DOptimization.PolygonColliderOptimizer>().tolerance = tolerance;
        gameObject.GetComponent<Collider2DOptimization.PolygonColliderOptimizer>().OnValidate();
    }
    
    List<Vector2> TransformLocalToLocalVector2(PolygonCollider2D collider)
    {
        var worldPts = (from pt in collider.points select ((Vector2)collider.transform.TransformPoint(pt))).ToList();
        var newPoints = from np in worldPts select (Vector2)transform.InverseTransformPoint(np);
        return newPoints.ToList();
    }

    void ApplyGroundColliderSettings(PolygonCollider2D collider)
    {
        //current Settings for ground colliders
        collider.offset = new Vector2(0, -.5f);
        collider.sharedMaterial = groundMat;
    }

    void ApplyWallColliderSettings(GameObject[] walls)
    {
        foreach ( GameObject wall in walls)
        {
            PolygonCollider2D collider = wall.GetComponent<PolygonCollider2D>();
            collider.sharedMaterial = wallMat;
        }
        //current settings for wall colliders

    }
}
