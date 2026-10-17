using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfCommonOptions;
using EventFlowAPI.Logic.Helpers.PdfOptions.PdfPictureOptions;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace EventFlowAPI.Logic.Extensions.PdfBuilderExtensions
{
    public static class Picture
    {
        public static void AddTicketPictures(this IContainer column, List<byte[]> tickets, PictureOptions ticketOptions, CommonOptions commonOptions)
        {
            column.Column(column =>
            {
                for (var i = 0; i < tickets.Count; i++)
                {
                    if (i == tickets.Count - 1)
                    {
                        if ((tickets.Count + 1) % 3 == 0)
                        {
                            column.Item()
                            .AddPicture(tickets[i], ticketOptions);

                            column.Item()
                            .PageBreak();
                        }
                        else
                        {
                            column.Item()
                            .AddBottomLine(commonOptions)
                            .AddPicture(tickets[i], ticketOptions);
                        }
                    }
                    else
                    {
                        column.Item()
                        .AddPicture(tickets[i], ticketOptions);
                    }
                }
            });
        }
        public static void AddPicture(this IContainer column, byte[] bitmap, PictureOptions options)
        {
            column
            .PaddingTop(options.PadTop)
            .PaddingBottom(options.PadBottom)
            .Image(bitmap);
        }
    }
}
