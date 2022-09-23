using Microsoft.VisualStudio.TestTools.UnitTesting;
using StripV3Consent.Model;

namespace ModelTest
{

    [TestClass]
    public class RecordTests
    {

        [TestMethod]
        public void TestOutOfBoundsRecord()
        {
            string[] RecordContentsWithoutConsent = new string[] { "1234567890", "01/01/2000", "01/01/2003" };

            DataFile OriginalFile = new ImportFile("consent_abc.csv", "");

            Record TestRecord = new Record(RecordContentsWithoutConsent, OriginalFile);

            string ConsentValue = TestRecord.GetValueByDataItemCode(DataItemCodes.ConsentForRegistryFieldID);

            Assert.IsTrue(ConsentValue == "");
        }
    }
}