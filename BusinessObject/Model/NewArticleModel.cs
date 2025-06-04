using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class NewArticleModel
    {
        public NewsArticle NewsArticle { get; set; }
        public List<int> TagIds { get; set; }
    }
}
