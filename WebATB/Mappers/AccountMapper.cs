using AutoMapper;
using WebATB.Data.Entities;
using WebATB.Data.Entities.Identity;
using WebATB.Models.Account;
using WebATB.Models.Category;
using WebATB.Models.Helpers;

namespace WebATB.Mappers;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<RegisterViewModel, UserEntity>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.Image, opt => opt.Ignore());

        CreateMap<UserEntity, UserLinkViewModel>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => $"{x.LastName} {x.FirstName}"))
            .ForMember(x => x.Image, opt => opt.MapFrom(x =>
                string.IsNullOrEmpty(x.Image)
                    ? "no-image-icon.png"
                    : $"/avatars/50_{x.Image}"
            ));
    }
}
