using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WishesContainer
{
    public int status;
    public string desc;
    public int row_start;
    public int row_end;
    public int row_count;

    public List<WishData> data;

}
