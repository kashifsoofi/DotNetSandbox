using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Wmhelp.XPath2;

namespace XPath2Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var input = File.ReadAllText("test.xml");
                var nav = new XmlDocument { InnerXml = input }.CreateNavigator();

                var nsResolver = new XmlNamespaceManager(new NameTable());
                nsResolver.AddNamespace("s", "http://www.w3.org/2001/12/soap-envelope");
                nsResolver.AddNamespace("m", "http://www.xyz.org/todo-list");

                // var pattern = "//TenantInfoByTenantID/iTenantID[text()='80964']";
                // var pattern = "//m:todo-item[text() = 'abc']";
                // var pattern = "//m:todo-list[count(m:todo-item) = 1]";

                var pattern = "//m:todo-list[count(m:todo-item[text() = 'abc']) = 1]";
                var v1 = nav.Evaluate(pattern, nsResolver);
                var result = (bool) nav.Evaluate($"boolean({pattern})", nsResolver);
                if (result)
                    Console.WriteLine("found");
                else
                    Console.WriteLine("Not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
