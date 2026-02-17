using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;

namespace SantaSecilia.Application.Services
{
    public class ReporteGlobalPDFGenerator
    {
        // Datos de prueba para el reporte
        public class ActividadReporte
        {
            public string Actividad { get; set; }
            public string Horas { get; set; }
            public string Tarifa { get; set; }
            public string Total { get; set; }
        }

        public static byte[] GenerarPDF(string semana, List<ActividadReporte> actividades, string totalPagado, byte[] logoBytes)
        {
            // Configurar licencia (Community es gratis)
            QuestPDF.Settings.License = LicenseType.Community;

            // Generar el PDF y devolverlo como array de bytes
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Configuración de página
                    page.Size(PageSizes.Letter);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black));

                    // Encabezado
                    page.Header()
                        .Column(column =>
                        {
                            // Logo y nombre en horizontal
                            column.Item().Row(row =>
                            {
                                // Logo a la izquierda
                                if (logoBytes != null && logoBytes.Length > 0)
                                {
                                    row.ConstantItem(100).Image(logoBytes);
                                    row.ConstantItem(15); // Espacio entre logo y texto
                                }

                                // Nombre de la finca
                                row.RelativeItem().AlignMiddle().Column(textColumn =>
                                {
                                    textColumn.Item().Text("Bananera Santa Cecilia")
                                        .FontSize(22)
                                        .Bold()
                                        .FontColor("#2C5F2D");

                                    textColumn.Item().Text("Reporte Global")
                                        .FontSize(16)
                                        .FontColor("#555555");
                                });
                            });

                            column.Item().PaddingVertical(5);

                            // Información de la semana y fecha
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"Semana: {semana}")
                                    .FontSize(11)
                                    .FontColor("#555555");

                                row.RelativeItem().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                    .FontSize(11)
                                    .FontColor("#555555")
                                    .AlignRight();
                            });

                            column.Item().PaddingVertical(10);
                            column.Item().LineHorizontal(1).LineColor("#E0E0E0");
                            column.Item().PaddingVertical(5);
                        });

                    // Contenido (tabla)
                    page.Content()
                        .Table(table =>
                        {
                            // Definir columnas con anchos específicos
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);  // Actividad
                                columns.RelativeColumn(2);  // Horas Totales
                                columns.RelativeColumn(2);  // Tarifa
                                columns.RelativeColumn(2.5f);  // Total por Actividad
                            });

                            // Encabezado de tabla
                            table.Header(header =>
                            {
                                header.Cell().Background("#E8F5E9")
                                    .Padding(8).AlignLeft().Text("Actividad").FontSize(12).Bold();
                                header.Cell().Background("#E8F5E9")
                                    .Padding(8).AlignCenter().Text("Horas Totales").FontSize(12).Bold();
                                header.Cell().Background("#E8F5E9")
                                    .Padding(8).AlignCenter().Text("Tarifa").FontSize(12).Bold();
                                header.Cell().Background("#E8F5E9")
                                    .Padding(8).AlignCenter().Text("Total por Actividad").FontSize(12).Bold();
                            });

                            // Filas de datos
                            foreach (var actividad in actividades)
                            {
                                table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                    .Padding(8).AlignLeft().Text(actividad.Actividad).FontSize(11);
                                table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                    .Padding(8).AlignCenter().Text(actividad.Horas).FontSize(11);
                                table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                    .Padding(8).AlignCenter().Text(actividad.Tarifa).FontSize(11);
                                table.Cell().BorderBottom(1).BorderColor("#E0E0E0")
                                    .Padding(8).AlignCenter().Text(actividad.Total).FontSize(11);
                            }

                            // Fila de total
                            table.Cell().ColumnSpan(3).Background("#E8F5E9")
                                .Padding(8).AlignLeft().Text("Total pagado:").FontSize(12).Bold();
                            table.Cell().Background("#E8F5E9")
                                .Padding(8).AlignCenter().Text(totalPagado).FontSize(12).Bold();
                        });

                    // Pie de página
                    page.Footer()
                        .AlignCenter()
                        .DefaultTextStyle(x => x.FontSize(9).FontColor("#999999"))
                        .Text(x =>
                        {
                            x.Span("Página ");
                            x.CurrentPageNumber();
                            x.Span(" de ");
                            x.TotalPages();
                        });
                });
            }).GeneratePdf();

            return pdf;
        }
    }
}