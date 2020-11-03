using System;

namespace Newspaper.GateWay.ViewModels.ViewModels.Base
{
    public abstract class ViewModelBase : IViewModel
    {
        public abstract Guid Id { get; set; }
    }
}