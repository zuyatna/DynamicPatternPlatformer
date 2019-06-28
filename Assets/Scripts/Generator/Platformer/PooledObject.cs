using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// information holder for a pooled object
/// </summary>
public class PooledObject : MonoBehaviour {

    [Tooltip(@"Name is used to offer the objects from the one other")]
    public string Name;

    [Tooltip(@"what object should be created?")]
    public GameObject Object;

    [Range(1, 1000)]
    [Tooltip(@"how much objects should be created?")]
    public int Amount;

    [Tooltip(@"can new objects be created in case there are none left?")]
    public bool canGrow;

    [Tooltip(@"false - objects must be created manually using Populated method.
true -  objects will be created automatically on awake")]
    public bool CreateOnAwake;
}
