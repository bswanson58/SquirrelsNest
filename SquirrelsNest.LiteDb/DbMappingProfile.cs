using AutoMapper;
using LiteDB;
using SquirrelsNest.Common.Entities;
using SquirrelsNest.LiteDb.Dto;

namespace SquirrelsNest.LiteDb {
    internal class DbMappingProfile : Profile {
        public DbMappingProfile() {
            CreateMap<DbProject, SnProject>()
                .ForMember( snProject => snProject.EntityId, opt => opt.MapFrom( src => src.Id.ToString()));
            CreateMap<SnProject, DbProject>()
                .ForMember( dbProject => dbProject.Id, opt => opt.MapFrom( src => new ObjectId( src.EntityId )));
        }
    }
}
