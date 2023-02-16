using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.ViewModel
{
    public class NewsViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Images { get; set; }
        public DateTime Created { get; set; }
        public NewsViewModel()
        {

        }
        public NewsViewModel(News news)
        {
            ID = news.ID;
            Title = news.Title;
            Content = news.Content;

        }
    }
    
}
