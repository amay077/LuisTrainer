using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using CsvHelper;

namespace LuisTrainer
{
    public class LuisTrainerModule
    {
        private readonly string[] SpecifiedColumns = {
                "Text",
                "Status",
                "Update_ts",
                "Intent"
            };

        public void Run(AppConfig config)
        {
            var examples = ReadCsv(config.Path);
            Console.WriteLine($"Input csv has {examples.Count} rows.");

            var stream = File.OpenWrite(config.Path + ".out");
            var processed = 0;
            var skipped = 0;
            using (var writer = new StreamWriter(stream))
            {
                var csvOut = new CsvWriter(writer);
                csvOut.WriteHeader<Example>();
                csvOut.NextRecord();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", config.AppKey);

                    foreach (var exam in examples)
                    {
                        if (exam.Status > 0)
                        {
                            csvOut.WriteRecord(exam);
                            csvOut.NextRecord();
                            skipped++;
                            continue;
                        }

                        var luisExam = exam.ToLuisExample();
                        Console.WriteLine($"request: {luisExam.ToJson()}");
                        PostToLuis(config, client, luisExam);
                        exam.Status = 1;
                        exam.UpdateTs = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz");
                        csvOut.WriteRecord(exam);
                        csvOut.NextRecord();
                        processed++;
                    }
                }

                writer.Flush();
            }

            Console.WriteLine($"{processed} processed, {skipped} skipped.");
        }

        private IList<Example> ReadCsv(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var csvReader = new CsvReader(new StreamReader(stream));

                // ヘッダーを読む
                var header = csvReader.ReadAsHeader();

                ValidHeader(header);

                // 実データ群を読む
                var records = new List<Example>();
                while (csvReader.Read())
                {
                    var example = csvReader.Let(csv =>
                    {
                        var text = csv.GetFieldOr(header["Text"], string.Empty);
                        var intent = csv.GetFieldOr(header["Intent"], string.Empty);
                        var status = Convert.ToInt32(csv.GetFieldOr(header["Status"], "0"));
                        var updateTs = csv.GetFieldOr(header["Update_ts"], string.Empty);

                        var entitiyMap = new Dictionary<String, String>();
                        foreach (var col in header)
                        {
                            if (IsSpecifiedColumn(col.Key))
                            {
                                continue;
                            }

                            var entityValue = csv.GetFieldOr(col.Value, string.Empty);
                            if (!string.IsNullOrEmpty(entityValue))
                            {
                                entitiyMap.Add(col.Key, entityValue);
                            }
                        }

                        return new Example(
                            text,
                            status,
                            updateTs,
                            intent,
                            entitiyMap
                        );
                    });

                    records.Add(example);
                }

                //return csv.GetRecords<Example>().ToList();
                return records;
            }
        }

        private void ValidHeader(IDictionary<string, int> header)
        {
            foreach (var specCol in SpecifiedColumns)
            {
                if (!header.ContainsKey(specCol))
                {
                    throw new InvalidDataException($"ヘッダ行に {specCol} が必要です。");
                }
            }
        }

        private bool IsSpecifiedColumn(string colName)
        {
            return SpecifiedColumns.Contains(colName);
        }

        private void PostToLuis(AppConfig config, HttpClient client, LuisExample luisExample)
        {
            var appId = config.AppId;// "5169xxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
            var versionId = config.VersionId; // "0.1";
            var url = $"https://westus.api.cognitive.microsoft.com/luis/api/v2.0/apps/{appId}/versions/{versionId}/example";

            var text = luisExample.ToJson();

            var content = new StringContent(text, Encoding.UTF8, "application/json");

            var task = client.PostAsync(url, content);
            var result = task.Result;
            var message = result.Content.ReadAsStringAsync().Result;

            Console.WriteLine($"status = {result.StatusCode}, response= {message}");
        }    
    }
}
