using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEdge : MonoBehaviour {


    private GraphNode fromNode;
    private GraphNode toNode;
    private bool hasDirection;
    private int edgeId;
    bool initialized = false;
    public LineRenderer line;

    public GraphNode From {
        get { return fromNode; }
        set { fromNode = value; }
    }

    public GraphNode To
    {
        get { return toNode; }
        set { toNode = value; }
    }

    public bool isDirected {
        get { return hasDirection; }
        set { hasDirection = value; }
    }

    public int EdgeId
    {
        get
        {
            return edgeId;
        }

        set
        {
            edgeId = value;
        }
    }

    public void SetEdge(GraphNode fn, GraphNode tn, bool hasDir, int edgeId = -1) {
        this.From = fn;
        this.To = tn;
        this.isDirected = hasDir;
        this.EdgeId = edgeId;

        
    }

    // Use this for initialization
    void Start () {
        line = this.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if( !initialized) {
            initialized = true;


            line.useWorldSpace = true;
            this.transform.position = From.transform.position;
            this.transform.rotation = Quaternion.LookRotation((To.transform.position - From.transform.position).normalized);


            Vector3 diff = (To.transform.position - From.transform.position);
            float distance = diff.magnitude;

            line.positionCount = (int)(distance / 0.3);


            int increments = (line.positionCount) / 8+1;
            int incrementSegment = 1;

            GradientColorKey[] keys = new GradientColorKey[ Mathf.Min(8, (line.positionCount) / 8+1) ];
            GradientAlphaKey[] alphas = new GradientAlphaKey[2];

            line.SetPosition(0, From.transform.position);
            keys[0] = new GradientColorKey(new Color(1,1,1,0) , 0);
            alphas[0] = new GradientAlphaKey(1,0);

            for (int i = 1; i < line.positionCount; i++) {
                float pct = i * 1.0f / line.positionCount;

                float x = From.transform.position.x + i * diff.x / line.positionCount;
                float z = From.transform.position.z + i * diff.z / line.positionCount;
                float maxHeight = distance / 100;

                if ( (i % increments) == 0 && incrementSegment < keys.Length) {
                    keys[incrementSegment] = new GradientColorKey(  Color.white*(1-pct)+Color.red*pct , pct);
                    incrementSegment++;
                }
                float y = Mathf.Sin( (float) (pct) *Mathf.PI)*(33*maxHeight); // 30 is a constant for the max height, this should be also in function of the distance
               
                line.SetPosition( i , new Vector3(x,y,z) );
            }

            keys[incrementSegment-1] = new GradientColorKey(Color.red, 1);
            alphas[1] = new GradientAlphaKey(1, 1);

            Gradient g = new Gradient();
            g.SetKeys(keys, alphas);
            line.colorGradient = g;
            line.SetPosition(line.positionCount-1, To.transform.position);

        }
	}
}
