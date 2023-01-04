using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoRDA.DataArchives
{
    public class DataArchiveBuilder
    {
        private DataArchiveBuilder() { }

        public static DataArchiveBuilder New()
        { 
            return new DataArchiveBuilder();
        }

        public DataArchive Build()
        {
            throw new NotImplementedException();
        }
    }
}
