using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Helpers.PdfOptions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfInfoOptions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EventFlowAPI.Logic.Extensions.PdfBuilderExtensions
{
    public static class Info
    {
        public static void AddOrderInfo(this IContainer column, InfoOptions options)
        {
            column
            .AlignLeft()
            .ExtendHorizontal()
            .PaddingLeft(options.PadLeft)
            .Column(column =>
            {
                column.AddTextItem(options.OrderNumber);

                column.AddTextItem(options.OrderDate);

                if (options is ReservationInfoOptions resOptions)
                {
                    column.AddTextItem(resOptions.TicketType);
                }
                else if (options is EventPassInfoOptions eventPassOptions)
                {
                    column.AddTextItem(eventPassOptions.EventPassRenew);
                    column.AddTextItem(eventPassOptions.OldEventPassType);
                }
                column.AddTextItem(options.PaymentType);

                column.AddTextItem(options.OrderingPerson);

            });
        }


        public static void AddHallViewInfo(this IContainer column, HallViewInfoOptions options)
        {
            if (options.HallRent != null || options.Event != null)
            {
                column
                .ExtendHorizontal()
                .PaddingTop(options.PadTop)
                .PaddingBottom(options.PadBottom)
                .AlignCenter()
                .Column(column =>
                {
                    column.Item()
                    .Row(row =>
                    {
                        row.Spacing(25);
                        row.AutoItem().Column(column =>
                        {
                            column.AddTextItem(options.HallNr);
                            column.AddTextItem(options.HallType);
                            column.AddTextItem(options.HallSeatsCount);
                            column.AddTextItem(options.HallDimensions);
                            column.AddTextItem(options.HallStageArea());
                        });
                        row.AutoItem().Column(column =>
                        {
                            if (options.HallRent != null)
                            {
                                column.AddTextItem(options.HallRentOrderNumber);
                                column.AddTextItem(options.HallRentOrderDate);
                                column.AddTextItem(options.HallRentOrderingPerson);
                                column.AddTextItem(options.HallRentStartDate);
                                column.AddTextItem(options.HallRentEndDate);
                            }
                            else if (options.Event != null)
                            {                   
                                column.AddTextItem(options.EventName);
                                column.AddTextItem(options.EventCategory);
                                column.AddTextItem(options.EventStartDate);
                                column.AddTextItem(options.EventEndDate);
                            }
                        });
                    });
                });
            }
            else
            {
                column
                .ExtendHorizontal()
                .PaddingTop(options.PadTop)
                .PaddingBottom(options.PadBottom)
                .AlignLeft()
                .Column(column =>
                {
                    column.AddTextItem(options.HallNr);
                    column.AddTextItem(options.HallType);
                    column.AddTextItem(options.HallSeatsCount);
                    column.AddTextItem(options.HallDimensions);
                    column.AddTextItem(options.HallStageArea());
                });
            }
        }
    }
}
