using System.Text.Json.Nodes;

namespace routines.package.docker.Utils
{
    static internal class InternalFunctions
    {

        public static JsonArray AddJsonsToArray(string[] lines)
        {
            //test

            JsonArray jsonArray = new JsonArray();

            foreach (string line in lines)
            {

                jsonArray.Add(JsonNode.Parse(line));
            }

            return jsonArray;
        }

        public static string[] NdJsonSplitter(string input)
        {

            string[] lines = input
           .Split([ "\r", "\n" ], StringSplitOptions.RemoveEmptyEntries);

            return lines;

        }
    }
}

