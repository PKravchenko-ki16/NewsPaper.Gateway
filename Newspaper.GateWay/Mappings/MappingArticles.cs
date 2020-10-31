using Newspaper.GateWay.ViewModels.ViewModels;
using NewsPaper.Gateway.Mappings.Base;
using NewsPaper.MassTransit.Contracts.DTO.Models.Articles;

namespace NewsPaper.Gateway.Mappings
{
    public class MappingArticlesDto : MapperConfigurationBase
    {
        public MappingArticlesDto()
        {
            CreateMap<ArticlesDto, ArticleViewModelApi>();
        }
    }
}
