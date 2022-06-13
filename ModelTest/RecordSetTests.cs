using Microsoft.VisualStudio.TestTools.UnitTesting;
using StripV3Consent.Model;
using System.Linq;

namespace ModelTest
{
	[TestClass]
	public class RecordSetTests
	{
		/// <summary>
		/// Tests whether a consent record with a newer date would override one with an older date
		/// </summary>
		[TestMethod]
		public void TestConsentDateOverride()
		{
			const string NHSNumber = "426 221 1894";
			const string DOB = "14/07/1926";
			string PatientLine = $"{NHSNumber}, {DOB}";
			string OldConsentLine = $"{NHSNumber}, {DOB},10/05/2003,N,,,,,,,V4";
			string NewConsentLine = $"{NHSNumber}, {DOB},28/02/2022,Y,,,,,,,V4";

			ImportFile PatientFile = new("patient_Trust.csv", PatientLine);
			ImportFile OldConsentFile = new("consent_Trust.csv", OldConsentLine);
			ImportFile NewConsentFile = new("consent_Registry.csv", NewConsentLine);

			RecordSet[] Patients = ConsentToolModel.SplitInputFilesIntoRecordSets(
				Files: new ImportFile[] { PatientFile, OldConsentFile, NewConsentFile },
				EnableNationalOptOut: false
				).ToArray();

			RecordSet TestingRecord = Patients.Where(rs => rs.GetFieldValue(DataItemCodes.NHSNumber) == NHSNumber).First();

			Assert.IsTrue(TestingRecord.IsConsentValid);
		}

		/// <summary>
		/// In the event of two records having matching DateTime values the one with Consent = N should be accepted, if it exists
		/// </summary>
		[TestMethod]
		public void TestMatchingDateTimesFallback()
		{
			const string NHSNumber = "426 221 1894";
			const string DOB = "14/07/1926";
			string PatientLine = $"{NHSNumber}, {DOB}";
			string OldConsentLine = $"{NHSNumber}, {DOB},10/05/2003,N,,,,,,,V4";
			string NewConsentLine = $"{NHSNumber}, {DOB},10/05/2003,Y,,,,,,,V4";

			ImportFile PatientFile = new("patient_Trust.csv", PatientLine);
			ImportFile OldConsentFile = new("consent_Trust.csv", OldConsentLine);
			ImportFile NewConsentFile = new("consent_Registry.csv", NewConsentLine);

			RecordSet[] Patients = ConsentToolModel.SplitInputFilesIntoRecordSets(
				Files: new ImportFile[] { PatientFile, NewConsentFile, OldConsentFile },
				EnableNationalOptOut: false
				).ToArray();

			RecordSet TestingRecord = Patients.Where(rs => rs.GetFieldValue(DataItemCodes.NHSNumber) == NHSNumber).First();

			Assert.IsFalse(TestingRecord.IsConsentValid);
		}
	}
}
