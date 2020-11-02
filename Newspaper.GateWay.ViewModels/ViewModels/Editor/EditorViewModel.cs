using System;
using Newspaper.GateWay.ViewModels.ViewModels.Base;

namespace Newspaper.GateWay.ViewModels.ViewModels.Editor
{
    public class EditorViewModel : ViewModelBase
    {
        public Guid IdentityGuid { get; set; }

        public string NikeName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int CountСompletedArticles { get; set; }

        public int CountUnderRevisionArticles { get; set; }
    }
}