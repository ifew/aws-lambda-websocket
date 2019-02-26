using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnDisconnect
{
    
    [Table("connection_socket")]
    public class ConnectionSocketModel
    {
        public int id { get; set; }
        public string connection_id { get; set; }
        public string channel { get; set; }
        public string user_id { get; set; }
        public DateTime add_datetime { get; set; }
    }

    public class ConnectionSocketInputModel
    {
        public string channel { get; set; }
        public string user_id { get; set; }
    }
}