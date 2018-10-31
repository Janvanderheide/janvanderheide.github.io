using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SLSInvoicer.Extractors;

namespace SLSInvoicerTest
{
    
    public class ExtractorsTests
    {
        [TestCase("3768 MB Soest     1013PW AMSTERDAM", "AMSTERDAM")]
        [TestCase("3768 MB Soest     1013PW AMSTERDAM-Zuid", "AMSTERDAM-Zuid")]
        [TestCase("3768MB Soest     1013PW AMSTERDAM Zuid oost   ", "AMSTERDAM Zuid oost")]
        [TestCase("3768 MB Soest     1013PW AMSTE'veld   ", "AMSTE'veld")]
        [TestCase("    3768 MB Soest     1013PW AMSTE'veld   ", "AMSTE'veld")]
        public void TestCityExtractor(string testCase, string result)
        {
            var city = CityExtractor.GetCity(testCase);

            Assert.That(city, Is.EqualTo(result));
        }

        [TestCase("3768 MB Soest     1013PW AMSTERDAM", "1013PW")]
        [TestCase("3728 MB Soest     1013PW AMSTERDAM", "3728 MB")]
        public void TestPostalCodeExtractor(string testCase, string result)
        {
            var postalCode = PostalCodeExtractor.GetPostalCode(testCase);

            Assert.That(postalCode, Is.EqualTo(result));
        }


        public static IEnumerable<TestCaseData> Convert_PositiveCases()
        {
            var testCases = new List<TestCaseData>();

            var dir = new DirectoryInfo(TestDataFolder);
            var files = dir.GetFiles();

            foreach (var filePath in files)
            {
                testCases.Add(new TestCaseData(filePath));
            }

            return testCases;
        }


        [Test]
        [TestCaseSource("Convert_PositiveCases")]
        public void TestPostalCodeExtractor(FileInfo fileInfo)
        {
            string text = System.IO.File.ReadAllText(fileInfo.FullName);
            var postalCode = PostalCodeExtractor.GetPostalCode(text);

            Console.WriteLine(postalCode);
            string expectedResult = "";
            GetPostalCodeList().TryGetValue(fileInfo.Name, out expectedResult);
            Assert.That(postalCode, Is.EqualTo(expectedResult));
        }

        public static string TestDataFolder = Path.Combine(PathProvider.AssemblyPath, "TestData");

        public Dictionary<string, string> GetPostalCodeList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("201700008.pdf.txt", "1601HB");
            dic.Add("201700036.pdf.txt", "3039DZ");
            dic.Add("201700051.pdf.txt", "1023CD");
            dic.Add("201700083.pdf.txt", "2515GC");
            return dic;
        }


        [Test]
        [TestCaseSource("Convert_PositiveCases")]
        public void TestCustomerNameExtractor(FileInfo fileInfo)
        {
            string text = System.IO.File.ReadAllText(fileInfo.FullName);
            var CustomerName = CustomerExtractor.GetCustomerName(text);

            Console.WriteLine(CustomerName);
            string expectedResult = "";
            GetCustomerList().TryGetValue(fileInfo.Name, out expectedResult);
            //Assert.That(CustomerName, Is.EqualTo(expectedResult));
        }

        public Dictionary<string, string> GetCustomerList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("201700087 1007.pdf.txt", "Peanut Van der Schoot");
            dic.Add("201700088 1008.pdf.txt", "Mariel Bitter");
            dic.Add("201700089 1009.pdf.txt", "Sandy Brouns");
            dic.Add("201700090 1010.pdf.txt", "Lisette Kastner");
            dic.Add("201700091 1030.pdf.txt", "D Draadjer");
            dic.Add("201700092 1031.pdf.txt", "Marieke Schut");
            dic.Add("201700096 1040.pdf.txt", "Mevr");
            dic.Add("201700097 1045.pdf.txt", "Laura Bouwman");
            
            return dic;
        }


    }

    public class PathProvider
    {
        public static string AssemblyPath => GetAssemblyPath();

        public static string GetAssemblyPath()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!path.Contains("Mac"))
            {
                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", ""));
            }

            return path;
        }
    }
}
