using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Specification;
using System.Xml.Serialization;
using System.IO;

namespace StripV3Consent
{
    internal static class Spec2021K
    {
        private static DataSubmissionSpecification cached2021k;
        public static DataSubmissionSpecification Specification
        {
            get
            {
                if (cached2021k is null)
                {
                    XmlSerializer Deserialiser = new System.Xml.Serialization.XmlSerializer(typeof(DataSubmissionSpecification));
                    using (TextReader reader = new StringReader(Properties.Resources._2021_K_IBD_Registry_Submission_Dataset_V01__Nov_2021_))
                    {
                        cached2021k = (DataSubmissionSpecification)Deserialiser.Deserialize(reader);
                    }

                }

                return cached2021k;
            }
        }
    }
}
