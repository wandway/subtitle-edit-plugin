using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace WordMarkSection
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Application();
            app.Options.CheckGrammarWithSpelling = true;
            app.Options.SuggestSpellingCorrections = true;
            app.Visible = true;
            var doc = app.Documents.Add();

            var lines = new List<String> { 
                "Hello Word",
                "第二句话",
                "世界属于你们"
            };
            var start = 0;
            var end = 0;
            var range = doc.Range(start, end);
            var str = "";
            for (int i = 0; i < lines.Count; i++)
            {
                str = lines[i];
                end = str.Length + start;
                Console.WriteLine(string.Format("start: {0}, end: {1}, text:{2}", start, end, str));
                range = doc.Range(start, end);
                range.Text = str;
                range.Comments.Add(range, (i+1).ToString());
                start = end + 1;
            }

            // 在第一句话后面添加内容，看最终通过批注反查原文时能否获得正确的内容
            doc.Range(0, 9).InsertAfter("在第一句末尾添加的内容");

            Console.WriteLine("批注数量 ：{0}", doc.Comments.Count);
            // Comments的索引从1开始
            // comment.Range表示的是注释的内容
            range = doc.Comments[1].Range;
            Console.WriteLine(string.Format("第一句注释：{0}", range.Text));

            str = doc.Comments[1].Reference.Text;
            Console.WriteLine(string.Format("第一句话：{0}", str));

            var a = doc.Comments[1].Reference.Start;
            Console.WriteLine(string.Format("第一句注释开始：{0}", a));

            a = doc.Comments[2].Reference.Start;
            Console.WriteLine(string.Format("第二句注释开始：{0}", a));

            start = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                end = doc.Comments[i + 1].Reference.Start;
                range = doc.Range(start, end);
                Console.WriteLine(string.Format("第一句话：{0}", range.Text));
                start = end + 1;
            }
            Console.WriteLine("word mark section");
            Console.ReadKey();
        }
    }
}
