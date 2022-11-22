using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Tables;

namespace Mayhem.Dal.Mappings
{
    public class TableDtoMappings : Profile
    {
        public TableDtoMappings()
        {
            CreateMap<GameUser, GameUserDto>()
                .ForMember(x => x.Wallet, y => y.MapFrom(z => z.Wallet));
        }
    }
}
