using System;

namespace Trojantrading.Models
{
    public class PdfBoard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
    }

    public class TestViewModel
    {
        public string DocTitle { get; set; }
        public string DocContent { get; set; }
    }
}