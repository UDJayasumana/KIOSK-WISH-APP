using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetRequest
{
   IEnumerator GetRequest(string url, Action<string> callback = null);
}
