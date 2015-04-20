using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Edge : IComparable<Edge>
{
    public Vector3 startPosition;
    GameObject startObj;
    public Vector3 endPosition;
    GameObject endObj;
    float distance;
    public string edgeName;
    public string startName;
    public string endName;

    public Edge()
    {

    }

    public Edge(GameObject _startObj, GameObject _endObj)
    {
        setEdge(_startObj, _endObj);


    }

 override public string ToString()
    {
        return startPosition.ToString() + ":" + endPosition.ToString() + "distance:" + distance + "objStart:" + startObj.name + "objEnd:" + endObj.name;
    }



    public float getDistance()
    {
        distance = Vector3.Distance(startObj.transform.position, endObj.transform.position);
        return distance;

    }
    public void setEdge(GameObject _start, GameObject _end)
    {
        startObj = _start;
        startName = _start.name;
        startPosition = startObj.transform.position;
        //getDistance();

        endObj = _end;
        endName = _end.name;
        endPosition = endObj.transform.position;

        edgeName = startName + endName;
        getDistance();
    }
    public GameObject getStart()
    {
        return startObj;
    }
    public GameObject getEnd()
    {
        return endObj;
    }

    public int CompareTo(Edge other)
    {
        return distance.CompareTo(other.distance);
    }



}
