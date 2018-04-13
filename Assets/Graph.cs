using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    public int maxLength = 5;
    public int maxDepth = 5;

    public float lengthUnit  = 2;
    public float depthUnit = 2;

    public int nodesAmount;
    public int edgesAmount;
    public GameObject nodePrefab;
    public GameObject edgePrefab;
    public List<GraphNode> nodesAvailable;
    public List<GraphEdge> edgesAvailable;
    public int[,] nodesPositions;


    public void CreateEmptyGraph() {
        nodesAvailable = new List<GraphNode>();
        edgesAvailable = new List<GraphEdge>();
    }
  
    public void AddNode(GraphNode gn) {
        nodesAvailable.Add(gn);
        gn.Id = nodesAvailable.Count - 1;
    }


    public void AddEdge(GraphEdge ge)
    {
        edgesAvailable.Add(ge);
        ge.EdgeId = edgesAvailable.Count - 1;
    }

    public void SetNodesAmount(int nodes) {
        nodesAmount = nodes;
    }

    public void SetEdgesAmount(int edges) {
        edgesAmount = edges;
    }

    public GraphNode CreateNode(int x, int z) {

        GameObject n = Instantiate(nodePrefab, new Vector3(x * lengthUnit, 0, z * depthUnit), Quaternion.identity);

        return n.GetComponent<GraphNode>();
    }

    public GraphEdge CreateEdge(int fromId, int toId) {

        GameObject node = nodesAvailable[fromId].gameObject;
        GameObject e = Instantiate(edgePrefab, node.transform.position , Quaternion.identity);
        GraphEdge edge = e.GetComponent<GraphEdge>();

        edge.SetEdge(nodesAvailable[fromId], nodesAvailable[toId], true);
        return edge;

    }

    public void CreateNodes() {

        nodesAvailable = new List<GraphNode>();

        for (int i = 0; i < nodesAmount; i++)
        {

            bool validPos = false;
            int enough = 0;
            while (!validPos && enough++ < 90)
            {
                int x = Random.Range(0, maxLength);
                int z = Random.Range(0, maxDepth);

                if (nodesPositions[x, z] == -1)
                {
                    validPos = true;
                    GraphNode gn = CreateNode(x, z);
                    nodesAvailable.Add( gn);
                    gn.Id = nodesAvailable.Count - 1;
                    nodesPositions[x, z] = gn.Id;
                }


                if (enough == 90)
                    Debug.LogWarning("Not all of the nodes were done");
            }
        }
    }

  

    public void CreateEdges() {

        int edgeCounter = this.edgesAmount;


        while (edgeCounter > 0) { 
            for (int i = 0; i < maxLength; i++) {

                for (int j = 0; j < maxDepth; j++)
                {

                    
                    if (nodesPositions[i, j] != -1) { // theres a node in it

                        int toLook = nodesPositions[i, j];
                       
                        int fId = nodesAvailable[toLook].Id;


                        int toId = Random.Range(0, nodesAvailable.Count);

                        if (toId == fId ) {
                            toId = ((toId + 1) % nodesAvailable.Count);
                        }

                        GraphEdge ge = CreateEdge(fId, toId);
                        edgesAvailable.Add(ge);
                        ge.EdgeId = edgesAvailable.Count - 1;
                        edgeCounter--;
                    }

                    if (edgeCounter <= 0)
                        break;
                }

                if (edgeCounter <= 0)
                    break;
            }
        }
    }


    public void ResetAll() {

        int i = 0;
        int to = edgesAvailable.Count;
        for (i=0; i < to ; i++)
        {
            GraphEdge edge = edgesAvailable[0];
            edge.From = null;
            edge.To = null;
            edgesAvailable.Remove(edge);
            Destroy(edge.gameObject);
        }

        i = 0;
        to = nodesAvailable.Count;

        for (i = 0; i < to; i++)
        {
            GraphNode node = nodesAvailable[0];
            nodesAvailable.Remove(node);
            Destroy(node.gameObject);
        }

        ResetNodePositions();

    }
    // method for generating the grpah from scratch
    public void GenerateGraph() {
        // get the amount of nodes and edges
        // ready via setters

        if (nodesAvailable.Count > 0)
            ResetAll();
        if (nodesAmount > maxLength * maxDepth) { 
            Debug.LogError("Impossible to accomodate all nodes");
            return;
        }

        if (edgesAmount < nodesAmount)
        {
            Debug.LogWarning("Some nodes may not be connected");
            
        }
        // create the nodes
        CreateNodes();
        // for each node set at leaast 1 arc to another one

        // the rest of arcs should be scattered among all nodes
        CreateEdges();

    }



    // none of these

	// Use this for initialization
	void Start () {
        nodesPositions = new int[maxLength, maxDepth];
        nodesAvailable = new List<GraphNode>();
        edgesAvailable = new List<GraphEdge>();
        ResetNodePositions();
    }

    void ResetNodePositions() { 
        for (int i = 0; i < maxLength; i++)
        {

            for (int j = 0; j < maxDepth; j++)
            {
                nodesPositions[i, j] = -1;

            }

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
