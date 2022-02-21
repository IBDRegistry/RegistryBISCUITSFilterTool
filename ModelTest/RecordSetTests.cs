using Microsoft.VisualStudio.TestTools.UnitTesting;
using StripV3Consent.Model;

namespace ModelTest
{
	[TestClass]
	public class RecordSetTests
	{
		[TestMethod]
		public void TestMethod1()
		{
			string PatientLine = "366 857 8761, 24/12/2000";

			ImportFile PatientFile = new("patient_abc.csv", PatientLine);
			//Record PatientRecord = new Record(new string[] { PatientFile.Sp, new ImportFile() });
			Record ConsentRecord;
			Record OptOutReocrd;
		}
	}
}
