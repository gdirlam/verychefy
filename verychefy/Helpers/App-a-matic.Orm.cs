using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_a_matic.Orm {

    public struct DataOrmContext {
        public string Create { get; set; }
        public string Read { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
    }

    public interface IOrmModel {
        DataOrmContext OrmContext { get; set; }
    }



}