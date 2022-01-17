using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Specification
{
    public class Field
    {
        /// <summary>
        /// Human readable name of the field in 2021K
        /// </summary>
        public string Name;

        /// <summary>
        /// Machine code for items
        /// </summary>
        public string DataItemCode;

        /// <summary>
        /// Version of 2021K this was introduced in
        /// </summary>
        public string Version;

        /// <summary>
        /// In format FILE: Group
        /// </summary>
        public string Group;

        public override string ToString()
        {
            return Name;
        }
    }

    public class FieldEqualityComparer : IEqualityComparer<Field>
    {
        public bool Equals(Field a, Field b)
        {
            return a.DataItemCode == b.DataItemCode;
        }

        public int GetHashCode(Field f)
        {
            return f.DataItemCode.GetHashCode();
        }
    }
}
