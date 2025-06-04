using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Entities;

public partial class Tag : IEntity<int>
{
    public int TagId { get; set; }
    int IEntity<int>.Id => TagId;

    public string? TagName { get; set; }

    public string? Note { get; set; }
    [JsonIgnore]
    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
