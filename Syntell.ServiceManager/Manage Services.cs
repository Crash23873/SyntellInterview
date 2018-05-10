using System;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Syntell.ServiceManager
{
    public class ServiceManger
    {

        class Program
        {
            static string BingMapsKey = "insertYourBingMapsKey";
            static void Main(string[] args)
            {
                try
                {
                    string locationsRequest = CreateRequest("New%20York");
                    Response locationsResponse = MakeRequest(locationsRequest);
                    ProcessResponse(locationsResponse);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Read();
                }

            }

            //Create the request URL
            public static string CreateRequest(string queryString)
            {
                string UrlRequest = "http://dev.virtualearth.net/REST/v1/Locations/" +
                                               queryString +
                                               "?key=" + BingMapsKey;
                return (UrlRequest);
            }

            public static Response MakeRequest(string requestUrl)
            {
                try
                {
                    HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                        object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                        Response jsonResponse
                        = objResponse as Response;
                        return jsonResponse;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }

            }

            static public void ProcessResponse(Response locationsResponse)
            {

                int locNum = locationsResponse.ResourceSets[0].Resources.Length;

                //Get formatted addresses: Option 1
                //Get all locations in the response and then extract the formatted address for each location
                Console.WriteLine("Show all formatted addresses");
                for (int i = 0; i < locNum; i++)
                {
                    Location location = (Location)locationsResponse.ResourceSets[0].Resources[i];
                    Console.WriteLine(location.Address.FormattedAddress);
                }
                Console.WriteLine();

                //Get the Geocode Points for each Location
                for (int i = 0; i < locNum; i++)
                {
                    Location location = (Location)locationsResponse.ResourceSets[0].Resources[i];
                    Console.WriteLine("Geocode Points for " + location.Address.FormattedAddress);
                    int geocodePointNum = location.GeocodePoints.Length;
                    for (int j = 0; j < geocodePointNum; j++)
                    {
                        Console.WriteLine("    Point: " + location.GeocodePoints[j].Coordinates[0].ToString() + "," +
                                                     location.GeocodePoints[j].Coordinates[1].ToString());
                        double test = location.GeocodePoints[j].Coordinates[1];
                        Console.Write("    Usage: ");
                        for (int k = 0; k < location.GeocodePoints[j].UsageTypes.Length; k++)
                        {
                            Console.Write(location.GeocodePoints[j].UsageTypes[k].ToString() + " ");
                        }
                        Console.WriteLine("\n\n");
                    }
                }
                Console.WriteLine();


                //Get all locations that have a MatchCode=Good and Confidence=High
                Console.WriteLine("Locations that have a Confidence=High");
                for (int i = 0; i < locNum; i++)
                {
                    Location location = (Location)locationsResponse.ResourceSets[0].Resources[i];
                    if (location.Confidence == "High")
                        Console.WriteLine(location.Address.FormattedAddress);
                }
                Console.WriteLine();

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();


            }

            [DataContract]
            public class Response
            {
                [DataMember(Name = "copyright")]
                public string Copyright { get; set; }
                [DataMember(Name = "brandLogoUri")]
                public string BrandLogoUri { get; set; }
                [DataMember(Name = "statusCode")]
                public int StatusCode { get; set; }
                [DataMember(Name = "statusDescription")]
                public string StatusDescription { get; set; }
                [DataMember(Name = "authenticationResultCode")]
                public string AuthenticationResultCode { get; set; }
                [DataMember(Name = "errorDetails")]
                public string[] errorDetails { get; set; }
                [DataMember(Name = "traceId")]
                public string TraceId { get; set; }
                [DataMember(Name = "resourceSets")]
                public ResourceSet[] ResourceSets { get; set; }
            }


            [DataContract]
            public class ResourceSet
            {
                [DataMember(Name = "estimatedTotal")]

                public long EstimatedTotal { get; set; }
                [DataMember(Name = "resources")]
                public Location[] Resources { get; set; }
            }

            [DataContract]
            public class Point
            {
                /// <summary>
                /// Latitude,Longitude
                /// </summary>
                [DataMember(Name = "coordinates")]
                public double[] Coordinates { get; set; }
            }


            [DataContract]
            public class BoundingBox
            {
                [DataMember(Name = "southLatitude")]
                public double SouthLatitude { get; set; }
                [DataMember(Name = "westLongitude")]
                public double WestLongitude { get; set; }
                [DataMember(Name = "northLatitude")]
                public double NorthLatitude { get; set; }
                [DataMember(Name = "eastLongitude")]
                public double EastLongitude { get; set; }
            }

            [DataContract]
            public class GeocodePoint : Point
            {
                [DataMember(Name = "calculationMethod")]
                public string CalculationMethod { get; set; }
                [DataMember(Name = "usageTypes")]
                public string[] UsageTypes { get; set; }
            }

            [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1")]
            public class Location
            {
                [DataMember(Name = "boundingBox")]
                public BoundingBox BoundingBox { get; set; }
                [DataMember(Name = "name")]
                public string Name { get; set; }
                [DataMember(Name = "point")]
                public Point Point { get; set; }
                [DataMember(Name = "entityType")]
                public string EntityType { get; set; }
                [DataMember(Name = "address")]
                public Address Address { get; set; }
                [DataMember(Name = "confidence")]
                public string Confidence { get; set; }
                [DataMember(Name = "geocodePoints")]
                public GeocodePoint[] GeocodePoints { get; set; }
                [DataMember(Name = "matchCodes")]
                public string[] MatchCodes { get; set; }
            }

            [DataContract]
            public class Address
            {
                [DataMember(Name = "addressLine")]
                public string AddressLine { get; set; }
                [DataMember(Name = "adminDistrict")]
                public string AdminDistrict { get; set; }
                [DataMember(Name = "adminDistrict2")]
                public string AdminDistrict2 { get; set; }
                [DataMember(Name = "countryRegion")]
                public string CountryRegion { get; set; }
                [DataMember(Name = "formattedAddress")]
                public string FormattedAddress { get; set; }
                [DataMember(Name = "locality")]
                public string Locality { get; set; }
                [DataMember(Name = "postalCode")]
                public string PostalCode { get; set; }
            }
        }

    }
}