using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Assets.Store.Core.Contracts;
using Ubik.Domain.Core;

namespace Ubik.Assets.Store.Core.Events
{
    public class FileUploadedEvent : IEvent
    {
        public FileUploadedEvent(IFileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }
        public IFileInfo FileInfo { get; private set; }
    }
}
