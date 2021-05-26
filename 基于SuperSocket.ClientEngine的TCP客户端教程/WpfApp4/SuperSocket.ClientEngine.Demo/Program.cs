using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.ClientEngine.Demo
{

    class MyFilter : IReceiveFilter<StringPackageInfo>
    {
        public IReceiveFilter<StringPackageInfo> NextReceiveFilter
        {
            get
            {
                return null;
            }
        }

        public FilterState State
        {
            get
            {
                return FilterState.Normal;
            }
        }

        public StringPackageInfo Filter(BufferList data, out int rest)
        {
            rest = 0;
            return new StringPackageInfo("","",new string[] { });
        }

        public void Reset()
        {
            ;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {



            
        }
    }
}
