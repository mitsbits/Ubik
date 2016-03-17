using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ubik.Postal.Contracts
{
    public interface IAttachmentInfo
    {
        string FileName { get; }
        string MediaType { get; }
        string MediaTSubType { get; }
        Stream GetStream();

    }
}
