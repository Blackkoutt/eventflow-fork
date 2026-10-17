using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.Logic.Identity.DTO.RequestDto;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.ResponseDto;

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
        target.UserData = source.UserData.ToResponse(context);
        return target;
    }
}
