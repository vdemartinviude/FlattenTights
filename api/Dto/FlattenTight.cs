namespace api.Dto;



public class FlattenTight
{
    public string deviceId { get; set; }
    public DateTime timeStatus { get; set; }
    public TimeOnly time { get; set; }
    public string globalStatus { get; set; }
    public DateOnly date { get; set; }
    public bool multiple { get; set; }
    public string partNumber { get; set; }
    public string serialNumber { get; set; }
    public string functionalOperationId { get; set; }
    public long channel { get; set; }
    public long nutCounter { get; set; }
    public long pointNumber { get; set; }
    public string TorqueControllerName { get; set; }
    public string timeStampLastMaintenance { get; set; }
    public double? ActualAngleValue { get; set; }
    public double? ActualTimeValue { get; set; }
    public double? ActualTorqueValue { get; set; }
    public double? AngleThresholdValue { get; set; }
    public double? LowerAngleTolerance { get; set; }
    public double? LowerTimeTolerance { get; set; }
    public double? LowerTorqueLimit { get; set; }
    public double? LowerTorqueTolerance { get; set; }
    public long Sequence { get; set; }
    public long Program { get; set; }
    public long Ssc { get; set; }
    public string StepDesignation { get; set; }
    public string StepStatus { get; set; }
    public string Strategy { get; set; }
    public double? TargetAngleValue { get; set; }
    public double? TargetTorqueValue { get; set; }
    public double? TorqueThresholdValue { get; set; }
    public string TranslatedStrategy { get; set; }
    public double? UpperAngleTolerance { get; set; }
    public double? UpperTimeTolerance { get; set; }
    public double? UpperTorqueLimit { get; set; }
    public double? UpperTorqueTolerance { get; set; }
}