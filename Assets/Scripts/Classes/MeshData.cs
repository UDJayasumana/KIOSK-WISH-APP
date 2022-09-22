using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData<TGameObject, TStartPoint, TEndPoint>
{
    public TGameObject GameObject { get; set; }
    public TStartPoint StartPoint { get; set; }
    public TEndPoint EndPoint { get; set; }
}
