using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.Entity
{
    public class BaseEntity
    {

        public long Id { get; set; }
      
        public DateTime CreateTime { get; set; }
       
        public DateTime UpdateTime { get; set; }
    }
}
