using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BPPS.Models
{
    public class Export
    {
        public string question { get; set; }
        public int answer { get; set; }
        public string comment { get; set; }

        //public static List<Export> GetData(){
        //    return new List<Export>()
        //    {
        //        new Export() { question = "abc", answer = 3, comment = "This is comment 1."},
        //        new Export() { question = "def", answer = 5, comment = "This is comment 2."},
        //        new Export() { question = "ghi", answer = 1, comment = "This is comment 3."}
        //    };
        //}
    }
}