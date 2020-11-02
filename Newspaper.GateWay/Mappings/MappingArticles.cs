using NewsPaper.GateWay.Mappings.Base;
using Newspaper.GateWay.ViewModels.ViewModels.Article;
using Newspaper.GateWay.ViewModels.ViewModels.Author;
using Newspaper.GateWay.ViewModels.ViewModels.Editor;
using Newspaper.GateWay.ViewModels.ViewModels.User;
using NewsPaper.MassTransit.Contracts.DTO.Models.Articles;
using NewsPaper.MassTransit.Contracts.DTO.Models.Author;
using NewsPaper.MassTransit.Contracts.DTO.Models.Editor;
using NewsPaper.MassTransit.Contracts.DTO.Models.User;

namespace NewsPaper.GateWay.Mappings
{
    public class MappingArticlesDto : MapperConfigurationBase
    {
        public MappingArticlesDto()
        {
            CreateMap<ArticleDto, ArticleViewModel>();

            CreateMap<AuthorDto, AuthorViewModel>();

            CreateMap<EditorDto, EditorViewModel>();

            CreateMap<UserDto, UserViewModel>();
        }
    }
}
