using Newspaper.GateWay.ViewModels.ViewModels;
using NewsPaper.Gateway.Mappings.Base;
using NewsPaper.MassTransit.Contracts.DTO;

namespace NewsPaper.Gateway.Mappings
{
    public class MappingArticles : MapperConfigurationBase
    {
        public MappingArticles()
        {
            CreateMap<ArticlesDto, ArticleViewModelApi>();
        }
    }
}
