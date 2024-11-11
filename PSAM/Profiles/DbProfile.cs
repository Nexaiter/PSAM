using AutoMapper;
using PSAM.DTOs.AccountDTOs;
using PSAM.Entities;

namespace PSAM.Profiles
{
    public class DbProfile : Profile
    {
        public DbProfile() {
            // Creating maps of entity with DTO
            CreateMap<AccountEntity, AccountDTO>();
            CreateMap<TechnologyEntity, TechnologyDTOs>();
            CreateMap<PostEntity, PostDTO>();
            CreateMap<CommentEntity, CommentDTO>();
            CreateMap<PostLikeEntity, PostLikeDTO>();
            CreateMap<CommentLikeEntity, CommentLikeDTO>();
        }
    }
}
