using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// responsible of connecting the UI with the Algorithm
public class UIcontroller : MonoBehaviour {

    public InputField nodesInput;
    public InputField edgesInput;
    public Button buildGraphButton;
    public Button runPrimAlgorithmButton;

    public Graph thisGraph;

    //public UnityAction<string> nodesUpdated;
    //public UnityAction<string> edgesUpdated;

    // Use this for initialization
    void Start () {
        nodesInput.onValueChanged.AddListener(UpdateNodes);
        edgesInput.onValueChanged.AddListener(UpdateEdges);
        buildGraphButton.onClick.AddListener(thisGraph.GenerateGraph);
	}

    void UpdateNodes(string s) {

        int nd;

        if (int.TryParse(s, out nd))
        {
            thisGraph.SetNodesAmount(nd);
        }
        else {
            thisGraph.SetNodesAmount(0);
        }
    }
    void UpdateEdges(string s)
    {
        int nd;

        if (int.TryParse(s, out nd))
        {
            thisGraph.SetEdgesAmount(nd);
        }
        else
        {
            thisGraph.SetEdgesAmount(0);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
