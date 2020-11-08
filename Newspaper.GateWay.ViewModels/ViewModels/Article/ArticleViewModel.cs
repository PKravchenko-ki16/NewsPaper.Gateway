﻿using System;
using Newspaper.GateWay.ViewModels.ViewModels.Base;

namespace Newspaper.GateWay.ViewModels.ViewModels.Article
{
    public class ArticleViewModel : ViewModelBase
    {
        public override Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public int Rating { get; set; }

        public string Text { get; set; }

        public string NikeNameAuthor { get; set; }

        public string NikeNameEditor { get; set; }

        public Guid AuthorGuid { get; set; }

        public Guid EditorGuid { get; set; }

        public bool IsArchive { get; set; }

        public bool IsRevision { get; set; }

        public DateTime DateOfWriting { get; set; }

        public DateTime DateOfRevision { get; set; }
    }
}