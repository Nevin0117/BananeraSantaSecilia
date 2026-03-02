using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;

namespace SantaSecilia.Application.Services
{
    public class ReporteGlobalPDFGenerator
    {
        public class ActividadReporte
        {
            public required string Actividad { get; set; }
            public required string Horas { get; set; }
            public required string Tarifa { get; set; }
            public required string Total { get; set; }
        }

        // --- Estilos Reutilizables de la Boleta Semanal ---
        private static string ColorFondoCoherente = "#E3F2FD"; // Azul cielo suave
        private static string ColorBordeCoherente = "#BBDEFB";

        private static QuestPDF.Infrastructure.IContainer TableHeaderStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Background(ColorFondoCoherente)
                .Border(1)
                .BorderColor(ColorBordeCoherente)
                .Padding(12)
                .DefaultTextStyle(x => x.Bold().FontSize(10).FontColor(Colors.Black));
        }

        private static QuestPDF.Infrastructure.IContainer TableCellStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor("#D6D6D6")
                .Padding(12)
                .DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Black));
        }

        public static byte[] GenerarPDF(
            string rangoSemana, // Ahora usamos directamente el string procesado del ViewModel
            List<ActividadReporte> actividades,
            string totalPagado,
            string totalHoras,
            int totalJornaleros,
            int totalActividades,
            byte[] logoBytes)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black).FontFamily(Fonts.Verdana));

                    // --- Encabezado ---
                    page.Header().Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            if (logoBytes != null && logoBytes.Length > 0)
                            {
                                row.ConstantItem(75).Image(logoBytes);
                                row.ConstantItem(15);
                            }

                            row.RelativeItem().AlignMiddle().Column(textColumn =>
                            {
                                textColumn.Item().Text("BANANERA SANTA CECILIA")
                                    .FontSize(18).Bold().FontColor(Colors.Black);

                                textColumn.Item().Text("OPERACIONES AGRÍCOLAS")
                                    .FontSize(10).FontColor(Colors.Grey.Medium);

                                textColumn.Item().PaddingTop(2).Text("REPORTE GLOBAL DE ACTIVIDADES")
                                    .FontSize(11).Bold().FontColor("#A8E6CF");
                            });
                        });
                        column.Item().PaddingVertical(10);
                    });

                    // --- Contenido Principal ---
                    page.Content().Column(col =>
                    {
                        // 1. Información de Fecha (Usando el rango real)
                        col.Item().PaddingBottom(15).Row(row =>
                        {
                            row.RelativeItem().Text(t => {
                                t.Span("PERIODO: ").Bold().FontSize(10);
                                t.Span(rangoSemana).FontSize(10);
                            });
                            row.RelativeItem().AlignRight().Text(t => {
                                t.Span("FECHA IMPRESIÓN: ").Bold().FontSize(10);
                                t.Span(DateTime.Now.ToString("dd/MM/yyyy")).FontSize(10);
                            });
                        });

                        // 2. Resumen Ejecutivo (Ahora con el Celeste solicitado)
                        col.Item().PaddingBottom(20).Row(row =>
                        {
                            row.Spacing(10);
                            row.RelativeItem().Component(new ResumenCard("ACTIVIDADES", totalActividades.ToString()));
                            row.RelativeItem().Component(new ResumenCard("JORNALEROS", totalJornaleros.ToString()));
                            row.RelativeItem().Component(new ResumenCard("HORAS TOTALES", totalHoras));
                        });

                        // 3. Tabla de Actividades
                        col.Item().PaddingBottom(10).Text("DESGLOSE DE ACTIVIDADES").Bold().FontSize(11);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2.5f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(TableHeaderStyle).Text("ACTIVIDAD");
                                header.Cell().Element(TableHeaderStyle).AlignCenter().Text("HORAS");
                                header.Cell().Element(TableHeaderStyle).AlignCenter().Text("TARIFA (B/.)");
                                header.Cell().Element(TableHeaderStyle).AlignCenter().Text("TOTAL (B/.)");
                            });

                            foreach (var item in actividades)
                            {
                                table.Cell().Element(TableCellStyle).Text(item.Actividad);
                                table.Cell().Element(TableCellStyle).AlignCenter().Text(item.Horas);
                                table.Cell().Element(TableCellStyle).AlignCenter().Text(item.Tarifa);
                                table.Cell().Element(TableCellStyle).AlignCenter().Text(item.Total);
                            }
                        });

                        // 4. Gran Total (Cuadro Neto)
                        col.Item().PaddingTop(20).AlignRight().Width(250).Column(totalesCol =>
                        {
                            totalesCol.Item()
                                .Background(ColorFondoCoherente)
                                .Border(1)
                                .BorderColor(ColorBordeCoherente)
                                .Padding(12)
                                .DefaultTextStyle(x => x.Bold().FontSize(12).FontColor(Colors.Black))
                                .Row(row =>
                                {
                                    row.RelativeItem().Text("TOTAL PAGADO");
                                    row.ConstantItem(100).AlignRight().Text(totalPagado).FontColor("#2C5F2D");
                                });
                        });
                    });

                    page.Footer().PaddingTop(20).AlignCenter().Text(x =>
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

        // Componente de resumen ajustado al Celeste de la Boleta
        private class ResumenCard : IComponent
        {
            private string Title { get; }
            private string Value { get; }

            public ResumenCard(string title, string value)
            {
                Title = title;
                Value = value;
            }

            public void Compose(QuestPDF.Infrastructure.IContainer container)
            {
                container
                    .Background(ColorFondoCoherente) // Mismo celeste que la boleta
                    .Border(1)
                    .BorderColor(ColorBordeCoherente)
                    .Padding(10)
                    .Column(col =>
                    {
                        col.Item().Text(Title).FontSize(8).Bold().FontColor(Colors.Black);
                        col.Item().PaddingTop(2).Text(Value).FontSize(12).Bold().FontColor(Colors.Black);
                    });
            }
        }
    }
}