﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Entities;

public partial class NewsArticle : IEntity<string>
{
    public string NewsArticleId { get; set; } = null!;
    string IEntity<string>.Id => NewsArticleId;

    public string? NewsTitle { get; set; }

    public string Headline { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string? NewsContent { get; set; }

    public string? NewsSource { get; set; }

    public short? CategoryId { get; set; }

    public bool? NewsStatus { get; set; }

    public short? CreatedById { get; set; }

    public short? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    public virtual Category? Category { get; set; }
    public virtual SystemAccount? CreatedBy { get; set; }
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
