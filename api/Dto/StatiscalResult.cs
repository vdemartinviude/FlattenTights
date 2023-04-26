using MathNet.Numerics.Statistics;

namespace api.Dto;

public class StatiscalResult
{
    public List<double> Values { get; set; }
    public List<Bin> bins { get; set; }
    public List<CurvePoint> curvePoints { get; set; }
    public double mean { get; set; }    
    public double stddev { get; set; }
    public double max { get; set; } 
    public double min { get; set; }
}

public class Bin
{
    public double LowerBound { get; set;}
    public double UpperBound { get; set;}
    public double Count { get; set;}
}

public class CurvePoint
{
    public double X { get; set; }
    public double Y { get; set; }
}