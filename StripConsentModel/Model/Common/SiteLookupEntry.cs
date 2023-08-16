using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace StripConsentModel.Model.Common
{
    public class SiteLookupEntry : LookupEntry
    {
        public SiteLookupEntry(string ibdAuditCode, string name, AdultPaed adultPaeds) : base(ibdAuditCode, name)
        {
            AdultPaeds = adultPaeds;
        }
        public override AdultPaed AdultPaeds { get; }
    }
    class TrustLookupEntry : LookupEntry
    {
        public TrustLookupEntry(string ibdAuditCode, string name, List<SiteLookupEntry> sites) : base(ibdAuditCode, name)
        {
            Sites = sites;
        }
        public List<SiteLookupEntry> Sites { get; }

        public override AdultPaed AdultPaeds
        {
            get
            {
                var groupings = Sites.GroupBy(x => x.AdultPaeds);
                if (groupings.Count() == 1)
                {
                    return groupings.First().Key;
                } else
                {
                    return AdultPaed.Mixed;
                }
            }
        }
    }

    public class ServiceLookupEntry : LookupEntry
    {
        public ServiceLookupEntry(string ibdAuditCode, string name, List<SiteLookupEntry> sites) : base(ibdAuditCode, name)
        {
            Sites = sites;
        }
        public List<SiteLookupEntry> Sites { get; }

        public override AdultPaed AdultPaeds
        {
            get
            {
                var groupings = Sites.GroupBy(x => x.AdultPaeds);
                if (groupings.Count() == 1)
                {
                    return groupings.First().Key;
                }
                else
                {
                    return AdultPaed.Mixed;
                }
            }
        }
    }

    public abstract class LookupEntry : ILookupEntry
    {
        public LookupEntry(string ibdAuditCode, string name)
        {
            IBDAuditCode = ibdAuditCode;
            Name = name;
        }

        public string IBDAuditCode { get; private set; }

        public string Name { get; private set; }

        public abstract AdultPaed AdultPaeds { get; }

        public override string ToString()
        {
            return Name;
        }
    }

    public interface ILookupEntry
    {
        string IBDAuditCode { get; }
        AdultPaed AdultPaeds { get; }
        string Name { get; }

    }
}

public enum AuditType
{
    Trust,
    Service,
    Site,
    Subsite
}

public enum AdultPaed
{
    Adult,
    Paediatric,
    Mixed
}

public enum Country
{
    England,
    Wales,
    Scotland,
    NorthernIreland
}