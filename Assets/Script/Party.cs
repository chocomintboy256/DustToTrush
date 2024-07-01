using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Party
{
    const float FOLLOW_DISTANCE = 2.0f;
    public List<DragCharactor> party;
    public bool isParty { get { return party.Count > 0; } }

    public Party(List<DragCharactor> party = null)
    {
        this.party = party ?? new List<DragCharactor>();
    }

    public void Add(Array cs)
    {
        foreach (var c in cs.OfType<DragCharactor>().ToList()) { Add(c); }
    }
    public DragCharactor Add(DragCharactor c)
    {
        if (!isMember(c)) party.Add(c);
        return c;
    }
    public DragCharactor Remove(DragCharactor c)
    {
        party.Remove(c);
        if (party.Count == 1) Disbanded();
        return c;
    }
    public void Disbanded() { party.Clear(); }
    public bool isReader(DragCharactor c) { 
        return party.FirstOrDefault() == c; 
    }
    public bool isMember(DragCharactor c) { return party.FirstOrDefault(x => x == c) != null; }
    public void followMove()
    {
        if (!isParty) return;
        var t = party.First(); 
        foreach (var m in party)
        {
            if (t == m) continue;
            Vector3 p1 = t.transform.position;
            Vector3 p2 = m.transform.position;
            if (Vector3.Distance(p1, p2) >= FOLLOW_DISTANCE)
                m.transform.position = p1 - (p1 - p2).normalized * FOLLOW_DISTANCE;
            t = m;
        }
    }
}
