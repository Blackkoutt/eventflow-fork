using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using Microsoft.VisualBasic;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class HallTypeService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IFileService fileService) :
        GenericService<
            HallType,
            HallTypeRequestDto,
            UpdateHallTypeRequestDto,
            HallTypeResponseDto,
            HallTypeQuery
        >(unitOfWork, authService),
        IHallTypeService
    {
        private readonly IFileService _fileService = fileService;
        public sealed override async Task<Result<IEnumerable<HallTypeResponseDto>>> GetAllAsync(HallTypeQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(ht => !ht.IsDeleted)
                                                              .ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<HallTypeResponseDto>>.Success(response);
        }
        

        public sealed override async Task<Result<HallTypeResponseDto>> AddAsync(HallTypeRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<HallTypeResponseDto>.Failure(validationError);

            var equipments = await _unitOfWork.GetRepository<Equipment>().GetAllAsync(q =>
                            q.Where(e => requestDto!.EquipmentIds.Contains(e.Id)));

            var hallType = new HallType
            {
                Name = requestDto!.Name,
                Description = requestDto!.Description,
                Equipments = equipments.ToList()
            };

            hallType.HallTypeGuid = Guid.NewGuid();
            var photoPostError = await _fileService.PostPhoto(hallType, requestDto!.HallTypePhoto, $"{hallType.Name}_{hallType.HallTypeGuid}", isUpdate: false);
            if (photoPostError != Error.None)
                return Result<HallTypeResponseDto>.Failure(photoPostError);

            await _repository.AddAsync(hallType);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(hallType);

            return Result<HallTypeResponseDto>.Success(response);
        }

        public sealed override async Task<Result<HallTypeResponseDto>> UpdateAsync(int id, UpdateHallTypeRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<HallTypeResponseDto>.Failure(validationError);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<HallTypeResponseDto>.Failure(Error.NotFound);

            var newEntity = oldEntity;

            newEntity.Name = requestDto!.Name;
            newEntity.Description = requestDto!.Description;
            var equipments = await _unitOfWork.GetRepository<Equipment>().GetAllAsync(q =>
                                q.Where(e => requestDto!.EquipmentIds.Contains(e.Id)));
            newEntity.Equipments = equipments.ToList();

            var photoPostError = await _fileService.PostPhoto(newEntity, requestDto!.HallTypePhoto, $"{newEntity.Name}_{newEntity.HallTypeGuid}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<HallTypeResponseDto>.Failure(photoPostError);
            _repository.Update(newEntity);

            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(newEntity);

            return Result<HallTypeResponseDto>.Success(response);
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            if (entity.Name == "Sala ogólna")
                return Result<object>.Failure(HallTypeError.CannotDeleteDefaultHallType);

            var defaultHallType = (await _repository.GetAllAsync(q => q.Where(s => s.Name == "Sala ogólna"))).FirstOrDefault();
            if (defaultHallType == null) return Result<object>.Failure(HallTypeError.CannotFoundDefaultHallType);

            var hallsWithoutRents = entity.Halls.Where(h => !h.Rents.Any()).ToList();
            foreach (var hall in hallsWithoutRents)
            {
                hall.Type = defaultHallType;
                _unitOfWork.GetRepository<Hall>().Update(hall);
            }

            await _fileService.DeletePhoto(entity);
            if (entity is ISoftDeleteable softDeleteableEntity && entity.Halls.Any(h => h.Rents.Any()))
            {
                softDeleteableEntity.IsDeleted = true;
                softDeleteableEntity.DeleteDate = DateTime.Now;
                entity.Halls = entity.Halls.Where(h => h.Rents.Any()).ToList();
                _repository.Update(entity);
            }
            else
            {
                _repository.Delete(entity);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result<object>.Success();
        }


        protected sealed override async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return userResult.Error;

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return AuthError.UserDoesNotHavePremissionToResource;

            if (id != null && id < 0)
                return Error.RouteParamOutOfRange;

            if (requestDto == null)
                return Error.NullParameter;

            List<int> equipmentIds = [];
            switch (requestDto)
            {
                case HallTypeRequestDto hallTypeRequestDto:
                    equipmentIds = hallTypeRequestDto.EquipmentIds;
                    break;
                case UpdateHallTypeRequestDto updateHallTypeRequestDto:
                    equipmentIds = updateHallTypeRequestDto.EquipmentIds;
                    break;
                default:
                    return Error.BadRequestType;
            }

            var isSameEntityExistsResult = await IsSameEntityExistInDatabase(requestDto, id);
            if (!isSameEntityExistsResult.IsSuccessful) return isSameEntityExistsResult.Error;

            var isSameEntityExistInDb = isSameEntityExistsResult.Value;
            if (isSameEntityExistInDb)
                return Error.SuchEntityExistInDb;

            if (equipmentIds.Count() != equipmentIds.Distinct().Count())
                return TicketTypeError.TicketDuplicates;
            var equipments = await _unitOfWork.GetRepository<Equipment>().GetAllAsync();
            var equipmentIdsInDb = equipments.Select(e => e.Id).ToList();
            if (!equipmentIds.All(id => equipmentIdsInDb.Contains(id)))
                return TicketTypeError.NotFound;

            if (id != null)
            {
                var entity = await _repository.GetOneAsync((int)id);
                if (entity == null)
                    return Error.NotFound;
            }

            return Error.None;
        }

        protected sealed override async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower() &&
                        !entity.IsDeleted
                      ))).Any();

            return Result<bool>.Success(result);
        }

        public new IEnumerable<HallTypeResponseDto> MapAsDto(IEnumerable<HallType> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.AsDto<HallTypeResponseDto>();
                responseDto.Equipments = entity.Equipments.Select(eq => eq.AsDto<EquipmentResponseDto>()).ToList();
                responseDto.PhotoEndpoint = $"/HallTypes/{responseDto.Id}/image";
                return responseDto;
            });
        }

        public new HallTypeResponseDto MapAsDto(HallType entity)
        {
            var responseDto = entity.AsDto<HallTypeResponseDto>();
            responseDto.Equipments = entity.Equipments.Select(eq => eq.AsDto<EquipmentResponseDto>()).ToList();
            responseDto.PhotoEndpoint = $"/HallTypes/{responseDto.Id}/image";
            return responseDto;
        }
    }
}
