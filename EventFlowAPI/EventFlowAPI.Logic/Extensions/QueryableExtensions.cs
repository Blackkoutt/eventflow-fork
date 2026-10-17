using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Query.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EventFlowAPI.Logic.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> query, int? pageNumber, int? pageSize)
        {
            if(pageNumber != null && pageSize != null)
            {
                var skipNumber = (int)((pageNumber - 1) * pageSize);
                return query.Skip(skipNumber).Take((int)pageSize);
            }
            return query;
        }
        public static IQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> query, string? sortBy, SortDirection sortDirection) where TEntity : class
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                PropertyInfo? property = typeof(TEntity)
                                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .FirstOrDefault(p => p.Name.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
                if (property == null) return query;

                var parameter = Expression.Parameter(typeof(TEntity), "x");

                // Create the property access expression (x.PropertyName)
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);

                // Create the sorting lambda expression (x => x.PropertyName)
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                // Get method name
                string methodName = (sortDirection == SortDirection.DESC) ? "OrderByDescending" : "OrderBy";

                // Use reflection to get the correct method
                MethodInfo orderByMethod = typeof(Queryable).GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .Single()
                    .MakeGenericMethod(typeof(TEntity), property.PropertyType);

                var invokedMethodResult = orderByMethod.Invoke(
                            obj: null,
                            parameters: new object[] { query, orderByExpression });

                if (invokedMethodResult == null)
                    throw new Exception();

                return (IQueryable<TEntity>)invokedMethodResult;

            }
            return query;
        }
        public static IQueryable<TEntity> ByName<TEntity>(this IQueryable<TEntity> queryable, INameableQuery query)
        {
            if (!string.IsNullOrEmpty(query.Name)) queryable = queryable.Where(e => ((INameableEntity)e!).Name.ToLower().Contains(query.Name.ToLower()));
            return queryable;
        }

        public static IQueryable<TEntity> ByDate<TEntity>(this IQueryable<TEntity> queryable, IDateableQuery query)
        {
            if (query.FromDate != null)
            {
                queryable = queryable.Where(e => ((IDateableEntity)e!).StartDate >= query.FromDate ||
                                                  ((IDateableEntity)e!).EndDate > query.FromDate);
            }

            if (query.ToDate != null)
            {
                queryable = queryable.Where(e => ((IDateableEntity)e!).StartDate <= query.ToDate);
            }

            //if (query.FromDate != null) queryable = queryable.Where(e => ((IDateableEntity)e!).StartDate >= query.FromDate);
            //if (query.ToDate != null) queryable = queryable.Where(e => ((IDateableEntity)e!).StartDate <= query.ToDate);
            return queryable;
        }

        public static IQueryable<TEntity> ByPrice<TEntity>(this IQueryable<TEntity> queryable, IPriceableQuery query)
        {
            if (query.MinPrice != null) queryable = queryable.Where(e => ((IPriceableEntity)e!).Price >= query.MinPrice);
            if (query.MaxPrice != null) queryable = queryable.Where(e => ((IPriceableEntity)e!).Price <= query.MaxPrice);
            return queryable;
        }

        public static IQueryable<Event> ByQuery(this IQueryable<Event> queryable, EventQuery query)
        {
            queryable = queryable.ByStatus(query.Status);
            queryable = queryable.ByName(query);
            queryable = queryable.ByDate(query);
            if (query.MinDuration != null) queryable = queryable.Where(e => EF.Functions.DateDiffSecond(e.StartDate, e.EndDate) >= query.MinDuration.Value.TotalSeconds);
            if (query.MaxDuration != null) queryable = queryable.Where(e => EF.Functions.DateDiffSecond(e.StartDate, e.EndDate) <= query.MaxDuration.Value.TotalSeconds);
            if (query.CategoryId != null) queryable = queryable.Where(e => e.CategoryId == query.CategoryId);
            if (query.HallNr != null) queryable = queryable.Where(e => e.Hall.HallNr == query.HallNr);
            if (query.TicketTypeId != null) queryable = queryable.Where(e => e.Tickets.Any(t => t.TicketTypeId == query.TicketTypeId));
            if (query.IsFestivalEvent == false) queryable = queryable.Where(e => !e.Festivals.Any());
            if (query.IsFestivalEvent == true) queryable = queryable.Where(e => e.Festivals.Any());
            if (query.MinTicketPrice != null) queryable = queryable.Where(e => e.Tickets.Any(t => t.Price >= query.MinTicketPrice));
            if (query.MaxTicketPrice != null) queryable = queryable.Where(e => e.Tickets.Any(t => t.Price <= query.MaxTicketPrice));
            queryable = queryable.SortBy(query.SortBy, query.SortDirection);
            return queryable;
        }
        public static IQueryable<Festival> ByQuery(this IQueryable<Festival> queryable, FestivalQuery query)
        {
            queryable = queryable.ByStatus(query.Status);
            queryable = queryable.ByName(query);
            queryable = queryable.ByDate(query);
            if (query.MinDuration != null) queryable = queryable.Where(x => EF.Functions.DateDiffSecond(x.StartDate, x.EndDate) >= query.MinDuration.Value.TotalSeconds);
            if (query.MaxDuration != null) queryable = queryable.Where(x => EF.Functions.DateDiffSecond(x.StartDate, x.EndDate) <= query.MaxDuration.Value.TotalSeconds);
            if (query.MinEventsCount != null) queryable = queryable.Where(x => x.Events.Count >= query.MinEventsCount);
            if (query.MaxEventsCount != null) queryable = queryable.Where(x => x.Events.Count <= query.MaxEventsCount);    
            if (query.MediaPatronId != null) queryable = queryable.Where(x => x.Partners.Any(p => p is MediaPatron && p.Id == query.MediaPatronId));
            if (query.OrganizerId != null) queryable = queryable.Where(x => x.Partners.Any(p => p is Organizer && p.Id == query.OrganizerId));
            if (query.SponsorId != null) queryable = queryable.Where(x => x.Partners.Any(p => p is Sponsor && p.Id == query.SponsorId));
            if (query.TicketTypeId != null) queryable = queryable.Where(e => e.Tickets.Any(t => t.Id == query.TicketTypeId));
            if (query.MinTicketPrice != null) queryable = queryable.Where(e => e.Tickets.Any(t => t.Price >= query.MinTicketPrice));
            if (query.MaxTicketPrice != null) queryable = queryable.Where(e => e.Tickets.Any(t => t.Price <= query.MaxTicketPrice));
            queryable = queryable.SortBy(query.SortBy, query.SortDirection);
            return queryable;
        }

        public static IQueryable<EventPass> ByQuery(this IQueryable<EventPass> queryable, EventPassQuery query)
        {
            queryable = queryable.ByStatus(query.Status);
            queryable = queryable.ByDate(query);
            if (query.IsRenewed == true) queryable = queryable.Where(x => x.RenewalDate != null);
            if (query.IsRenewed == false) queryable = queryable.Where(x => x.RenewalDate == null);
            if (query.MinPrice != null) queryable = queryable.Where(x => x.PaymentAmount >= query.MinPrice);
            if (query.MaxPrice != null) queryable = queryable.Where(x => x.PaymentAmount <= query.MaxPrice);
            if (query.PaymentTypeId != null) queryable = queryable.Where(x => (int)x.PaymentType == query.PaymentTypeId);
            if (query.PassTypeId != null) queryable = queryable.Where(x => x.PassTypeId == query.PassTypeId);
            if (!string.IsNullOrEmpty(query.UserName)) queryable = queryable.Where(x => $"{x.User.Name} {x.User.Surname}".Contains(query.UserName));
            queryable = queryable.SortBy(query.SortBy, query.SortDirection);
            return queryable;
        }

        public static IQueryable<HallRent> ByQuery(this IQueryable<HallRent> queryable, HallRentQuery query)
        {
            queryable = queryable.ByStatus(query.Status);
            queryable = queryable.ByDate(query);
            if (query.FromRentDate != null) queryable = queryable.Where(x => x.RentDate >= query.FromRentDate);
            if (query.ToRentDate != null) queryable = queryable.Where(x => x.RentDate <= query.ToRentDate);
            if (query.MinDuration != null) queryable = queryable.Where(x => EF.Functions.DateDiffSecond(x.StartDate, x.EndDate) >= query.MinDuration.Value.TotalSeconds);
            if (query.MaxDuration != null) queryable = queryable.Where(x => EF.Functions.DateDiffSecond(x.StartDate, x.EndDate) <= query.MaxDuration.Value.TotalSeconds);
            if (query.MinPrice != null) queryable = queryable.Where(x => x.PaymentAmount >= query.MinPrice);
            if (query.MaxPrice != null) queryable = queryable.Where(x => x.PaymentAmount <= query.MaxPrice);
            if (query.PaymentTypeId != null) queryable = queryable.Where(x => (int)x.PaymentType == query.PaymentTypeId);
            if (query.HallNr != null) queryable = queryable.Where(x => x.Hall.HallNr == query.HallNr);
            if (!string.IsNullOrEmpty(query.UserName))
            {
                queryable = queryable.Where(x => (x.User.Name + " " + x.User.Surname).Contains(query.UserName));
            }

            queryable = queryable.SortBy(query.SortBy, query.SortDirection);
            return queryable;
        }

        public static IQueryable<Reservation> ByQuery(this IQueryable<Reservation> queryable, ReservationQuery query)
        {
            queryable = queryable.ByStatus(query.Status);
            queryable = queryable.ByDate(query);
            if (query.EventId != null) queryable = queryable.Where(x => x.Ticket.EventId == query.EventId);
            if (query.FromReservationDate != null) queryable = queryable.Where(x => x.ReservationDate >= query.FromReservationDate);
            if (query.ToReservationDate != null) queryable = queryable.Where(x => x.ReservationDate <= query.ToReservationDate);
            if (query.IsFestivalReservation == true) queryable = queryable.Where(x => x.Ticket.FestivalId != null);
            if (query.IsFestivalReservation == false) queryable = queryable.Where(x => x.Ticket.FestivalId == null);
            if (query.MinPrice != null) queryable = queryable.Where(x => x.PaymentAmount >= query.MinPrice);
            if (query.MaxPrice != null) queryable = queryable.Where(x => x.PaymentAmount <= query.MaxPrice);
            if (!string.IsNullOrEmpty(query.UserName)) queryable = queryable.Where(x => $"{x.User.Name} {x.User.Surname}".Contains(query.UserName));
            if (query.PaymentTypeId != null) queryable = queryable.Where(x => (int)x.PaymentType == query.PaymentTypeId);
            if (query.ReservationByEventPass == true) queryable = queryable.Where(x => x.EventPassId != null);
            if (query.ReservationByEventPass == false) queryable = queryable.Where(x => x.EventPassId == null);
            if (query.TicketTypeId != null) queryable = queryable.Where(x => x.Ticket.TicketTypeId == query.TicketTypeId);
            if (query.MinSeatsCount != null) queryable = queryable.Where(x => x.Seats.Count >= query.MinSeatsCount);
            if (query.MaxSeatsCount != null) queryable = queryable.Where(x => x.Seats.Count <= query.MaxSeatsCount);
            queryable = queryable.SortBy(query.SortBy, query.SortDirection);
            return queryable;
        }

        public static IQueryable<Hall> ByQuery(this IQueryable<Hall> queryable, HallQuery query)
        {
            if (query.HallNr != null) queryable = queryable.Where(x => x.HallNr == query.HallNr);
            if (query.RentFromDate != null && query.RentToDate != null) 
                queryable = queryable.Where(x => !x.Rents.Any(r => r.EndDate >= DateTime.Now && !r.IsDeleted && r.StartDate < query.RentToDate && r.EndDate > query.RentFromDate) &&
                                                 !x.Events.Any(e => e.EndDate >= DateTime.Now && !e.IsDeleted && e.StartDate < query.RentToDate && e.EndDate > query.RentFromDate));
            if (query.MinRentalPrice != null) queryable = queryable.Where(x => x.RentalPricePerHour >= query.MinRentalPrice);
            if (query.MaxRentalPrice != null) queryable = queryable.Where(x => x.RentalPricePerHour <= query.MaxRentalPrice);
            if (query.Floor != null) queryable = queryable.Where(x => x.Floor == query.Floor);
            if (query.HallTypeId != null) queryable = queryable.Where(x => x.HallTypeId == query.HallTypeId);
            if (query.MinSeatsCount != null) queryable = queryable.Where(x => x.HallDetails!.MaxNumberOfSeats >= query.MinSeatsCount);
            if (query.MaxSeatsCount != null) queryable = queryable.Where(x => x.HallDetails!.MaxNumberOfSeats <= query.MaxSeatsCount);
            if (query.MinHallArea != null) queryable = queryable.Where(x => Convert.ToDecimal(x.HallDetails!.TotalLength * x.HallDetails!.TotalWidth) >= query.MinHallArea);
            if (query.MaxHallArea != null) queryable = queryable.Where(x => Convert.ToDecimal(x.HallDetails!.TotalLength * x.HallDetails!.TotalWidth) <= query.MaxHallArea);
            if (query.IsHallHaveStage == false) queryable = queryable.Where(x => x.HallDetails!.StageLength == null && x.HallDetails!.StageWidth == null);
            if (query.IsHallHaveStage == true) queryable = queryable.Where(x => x.HallDetails!.StageLength != null && x.HallDetails!.StageWidth != null);
            queryable = queryable.SortBy(query.SortBy, query.SortDirection);
            return queryable;
        }

        public static IQueryable<TEntity> ByStatus<TEntity>(this IQueryable<TEntity> queryable, Status? status) where TEntity : class
        {
            return status switch
            {
                Status.Active => queryable.Where(r => (!((ISoftDeleteable)r).IsDeleted && ((IDateableEntity)r).EndDate > DateTime.Now)),
                Status.Expired => queryable.Where(r => (!((ISoftDeleteable)r).IsDeleted && !(((IDateableEntity)r).EndDate > DateTime.Now))),
                Status.Canceled => queryable.Where(r => (((ISoftDeleteable)r).IsDeleted)),
                _ => queryable
            };
        }


        public static Expression<Func<TEntity, bool>>? GetContainsExpression<TEntity>(string? value, string propertyPath) where TEntity : class
        {
            if (string.IsNullOrEmpty(propertyPath)) return null;

            var entity = Expression.Parameter(typeof(TEntity), "entity");
            Expression? propertyAccess = entity;

            foreach (var propertyName in propertyPath.Split('.'))
            {
                var property = propertyAccess.Type.GetProperty(propertyName);
                if (property == null) return null;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }

            if (propertyAccess.Type != typeof(string)) return null;

            var valueAsConstant = Expression.Constant(value);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsExpression = Expression.Call(propertyAccess, containsMethod!, valueAsConstant);

            return Expression.Lambda<Func<TEntity, bool>>(containsExpression, entity);
        }

        private static Expression<Func<TEntity, bool>>? GetEqualExpression<TEntity, TValue>(TValue? value, string? propertyName)
             where TEntity : class
             where TValue : struct
        {
            if (string.IsNullOrEmpty(propertyName)) return null;

            var property = typeof(TEntity).GetProperty(propertyName);

            if (value == null || property == null || property.PropertyType != typeof(TValue))
                return null;

            var entity = Expression.Parameter(typeof(TEntity), "entity");
            var propertyAccess = Expression.MakeMemberAccess(entity, property);

            Expression equalsExpression;

            if (typeof(TValue) == typeof(string))
            {
                var valueAsConstant = Expression.Constant(value.Value.ToString());
                equalsExpression = Expression.Call(
                    propertyAccess,
                    typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(StringComparison) })!,
                    valueAsConstant,
                    Expression.Constant(StringComparison.OrdinalIgnoreCase)
                );
            }
            else
            {
                var valueAsConstant = Expression.Constant(value.Value);
                equalsExpression = Expression.Equal(propertyAccess, valueAsConstant);
            }

            return Expression.Lambda<Func<TEntity, bool>>(equalsExpression, entity);
        }

        private static Expression<Func<TEntity, bool>>? GetMinMaxExpression<TEntity, TValue>(TValue? MinValue, TValue? MaxValue, string? propertyPath)
            where TEntity : class
            where TValue : struct
        {
            if ((MinValue == null && MaxValue == null) || string.IsNullOrEmpty(propertyPath)) return null;

            var entity = Expression.Parameter(typeof(TEntity), "entity");
            Expression? propertyAccess = entity;

            foreach (var propertyName in propertyPath.Split('.'))
            {
                var property = propertyAccess.Type.GetProperty(propertyName);
                if (property == null) return null;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            }

            Expression? rangeExpression = null;

            if (MinValue.HasValue)
            {
                var minAsConstant = Expression.Constant(MinValue.Value);
                var greaterThanOrEqual = Expression.GreaterThanOrEqual(propertyAccess, minAsConstant);
                rangeExpression = greaterThanOrEqual;
            }

            if (MaxValue.HasValue)
            {
                var maxAsConstant = Expression.Constant(MaxValue.Value);
                var lessThanOrEqual = Expression.LessThanOrEqual(propertyAccess, maxAsConstant);

                if (rangeExpression != null)
                    rangeExpression = Expression.AndAlso(rangeExpression, lessThanOrEqual);
                else
                    rangeExpression = lessThanOrEqual;
            }

            return Expression.Lambda<Func<TEntity, bool>>(rangeExpression!, entity);
        }
    }
}
