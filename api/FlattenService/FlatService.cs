using Amazon.Runtime.Internal.Transform;
using Amazon.SecurityToken;
using api.ConfigOptions;
using api.Dto;
using api.Model;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Globalization;

namespace api.FlattenService;

public class FlatService
{
    public IEnumerable<FlattenTight> Flat(IEnumerable<Tight> tights)
    {


        DateTime time;

        return tights.SelectMany(resp => resp.Message.response.resultList
                     .SelectMany(result => result.stepsList.Select(
                                 step => {
                                     
                                     var time = DateTime.ParseExact(resp.Message.response.timeStatus, "dd-MM-yyyy HH:mm:ss.fff", new CultureInfo("pt-br"));
                                     return new FlattenTight
                                     {
                                         deviceId = resp.Message.response.deviceId,
                                         timeStatus = time,
                                         time = TimeOnly.FromDateTime(time),
                                         globalStatus = resp.Message.response.resultList.Any(x => x.status!="OK") ? "OK" : "NOK",
                                         date = DateOnly.FromDateTime(time),
                                         multiple = resp.Message.response.resultList.Length > 0,
                                         partNumber = resp.Message.response.partNumber,
                                         serialNumber = resp.Message.response.serialNumber,
                                         functionalOperationId = resp.Message.response.functionalOperationId,
                                         channel = result.channel,
                                         nutCounter = result.nutCounter,
                                         pointNumber = Convert.ToInt64(result.pointNumber ?? "0"),
                                         TorqueControllerName = result.torqueControllerName,
                                         timeStampLastMaintenance = result.timeStampLastMaintenance,
                                         ActualTorqueValue = step.parameterList.SingleOrDefault(x => x.parameter == "MI")?.value ?? null,
                                         ActualTimeValue = step.parameterList.SingleOrDefault(x => x.parameter == "TI")?.value ?? null,
                                         ActualAngleValue = step.parameterList.SingleOrDefault(x => x.parameter == "WI")?.value ?? null,
                                         LowerAngleTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "W_LOWER")?.value ?? null,
                                         LowerTimeTolerance = step.parameterList.SingleOrDefault(x => x.parameter =="T_LOWER")?.value ?? null,
                                         LowerTorqueLimit = step.parameterList.SingleOrDefault(x => x.parameter == "MU")?.value ?? null,
                                         LowerTorqueTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "M_LOWER")?.value ?? null,
                                         Program = result.program,
                                         Sequence = step.sequence,
                                         AngleThresholdValue = step.parameterList.SingleOrDefault(x => x.parameter == "WS")?.value ?? null,
                                         Ssc = step.ssc,
                                         StepDesignation = TranslateDesignation(step.stepStatus),
                                         StepStatus = step.stepStatus,
                                         Strategy = step.strategy,
                                         TargetAngleValue = step.parameterList.SingleOrDefault(x => x.parameter == "WA")?.value ?? null,
                                         TargetTorqueValue = step.parameterList.SingleOrDefault(x => x.parameter == "MA")?.value ?? null,
                                         TorqueThresholdValue = step.parameterList.SingleOrDefault(x => x.parameter == "WS")?.value ?? null,
                                         TranslatedStrategy = TranslateStrategy(step.strategy),
                                         UpperAngleTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "W_UPPER")?.value ?? null,
                                         UpperTimeTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "T_UPPER")?.value ?? null,
                                         UpperTorqueLimit = step.parameterList.SingleOrDefault(x => x.parameter == "MO")?.value ?? null,
                                         UpperTorqueTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "M_UPPER")?.value ?? null


                                     };
                                 })));
        
        
        
        
        //foreach (Tight tight in tights) 
        //{
        //    foreach(Resultlist result in tight.Message.response.resultList)
        //    {
        //        foreach(Stepslist step in result.stepsList)
        //        {
        //            if (step != null)
        //            resultg.Add(new FlattenTight
        //            {
        //                //ActualTorqueValue = step.parameterList.SingleOrDefault(x => x.parameter == "MI").value,
        //                //ActualTimeValue = step.parameterList.SingleOrDefault(x => x.parameter == "TI").value,
        //                //ActualAngleValue = step.parameterList.SingleOrDefault(x => x.parameter == "WI").value,
        //                //AngleThresholdValue = step.parameterList.SingleOrDefault(x => x.parameter == "WS").value,
        //                channel = result.channel,
        //                //date = DateOnly.FromDateTime(DateTime.ParseExact(tight.Message.response.timeStatus, "dd-MM-yyyy HH:mm:ss.fff", new CultureInfo("pt-BR"))),
        //                deviceId = tight.Message.response.deviceId,
        //                functionalOperationId = tight.Message.response.functionalOperationId,
        //                //globalStatus = tight.Message.response.resultList.Any(x => x.status != "OK") ? "NOK" : "OK",
        //                //LowerAngleTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "W_LOWER").value,
        //                //LowerTimeTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "T_LOWER").value,
        //                //LowerTorqueLimit = step.parameterList.SingleOrDefault(x => x.parameter == "MU").value,
        //                //LowerTorqueTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "M_LOWER").value,
        //                multiple = tight.Message.response.resultList.Length>1,
        //                nutCounter = result.nutCounter,
        //                partNumber = tight.Message.response.partNumber,
        //                pointNumber = Convert.ToInt32(result.pointNumber),
        //                Program = result.program,
        //                Sequence = step.sequence,
        //                serialNumber = tight.Message.response.serialNumber,
        //                Ssc = step.ssc,
        //                //StepDesignation = TranslateDesignation(step.stepStatus),
        //                StepStatus = step.stepStatus,
        //                Strategy = step.strategy,
        //                //TargetAngleValue = step.parameterList.SingleOrDefault(x => x.parameter == "WA").value,
        //                //TargetTorqueValue = step.parameterList.SingleOrDefault(x => x.parameter == "MA").value,
        //                //time = TimeOnly.FromDateTime(DateTime.ParseExact(tight.Message.response.timeStatus, "dd-MM-yyyy HH:mm:ss.fff", new CultureInfo("pt-BR"))),
        //                timeStampLastMaintenance = result.timeStampLastMaintenance,
        //                //timeStatus = DateTime.ParseExact(tight.Message.response.timeStatus, "dd-MM-yyyy HH:mm:ss.fff", new CultureInfo("pt-BR")),
        //                TorqueControllerName = result.torqueControllerName,
        //                TorqueThresholdValue = step.parameterList.SingleOrDefault(x => x.parameter == "WS").value,
        //                //TranslatedStrategy = TranslateStrategy(step.strategy),
        //                //UpperAngleTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "W_UPPER").value,
        //                //UpperTimeTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "T_UPPER").value,
        //                //UpperTorqueLimit = step.parameterList.SingleOrDefault(x => x.parameter == "MO").value,
        //                //UpperTorqueTolerance = step.parameterList.SingleOrDefault(x => x.parameter == "M_UPPER").value

        //            });
        //        }
        //    }
        //}
        //return resultg;
    }
    private static string TranslateDesignation(string step) => _stepDesignations[step];
    private static string TranslateStrategy(string strategy) => _strategies[strategy];
    private static readonly Dictionary<string, string> _stepDesignations = new()
    {
        { "AS", "Start-up step" },
        { "FS", "Finding step"},
        { "WS", "Waiting step" },
        { "LS", "Loosening step" },
        { "VS", "Initial tightening step" },
        { "ES", "Final tightening step" }
    };

    private static readonly Dictionary<string, string> _strategies = new()
    {
        {"AD","Torque tightening strategy" },
        {"AW","Angle tightening strategy" },
        {"ADW","Torque/angle strategy" },
        {"AF","Strategy to tap a thread for passage boreholes" },
        {"AZ","Strategy to define a waiting time" },
        {"AX","Strategy to external shut-down" },
    };
}
