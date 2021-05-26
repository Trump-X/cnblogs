using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.ClientEngine.Demo
{
    class MyReceiveFilter : TerminatorReceiveFilter<MyStringPackageInfo>
    {
        public MyReceiveFilter():base(Encoding.ASCII.GetBytes("\r\n"))
        {

        }

        public override MyStringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            throw new NotImplementedException();
        }
    }
}
