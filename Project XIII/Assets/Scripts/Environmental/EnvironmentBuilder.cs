using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnvironmentBuilder : MonoBehaviour {
    //Put this script on the parent of your ground and wall sprites
    public List<List<Vector2>> groundBounds;
    public List<List<Vector2>> wallBounds;
    PolyCombine combiner;
    public PhysicsMaterial2D groundMat;
    public PhysicsMaterial2D wallMat;
    public GameObject polygonHelper;
    GameObject wallObj, groundObj;
    public int tolerance;
    Vector2 origin = new Vector2(0, 0);

	void Start () {
        //Get All Ground Objects
        var polygons = GameObject.FindGameObjectsWithTag("Ground");
        var walls = GameObject.FindGameObjectsWithTag("Wall");
        groundBounds = new List<List<Vector2>>();
        wallBounds = new List<List<Vector2>>();
        combiner = new PolyCombine();
        foreach( GameObject poly in polygons)
        {
            groundBounds.Add(TransformLocalToLocalVector2(poly.GetComponent<PolygonCollider2D>()));
            Destroy(poly.GetComponent<PolygonCollider2D>());
        }

        foreach (GameObject wall in walls)
        {
            wallBounds.Add(TransformLocalToLocalVector2(wall.GetComponent<PolygonCollider2D>()));
            Destroy(wall.GetComponent<PolygonCollider2D>());
        }
        var newGroundBounds = combiner.UniteCollisionPolygons(groundBounds);
        var newWallBounds = combiner.UniteCollisionPolygons(wallBounds);
        wallObj = new GameObject("WallCollider");
        wallObj.transform.parent = gameObject.transform;
        groundObj = new GameObject("GroundCollider");
        groundObj.transform.parent = gameObject.transform;
        combiner.CreateLevelCollider(groundObj, newGroundBounds);
        combiner.CreateLevelCollider(wallObj, newWallBounds);
        ApplyGroundColliderSettings(gameObject.GetComponent<PolygonCollider2D>());
        ApplyWallColliderSettings(walls);
        SetColliderTolerance();
        wallObj.GetComponent<PolygonCollider2D>().offset = new Vector2(-10, 2.5f);
        groundObj.GetComponent<PolygonCollider2D>().offset = new Vector2(-10, 2.5f);



    }

    void SetColliderTolerance()
    {
        wallObj.GetComponent<Collider2DOptimization.PolygonColliderOptimizer>().tolerance = tolerance;
        wallObj.GetComponent<Collider2DOptimization.PolygonColliderOptimizer>().OnValidate();
        groundObj.GetComponent<Collider2DOptimization.PolygonColliderOptimizer>().tolerance = tolerance;
        groundObj.GetComponent<Collider2DOptimization.PolygonColliderOptimizer>().OnValidate();
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
