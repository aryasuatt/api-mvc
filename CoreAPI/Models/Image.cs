namespace CoreAPI.Models
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }   // Binary data for the image
        public string FileName { get; set; }  // File name of the image
        public string ContentType { get; set; } // MIME type of the image (e.g., image/jpeg)

    }
}
