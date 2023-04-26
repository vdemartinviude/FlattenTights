using MongoDB.Bson.Serialization.Attributes;

namespace api.Model;


[BsonIgnoreExtraElements]
public class Tight
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string _id { get; set; }
    public long Count { get; set; }
    public Message Message { get; set; }
}

[BsonIgnoreExtraElements]
public class Message
{
    public Response response { get; set; }
}

[BsonIgnoreExtraElements]
public class Response
{
    public string deviceId { get; set; }
    public string functionalOperationId { get; set; }
    public string group { get; set; }
    public bool reverse { get; set; }
    public string partNumber { get; set; }
    public string serialNumber { get; set; }
    public string timeStatus { get; set; }
    public Resultlist[] resultList { get; set; }
}
[BsonIgnoreExtraElements]
public class Resultlist
{
    public string status { get; set; }
    public int channel { get; set; }
    public int program { get; set; }
    public object pointDesignation { get; set; }
    public string? pointNumber { get; set; }
    public string torqueControllerName { get; set; }
    public string numberIdChannel { get; set; }
    public long nutCounter { get; set; }
    public string nutCounterLastMaintenance { get; set; }
    public string maintenanceAlert { get; set; }
    public string timeStampLastMaintenance { get; set; }
    public Stepslist[] stepsList { get; set; }
}
[BsonIgnoreExtraElements]
public class Stepslist
{
    public string stepStatus { get; set; }
    public long sequence { get; set; }
    public long ssc { get; set; }
    public object[] codeList { get; set; }
    public string strategy { get; set; }
    public Parameterlist[] parameterList { get; set; }
}
[BsonIgnoreExtraElements]
public class Parameterlist
{
    public string parameter { get; set; }
    public string unit { get; set; }
    public double value { get; set; }
}
