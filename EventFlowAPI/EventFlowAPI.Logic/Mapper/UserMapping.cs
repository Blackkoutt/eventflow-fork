using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.Logic.Identity.DTO.RequestDto;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class UserMapping
{
    public static User ToEntity(this UserRegisterRequestDto source) => source is null ? null! : source.ApplyTo(new User());

    public static User ApplyTo(this UserRegisterRequestDto source, User target)
    {
        target.Email = source.Email;
        target.Name = source.Name;
        target.Surname = source.Surname;
        target.DateOfBirth = source.DateOfBirth;
        return target;
    }

    public static User ApplyTo(this UserDataRequestDto source, User target)
    {
        target.Street = source.Street;
        target.HouseNumber = source.HouseNumber;
        target.FlatNumber = source.FlatNumber;
        target.City = source.City;
        target.ZipCode = source.ZipCode;
        target.PhoneNumber = source.PhoneNumber;
        return target;
    }

    public static User ApplyTo(this UpdateUserDataRequestDto source, User target)
    {
        target.Street = source.Street;
        target.HouseNumber = source.HouseNumber;
        target.FlatNumber = source.FlatNumber;
        target.City = source.City;
        target.ZipCode = source.ZipCode;
        target.PhoneNumber = source.PhoneNumber;
        return target;
    }

    public static UserDataResponseDto? ToUserDataResponse(this User source)
    {
        if (!source.HasUserData) return null;
        return new UserDataResponseDto
        {
            Id = int.TryParse(source.Id, out var id) ? id : 0,
            Street = source.Street ?? string.Empty,
            HouseNumber = source.HouseNumber ?? 0,
            FlatNumber = source.FlatNumber,
            City = source.City ?? string.Empty,
            ZipCode = source.ZipCode ?? string.Empty,
            PhoneNumber = source.PhoneNumber ?? string.Empty
        };
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static UserResponseDto? ToResponse(this User? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static UserResponseDto? ToResponse(this User? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<UserResponseDto>(source, out var existing)) return existing;
        var target = new UserResponseDto();
        context.Add(source, target);
        target.Id = source.Id;
        target.Name = source.Name;
        target.Surname = source.Surname;
        target.DateOfBirth = source.DateOfBirth;
        target.RegisteredDate = source.RegisteredDate;
        target.PhotoName = source.PhotoName;
        target.UserData = source.ToUserDataResponse();
        return target;
    }
}
