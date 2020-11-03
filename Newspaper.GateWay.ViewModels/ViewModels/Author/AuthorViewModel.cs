using System;
using Newspaper.GateWay.ViewModels.ViewModels.Base;

namespace Newspaper.GateWay.ViewModels.ViewModels.Author
{
    public class AuthorViewModel : ViewModelBase
    {
        public override Guid Id { get; set; }

        public Guid IdentityGuid { get; set; }

        public string NikeName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}