using System;
using System.Collections.Generic;
using CsvHelper;

namespace LuisTrainer
{
    public static class CsvReaderExtensions
    {
        public static string GetFieldOr(this CsvReader self, int index, string elseValue)
        {
            try
            {
                return self[index];
            }
            catch (Exception)
            {
                return elseValue;
            }
        }

        /// <summary>
        /// ヘッダ行のつもりで1行読んで、列名とインデックスのマップを返す
        /// </summary>
        public static IDictionary<String, int> ReadAsHeader(this CsvReader self)
        {
            var cols = new Dictionary<String, int>(); // 列名と列indexのマップ
            if (self.Read())
            {
                try
                {
                    // 列数が取れないのでエラーになるまでインクリメントする
                    var i = 0;
                    while (true)
                    {
                        cols.Add(self[i], i);

                        i++;
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            return cols;
        }
    }
}
