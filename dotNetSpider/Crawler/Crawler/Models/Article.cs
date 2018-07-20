namespace Crawler
{
    public class Article1
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }


        public FileType FileType { get; set; }

        public string[] AttachFiles { get; set; }

    }


    public enum FileType
    {
        pdf,
        doc,
        xls,
        txt
    }



}