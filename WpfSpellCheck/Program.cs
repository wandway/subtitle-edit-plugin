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
                doc.Range().Text = "Hellx worlc!祖国富强，安居乐叶。\nDream green grasx.";
                var ranges = doc.Words.Cast<Range>();
                foreach (var word in ranges)
                {
                    if (word.SpellingErrors.Count > 0)
                    {
                        foreach (var error in word.SpellingErrors)
                        {
                            Console.WriteLine("error", error.ToString());
                        }
                        Console.WriteLine(word.Text);
                        var suggestions = word.GetSpellingSuggestions("custom.dic", string.Empty, "english");
                        if (suggestions != null)
                        {
                            foreach (var suguesstion in suggestions)
                            {
                                Console.WriteLine("修改建议：" + suguesstion.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("没有修改建议");
                        }
                    }
                }
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
