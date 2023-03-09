using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Dal.Dto.Response
{
    public class ProfileByHandleDataResponse
    {
        public ProfileByHandleData data { get; set; }
    }
    public class ProfileByHandleData
    {
        public ProfileByHandle profileByHandle { get; set; }
    }
    public class ProfileByHandle
    {
        public MetadataInfo metadataInfo { get; set; }
        public Owner owner { get; set; }
        public bool isPrimary { get; set; }
    }

    public class MetadataInfo
    {
        public string avatar { get; set; }
        public string bio { get; set; }
    }

    public class Owner
    {
        public string address { get; set; }
    }
}
