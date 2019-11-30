using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    //node存的是左岸的P，D
    public int P;
    public int D;
    public bool ifBoatSizeRight; //false left , true right
    public List<Node> adjacentNodes;
    public int parent = -1;//寻找路线时标记

    public Node(Node cpyNode) {
        this.P = cpyNode.P;
        this.D = cpyNode.D;
        this.ifBoatSizeRight = cpyNode.ifBoatSizeRight;
        this.adjacentNodes = new List<Node>(cpyNode.adjacentNodes);
    }

    public Node(int P, int D, bool boatSize) {
        this.P = P;
        this.D = D;
        this.ifBoatSizeRight = boatSize;
        adjacentNodes = new List<Node>();
    }

    public bool equals(Node node) {
        return this.P == node.P && this.D == node.D
            && this.ifBoatSizeRight == node.ifBoatSizeRight;
    }

    public void addAdjacentNode(Node newAdjacentNode) {
        if (!exist(newAdjacentNode, adjacentNodes)) {
            adjacentNodes.Add(newAdjacentNode);
        }
    }

    public bool exist(Node findNode, List<Node> searchList) {
        foreach (Node tmp in searchList) {
            if (findNode.equals(tmp)) return true;
        }
        return false;
    }

    //到相接的点所用的操作
    public operation[] getOperationByAnthoerNode() {
        operation[] result = new operation[adjacentNodes.Count];
        for (int i = 0; i < adjacentNodes.Count; i++) {
            result[i] = new operation(Mathf.Abs(adjacentNodes[i].P - P), Mathf.Abs(adjacentNodes[i].D - D));
        }
        return result;
    }

    public static Node operator -(Node first, operation second) {
        return new Node(first.P - second.P, first.D - second.D, !first.ifBoatSizeRight);
    }

    public static Node operator +(Node first, operation second) {
        return new Node(first.P + second.P, first.D + second.D, !first.ifBoatSizeRight);
    }
}

public class operation {
    public int P;
    public int D;
    public operation(int P, int D) {
        this.P = P;
        this.D = D;
    }
}

public class statusGraph : MonoBehaviour {
    private List<Node> allGraphNodes;
    private operation[] nodeOperations = {new operation (0, 1) , new operation (1, 0) ,
        new operation (1, 1) , new operation (2, 0) , new operation (0, 2) };
    private int maxP;
    private int maxD;
    private bool boatStartSize;
    private Node endStatusNode;
    List<Node> path = null;

    public void createGraph() {
        this.maxP = 3;
        this.maxD = 3;
        endStatusNode = new Node(maxP, maxD, false);

        allGraphNodes = new List<Node>();
        allGraphNodes.Add(new Node(0, 0, true));

        for (int index = 0; index < allGraphNodes.Count; index++) {
            Node thisNode = allGraphNodes[index];
            foreach (operation op in nodeOperations) {
                Node adjcentNode;
                if (thisNode.ifBoatSizeRight) adjcentNode = thisNode + op;
                else adjcentNode = thisNode - op;

                if (ifNodeValid(adjcentNode)) {
                    if (!adjcentNode.exist(adjcentNode, allGraphNodes)) {
                        allGraphNodes.Add(adjcentNode);
                    }
                    adjcentNode.addAdjacentNode(thisNode);
                    thisNode.addAdjacentNode(adjcentNode);
                }
            }
        }
    }

    private bool ifNodeValid(Node test) {
        Node adj = new Node(maxP - test.P, maxD - test.D, !test.ifBoatSizeRight);
        return (((test.P >= test.D || test.P == 0) && test.D <= maxD && test.P <= maxP) &&
            ((adj.P >= adj.D || adj.P == 0) && adj.D <= maxD && adj.P <= maxP));
    }

    public operation getNextStep(Node current) {
        if (ifNodeValid(current)) {
            Node nextNode = getNextNode(current);
            return new operation(Mathf.Abs(current.P - nextNode.P), Mathf.Abs(current.D - nextNode.D));
        }
        return null;
    }

    private int getNodeIndex(Node findNode) {
        for (int i = 0; i < allGraphNodes.Count; i++) {
            if (findNode.equals(allGraphNodes[i])) return i;
        }
        return -1;
    }

    private List<Node> findSol(Node start) {
        List<Node> sol = new List<Node>();
        List<Node> open = new List<Node>();
        List<Node> close = new List<Node>();

        open.Add(start);
        Node current = allGraphNodes[getNodeIndex(start)];
        while (open.Count > 0) {
            if (current.equals(endStatusNode)) break;
            foreach (Node adj in current.adjacentNodes) {
                if (!adj.exist(adj, open) && !adj.exist(adj, close)) {
                    adj.parent = getNodeIndex(current);
                    open.Add(adj);
                }
            }
            close.Add(open[0]);
            open.RemoveAt(0);
            current = allGraphNodes[getNodeIndex(open[0])];
        }
        int parent;
        current = allGraphNodes[getNodeIndex(endStatusNode)];
        while (!current.equals(start)) {
            parent = current.parent;
            current = allGraphNodes[parent];
            sol.Add(allGraphNodes[parent]);
        }
        sol.Reverse();
        sol.Add(endStatusNode);
        return sol;
    }

    private Node getNextNode(Node startNode) {
        if (path == null || !startNode.exist(startNode, path)) path = findSol(startNode);
        for (int i = 0; i < path.Count; i++) {
            if (path[i].equals(startNode)) return path[i + 1];
        }
        return null;
    }
}

