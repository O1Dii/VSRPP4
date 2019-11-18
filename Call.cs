using System;
using SQLite;

namespace VSRPP4
{
    class Call
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Num1 { get; set; }
        public string Num2 { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
