using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace LoTable_LocalizationExtracter
{
    class Program
    {
        public class Row
        {
            public Row(string Id) {
                this.Id = Id;
            }
            public string Id { get; set; }
            public string KO { get; set; }
            public string JP { get; set; }
            public string EN { get; set; }
            public string TC { get; set; }
        }
        static int Main(string[] args)
        {
            string[] files =
            {
                "Table_Localization_ko.bin.bytes",
                "Table_Localization_ja.bin.bytes",
                "Table_Localization_en.bin.bytes",
                "Table_Localization_tc.bin.bytes",
            };
            System.Collections.Generic.Dictionary<string, Row> final_d = new System.Collections.Generic.Dictionary<string, Row>();
            for (int i = 0; i < files.Length; i++)
            {
                string filepath = files[i];
                if (!File.Exists(filepath))
                {

                    Console.WriteLine($"File {filepath} not found.");
                    continue;
                }
                StreamReader streamReader = new StreamReader(filepath);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                System.Collections.Generic.Dictionary<string, System.Object> d = (System.Collections.Generic.Dictionary<string, System.Object>)binaryFormatter.Deserialize(streamReader.BaseStream);
                foreach (var kvp in d) {
                    if (!final_d.ContainsKey(kvp.Key)) {
                        final_d.Add(kvp.Key, new Row(kvp.Key));
                    }
                    switch (i) { 
                        case 0:
                            final_d[kvp.Key].KO = (string)kvp.Value;
                            break;
                        case 1:
                            final_d[kvp.Key].JP = (string)kvp.Value;
                            break;
                        case 2:
                            final_d[kvp.Key].EN = (string)kvp.Value;
                            break;
                        case 3:
                            final_d[kvp.Key].TC = (string)kvp.Value;
                            break;

                    }
                    
                }
                
            }

            string savecsv = "final.csv";
            using (var writer = new StreamWriter(savecsv))
            using (CsvHelper.CsvWriter csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<Row>();
                csv.NextRecord();
                csv.WriteRecords(final_d.Select(kvp => kvp.Value).ToList());
            }

            return 0;
        }
    }
}
