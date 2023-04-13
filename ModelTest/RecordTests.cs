using Microsoft.VisualStudio.TestTools.UnitTesting;
using StripV3Consent.Model;
using System;

namespace ModelTest
{

    [TestClass]
    public class RecordTests
    {

        [TestMethod]
        public void TestOutOfBoundsRecord()
        {
            string[] RecordContentsWithoutConsent = new string[] { "1234567890", "01/01/2000", "01/01/2003" };

            ImportFile OriginalFile = new ImportFile("consent_abc.csv", "", DateTime.Now, @"C:\SubmissionFiles\SiteOne\consent_abc.csv");

            Record TestRecord = new Record(RecordContentsWithoutConsent, OriginalFile);

            string ConsentValue = TestRecord.GetValueByDataItemCode(DataItemCodes.Consent.Registry);

            Assert.IsTrue(ConsentValue == "");
        }
    }
}