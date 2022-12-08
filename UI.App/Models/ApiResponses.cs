using System.ComponentModel.DataAnnotations;

namespace UI.App.Models
{
    // {
    //    "id": 105,
    //    "employer_name": "IGM Hospital",
    //    "member_name": "Ahmed Mohamed",
    //    "nic": "M000001",
    //    "start_date": "2019-02-01T00:00:00.000Z",
    //    "end_date": "2020-01-31T00:00:00.000Z"
    //}
public class ResponseEmployment
    {
        public int id { get; set; }
        public string employer_name { get; set; }
        public string member_name { get; set; }
        public string nic { get; set; }
        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }
    }

    public class ResponseToken
    {
        public string access_token { get; set; }
    }
    public class Response400 // bad request
    {
        public int statusCode { get; set; }
        public List<string> message { get; set; }
        public string error { get; set; }
    }

    public class Response403 // forbidden
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public string error { get; set; }
    }

}
