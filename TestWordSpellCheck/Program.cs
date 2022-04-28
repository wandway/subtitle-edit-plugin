using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace TestWordSpellCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Application();
            app.Options.CheckGrammarWithSpelling = true;
            app.Options.SuggestSpellingCorrections = true;
            app.Visible = false;
            try
            {
                var doc = app.Documents.Add();
                doc.Content.Text = "Hellx worlc!祖国富强，安居乐叶。Dream green grasx.";
                doc.CheckSpelling();
                var text = doc.Range().Text;
                Console.WriteLine("修改后的文档：" + text);
                doc.Close(false); // 需要关闭文档，否则会创建很多wps word进程。
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("发生错误");
            }
            finally
            {
                app.Quit(false);
            }
        }
    }
}
