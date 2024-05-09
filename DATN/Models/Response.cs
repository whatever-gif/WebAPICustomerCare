using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class Response<T>
{
    public bool Success { get; set; } = false;

    public int Count { get; set; } = 0;


    public List<T> DataList { get; set; } = new List<T>();

    public T Data { get; set; }


    public object Error { get; set; } = new { };

    public object ErrorDetail { get; set; } = new { };

}


