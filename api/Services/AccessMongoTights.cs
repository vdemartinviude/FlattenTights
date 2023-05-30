using api.ConfigOptions;
using api.Dto;
using api.FlattenService;
using api.Mapster;
using api.Model;
using api.Request;
using Mapster;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Services;

public class AccessMongoTights
{
    private readonly IMongoCollection<Tight> _tightsCollection;
    private readonly ILogger<AccessMongoTights> _logger;
    private readonly FlatService _flattenService;
    public AccessMongoTights(ILogger<AccessMongoTights> logger, IConfiguration configuration, FlatService flattenService)
    {

        _logger = logger;
        var mongoclient = new MongoClient(configuration.GetValue<string>("MONGODBCONNECTIONSTRING"));
        var mongoDatabase = mongoclient.GetDatabase(configuration.GetValue<string>("MONGODBDATABASE"));
        _tightsCollection = mongoDatabase.GetCollection<Tight>(configuration.GetValue<string>("MONGODBTIGHTSCOLLECTION")); 

        _logger.LogInformation("Mongo Connected");
        _flattenService = flattenService;
    }

    internal async Task<StatiscalResult> GetAnalisys(CancellationToken cancellation)
    {
        var sourceList = await GetTights(cancellation);
        var res = sourceList
            .SelectMany(mes => mes.Message.response.resultList
            .SelectMany(res => res.stepsList
            .Select(step => step.parameterList.Where(p => p.parameter == "MI" && res.status=="OK" && res.torqueControllerName== "LM01C_IP123" && res.program==2).SingleOrDefault())
            .Where(parameter => parameter != null && parameter.value < 1000 && parameter.value>0)
            .Select(pararameter => pararameter.value)));

        var res2 = sourceList
            .SelectMany(mes => mes.Message.response.resultList
            .SelectMany(res => res.stepsList
            .Select(step => step.parameterList.Where(p => p.parameter == "MI" && res.status == "OK").SingleOrDefault())
            .Where(parameter => parameter != null && parameter.value < 0)
            .Select(x => new
            {
                mes.Message.response.deviceId,
                mes.Message.response.partNumber,
                mes.Message.response.serialNumber,
                mes.Message.response.timeStatus,
                res.program,
                res.torqueControllerName,
                res.status,
            }))).ToList();



        var statistics = new DescriptiveStatistics(res);
        int numBins = (int)Math.Ceiling(Math.Log(res.ToList().Count, 2) + 1);
        Histogram histogram = new Histogram(res, numBins);  



        var bins = new List<Bin>();

        var points = new List<CurvePoint>();


        var xmin = statistics.Mean - 3 * statistics.StandardDeviation;
        var xmax = statistics.Mean + 3 * statistics.StandardDeviation;
        var xstep = (xmax - xmin) / 99;
        var normalDist = new Normal(statistics.Mean, statistics.StandardDeviation);
        for(int i = 0; i < 100; i++)
        {
            points.Add(new CurvePoint()
            {
                X = i * xstep + xmin,
                Y = normalDist.Density(i * xstep + xmin)
            });
        }
        for(int i = 0; i < histogram.BucketCount;i++)
        {
            bins.Add(new Bin
            {
                Count = histogram[i].Count,
                LowerBound = histogram[i].LowerBound,
                UpperBound = histogram[i].UpperBound
            });
        }
        return new StatiscalResult
        {
            Values = res.ToList(),
            curvePoints = points,
            mean = statistics.Mean,
            stddev = statistics.StandardDeviation,
            max = statistics.Maximum,
            min = statistics.Minimum,
            bins = bins
        };
    }

    internal async Task<IEnumerable<FlattenTight>> GetFlatten2(Request.FlattenDataRequest request, CancellationToken cancellation)
    {

        DateTime startDate = request.date.Date;
        DateTime endDate = startDate.AddDays(1);

        Console.WriteLine(request.date.Date.ToShortDateString());

        FilterDefinition<Tight> fiter = Builders<Tight>.Filter.Regex("Message.response.timeStatus", new BsonRegularExpression(startDate.ToString("dd-MM-yyyy")));

        var cursor = await _tightsCollection.Find(fiter, new()
        {
            BatchSize = 5000,
        }).ToCursorAsync();

        await cursor.MoveNextAsync();        

        return _flattenService.Flat(cursor.Current);
    }

    internal Task<IEnumerable<FlattenTight>> GetFlatten3(FlattenDataRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    internal async Task<IEnumerable<FlattenTight>> GetFlattenTights(CancellationToken cancellation)
    {
        var returnList = new List<Tight>();
        var options = new FindOptions<Tight>()
        {
            BatchSize = 2000,
            Limit = 2000,
            Skip = 0
        };
        using (var cursor = await _tightsCollection.FindAsync<Tight>(x => true, options, cancellation))
        {
            await cursor.MoveNextAsync(cancellation);
            var listTight = cursor.Current.ToList();
            returnList.AddRange(listTight);

        }
        return _flattenService.Flat(returnList);

    }

    internal async Task<StatiscalResult> GetHistogram(HistogramRequest request, CancellationToken cancellation)
    {
        var statistics = new DescriptiveStatistics(request.Data);
        int numBins = (int)Math.Ceiling(Math.Log(request.Data.Count(), 2) + 1);
        Histogram histogram = new Histogram(request.Data, numBins);

        var bins = new List<Bin>();

        var points = new List<CurvePoint>();

        var xmin = statistics.Mean - 3 * statistics.StandardDeviation;
        var xmax = statistics.Mean + 3 * statistics.StandardDeviation;

        var xstep = (xmax - xmin) / 99;
        var normalDist = new Normal(statistics.Mean, statistics.StandardDeviation);
        for (int i = 0; i < 100; i++)
        {
            points.Add(new CurvePoint()
            {
                X = i * xstep + xmin,
                Y = normalDist.Density(i * xstep + xmin)
            });
        }
        for (int i = 0; i < histogram.BucketCount; i++)
        {
            bins.Add(new Bin
            {
                Count = histogram[i].Count,
                LowerBound = histogram[i].LowerBound,
                UpperBound = histogram[i].UpperBound
            });
        }
        return new StatiscalResult
        {
            Values = request.Data.ToList(),
            curvePoints = points,
            mean = statistics.Mean,
            stddev = statistics.StandardDeviation,
            max = statistics.Maximum,
            min = statistics.Minimum,
            bins = bins
        };

    }

    internal async Task<List<Tight>> GetTights(CancellationToken cancellation)
    {
        var returnList = new List<Tight>();
        var options = new FindOptions<Tight>()
        {
            BatchSize = 20000,
            Limit = 20000,
            Skip = 0,
            
            

        };


        using (var cursor = await _tightsCollection.FindAsync<Tight>(x => true,options,cancellation))
        {
            while (await cursor.MoveNextAsync(cancellation))
            {
                returnList.AddRange(cursor.Current.ToList());
            }
        }


        return returnList;
    }
}
