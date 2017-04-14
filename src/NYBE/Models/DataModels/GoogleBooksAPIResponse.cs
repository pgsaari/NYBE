using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NYBE.Models.DataModels
{
    public class GoogleBooksAPIResponse
    {
        [JsonIgnore]
        public Object kind { get; set; }
        public int totalItems { get; set; }
        public List<GoogleBooksAPIItem> items { get; set; }

        public List<GoogleBook> getBooks()
        {
            List<GoogleBook> books = new List<GoogleBook>();
            foreach(GoogleBooksAPIItem item in items)
            {
                GoogleBook book = new GoogleBook();
                GoogleBooksAPIVolumeInfo info = item.volumeInfo;
                book.title = info.title;
                book.description = info.description;
                foreach(GoogleBooksAPIIndustryIdentifier ii in info.industryIdentifiers)
                {
                    if(ii.type == "ISBN_13" || book.isbn == null)
                    {
                        book.isbn = ii.identifier;
                    }
                }
                book.image = info.imageLinks.thumbnail;
                string author = info.authors[0];
                book.authorFName = author.Substring(0, author.IndexOf(' '));
                book.authorLName = author.Substring(author.IndexOf(' ') + 1);
                //book.authorLName = author;
                books.Add(book);
            }
            return books;
        }
    }

    public class GoogleBooksAPIItem
    {
        [JsonIgnore]
        public Object kind { get; set; }

        [JsonIgnore]
        public Object id { get; set; }

        [JsonIgnore]
        public Object etag { get; set; }

        [JsonIgnore]
        public Object selfLink { get; set; }

        [JsonIgnore]
        public Object saleInfo { get; set; }

        [JsonIgnore]
        public Object accessInfo { get; set; }

        [JsonIgnore]
        public Object searchInfo { get; set; }
        public GoogleBooksAPIVolumeInfo volumeInfo { get; set; }
    }

    public class GoogleBooksAPIVolumeInfo
    {
        [JsonIgnore]
        public Object publishDate { get; set; }
        [JsonIgnore]
        public Object readingModes { get; set; }
        [JsonIgnore]
        public Object pageCount { get; set; }
        [JsonIgnore]
        public Object printType { get; set; }
        [JsonIgnore]
        public Object categories { get; set; }
        [JsonIgnore]
        public Object averageRating { get; set; }
        [JsonIgnore]
        public Object ratingsCount { get; set; }
        [JsonIgnore]
        public Object maturityRating { get; set; }
        [JsonIgnore]
        public Object allowAnonLogging { get; set; }
        [JsonIgnore]
        public Object contentVersion { get; set; }
        [JsonIgnore]
        public Object language { get; set; }
        [JsonIgnore]
        public Object previewLink { get; set; }
        [JsonIgnore]
        public Object infoLink { get; set; }
        [JsonIgnore]
        public Object canonicalVolumeLink { get; set; }
        public string title { get; set; }
        public List<String> authors { get; set; }
        public string description { get; set; }
        public GoogleBooksAPIImageLink imageLinks { get; set; }
        public List<GoogleBooksAPIIndustryIdentifier> industryIdentifiers { get; set; }
    }

    public class GoogleBooksAPIIndustryIdentifier
    {
        public string type { get; set; }
        public string identifier { get; set; }
    }

    public class GoogleBooksAPIImageLink
    {
        public string smallThumbnail { get; set; }
        public string thumbnail { get; set; }
        public string small { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string extraLarge { get; set; }
    }
}
