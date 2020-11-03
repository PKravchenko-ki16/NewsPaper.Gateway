using System;
using Newspaper.GateWay.ViewModels.ViewModels.Base;

namespace Newspaper.GateWay.ViewModels.ViewModels.User
{
    public class UserViewModel : ViewModelBase
    {
        public override Guid Id { get; set; }

        public Guid IdentityGuid { get; set; }

        public string NikeName { get; set; }
    }
}