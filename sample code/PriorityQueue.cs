using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! ArrayList에 Node를 추가하거나 제거할 때 Sort 메소드를 호출한다
//! (내부적으로 Node 오브젝트의 CompareTo 메소드를 호출해 estimatedCost 값에 따라 Node를 정렬함)
public class PriorityQueue
{
    private ArrayList nodes = new ArrayList();

    public int Length
    {
        get { return this.nodes.Count; }
    }
    public bool Contains(object node)
    {
        return this.nodes.Contains(node);
    }
    public Node First()
    {
        if (this.nodes.Count > 0)
        {
            return (Node)this.nodes[0];
        }
        return null;
    }
    public void Push(Node node)
    {
        this.nodes.Add(node);
        this.nodes.Sort();
    }
    public void Remove(Node node)
    {
        this.nodes.Remove(node);
        //리스트를 확실하게 정렬한다.
        this.nodes.Sort();
    }

}
