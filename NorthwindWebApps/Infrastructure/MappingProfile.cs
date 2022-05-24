namespace NorthwindWebApps.Infrastructure
{
    using AutoMapper;
    using Northwind.Services.Blogging;
    using Northwind.Services.Blogging.Models;
    using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

#pragma warning disable SA1600

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<BlogArticleEntity, BlogArticle>().ReverseMap();
            this.CreateMap<BlogArticleProductEntity, BlogArticleProduct>().ReverseMap();
            this.CreateMap<BlogArticleCommentEntity, BlogArticleComment>().ReverseMap();
        }
    }
}
