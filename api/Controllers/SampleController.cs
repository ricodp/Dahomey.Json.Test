using System;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace api.Controllers
{
    public abstract record MonitoringRunSample();
    public record BoreSample(
        LocalTime Sampled,
        string WeatherConditions,
        string Comments,
        string SampleMethod,
        bool BorePurged,
        string PurgeNotes,
        Measurement2[] Measurements,
        string Containers,
        (Guid suiteOrParameter, bool unableToSample)[] LabSuitesOrParameters
    ) : MonitoringRunSample;
    public abstract record Measurement2();
    public record MonitoringRunMeasurement(Guid ParameterId, decimal Value, string Comments) : Measurement2;

    public record AddSample(
        Guid MonitoringRun,
        Guid Point,
        MonitoringRunSample Sample
    );

    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        public SampleController()
        {
        }

        [HttpPost("/AddSample")]
        public AddSample Post([FromBody] AddSample sample)
        {
            // Post: See Below
            return sample;
        }

        [HttpPost("/LocalTime")]
        public LocalTime Post([FromBody] LocalTime time)
        {
            //POST: "11:59:45"
            return time;
        }
    }
}

// POST: 
// {
//     "monitoringRun": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//     "point": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//     "sample": {
//         "name": "yyrtesy",
//         "sampleTime": "09:40:28.252+08:00",
//         "files": [],
//         "selectedFiles": [],
//         "samplers": [],
//         "unableToSample": false,
//         "createDuplicate": false,
//         "sampled": "16:59:45.664",
//         "weatherConditions": "",
//         "comments": "dfgh",
//         "sampleMethod": "",
//         "borePurged": false,
//         "purgeNotes": "",
//         "measurements": [
//             {
//                 "parameterId": "6ed6e524-2ca4-4fea-8e21-7245ccb61863",
//                 "value": 5,
//                 "comments": "",
//                 "$type": "MonitoringRunMeasurement"
//             },            
//             {
//                 "parameterId": "39f51d33-5fc7-4f10-aa6a-d51a6696aa36",
//                 "comments": "",
//                 "value": 4,
//                 "unit": "d471c77d-5412-4e7a-a98d-8304e87792ed",
//                 "unableToSample": false,
//                 "$type": "MonitoringRunMeasurement"
//             }
//         ],
//         "containers": "",
//         "labSuitesOrParameters": [["002f5c32-69d1-402c-897f-c3b58b6ba86b", false], ["002f5c32-69d1-402c-897f-c3b58b6ba86b", false]],
//         "$type": "BoreSample"
//     }
// }