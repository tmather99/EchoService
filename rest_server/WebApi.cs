// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using CoreWCF;
using Serilog;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class WebApi : IWebApi
    {
        public string Version()
        {
            return "rest_server:v1";
        }

        public string PathEcho(string param)
        {
            Log.Information(param);
            return param;
        }

        public string QueryEcho(string param)
        {
            Log.Information(param);
            return param;
        }

        public ExampleContract BodyEcho(ExampleContract param)
        {
            Log.Information(this.JsonSerialize(param));
            return param;
        }

        private string JsonSerialize<T>(T thing)
        {
            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            var sw = new StringWriter();
            var writer = new Newtonsoft.Json.JsonTextWriter(sw);
            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
            jsonSerializer.Serialize(writer, thing);
            return sw.ToString();
        }
    }
}
