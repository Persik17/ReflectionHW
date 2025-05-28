using ReflectionHW.Classes;
using System.Diagnostics;
using System.Text.Json;
using ReflectionHW.Serializers;
using System.Text;

namespace ReflectionHW
{
    public class Program
    {
        public static void Main(string[] args)
        {
            F obj = F.Get();
            int iterations = 10000;

            Stopwatch sw = Stopwatch.StartNew();
            string csvData = "";
            for (int i = 0; i < iterations; i++)
            {
                csvData = CSVSerializer.Serialize(obj);
            }
            sw.Stop();

            Console.WriteLine($"{nameof(CSVSerializer)} Serialized string: {csvData}");
            Console.WriteLine($"{nameof(CSVSerializer)} Time for {iterations} iterations = {sw.ElapsedMilliseconds} ms");

            StringBuilder sb = new();
            for (int i = 0; i < iterations; i++)
            {
                sb.AppendLine(csvData);
            }

            Stopwatch consoleSw = Stopwatch.StartNew();
            Console.WriteLine(sb.ToString());
            consoleSw.Stop();

            Stopwatch jsonSw = Stopwatch.StartNew();
            string jsonData = "";
            for (int i = 0; i < iterations; i++)
            {
                jsonData = JsonSerializer.Serialize(obj);
            }
            jsonSw.Stop();

            Console.WriteLine($"{nameof(JsonSerializer)} Serialized string: {jsonData}");
            Console.WriteLine($"{nameof(JsonSerializer)} Time for {iterations} iterations = {jsonSw.ElapsedMilliseconds} ms");

            Stopwatch deserializeSw = Stopwatch.StartNew();
            F deserializedObj = null;
            for (int i = 0; i < iterations; i++)
            {
                deserializedObj = CSVSerializer.Deserialize<F>(csvData);
            }
            deserializeSw.Stop();

            Console.WriteLine($"{nameof(CSVSerializer)} Time for {iterations} iterations = {deserializeSw.ElapsedMilliseconds} ms");
            Console.WriteLine($"{nameof(CSVSerializer)} Deserialized object i1 = {deserializedObj.I1}, i2 = {deserializedObj.I2}, i3 = {deserializedObj.I3}, i4 = {deserializedObj.I4}, i5 = {deserializedObj.I5}, i6 = {deserializedObj.I6}");

            Stopwatch jsonDeserializeSw = Stopwatch.StartNew();
            F jsonDeserializedObj = null;
            for (int i = 0; i < iterations; i++)
            {
                jsonDeserializedObj = JsonSerializer.Deserialize<F>(jsonData);
            }
            jsonDeserializeSw.Stop();

            Console.WriteLine($"{nameof(JsonSerializer)} Time for {iterations} iterations = {jsonDeserializeSw.ElapsedMilliseconds} ms");
            Console.WriteLine($"{nameof(JsonSerializer)} Deserialized object i1 = {jsonDeserializedObj.I1}, i2 = {jsonDeserializedObj.I2}, i3 = {jsonDeserializedObj.I3}, i4 = {jsonDeserializedObj.I4}, i5 = {jsonDeserializedObj.I5}, i6 = {jsonDeserializedObj.I6}");

            string filePath = "data.csv";

            try
            {
                File.WriteAllText(filePath, csvData);

                Stopwatch loadSw = Stopwatch.StartNew();
                F loadedObj = CSVSerializer.LoadFromCsvFile<F>(filePath);
                loadSw.Stop();

                Console.WriteLine($"Time to load from CSV file: {loadSw.ElapsedMilliseconds} ms");
                Console.WriteLine($"Loaded from CSV file: i1 = {loadedObj.I1}, i2 = {loadedObj.I2}, i3 = {loadedObj.I3}, i4 = {loadedObj.I4}, i5 = {loadedObj.I5}, i6 = {loadedObj.I6}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при работе с файлом: {ex.Message}");
            }
        }
    }
}