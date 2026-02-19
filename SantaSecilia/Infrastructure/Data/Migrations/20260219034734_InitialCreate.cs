using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantaSecilia.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    deactivated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    hourly_rate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entity_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lots",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    code = table.Column<int>(type: "INTEGER", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    deactivated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lots", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", nullable: false),
                    full_name = table.Column<string>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workers",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    code = table.Column<int>(type: "INTEGER", nullable: false),
                    full_name = table.Column<string>(type: "TEXT", nullable: false),
                    id_number = table.Column<string>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    deactivated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    entity_type_id = table.Column<int>(type: "INTEGER", nullable: false),
                    entity_id = table.Column<int>(type: "INTEGER", nullable: false),
                    action = table.Column<string>(type: "TEXT", nullable: false),
                    details = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_events_entity_types_entity_type_id",
                        column: x => x.entity_type_id,
                        principalTable: "entity_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_audit_events_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "weekly_closings",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    week_start = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    week_end = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    ss_rate_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    se_rate_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    union_dues_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    created_by_user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weekly_closings", x => x.id);
                    table.CheckConstraint("ck_weekly_closings_status", "status IN ('CLOSED', 'VOIDED')");
                    table.ForeignKey(
                        name: "FK_weekly_closings_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "daily_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    worker_id = table.Column<int>(type: "INTEGER", nullable: false),
                    work_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    activity_id = table.Column<int>(type: "INTEGER", nullable: false),
                    lot_id = table.Column<int>(type: "INTEGER", nullable: false),
                    hours = table.Column<int>(type: "INTEGER", nullable: false),
                    rate_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    subtotal_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    created_by_user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_by_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    is_locked = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    weekly_closing_id = table.Column<int>(type: "INTEGER", nullable: true),
                    allow_duplicate = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_records", x => x.id);
                    table.ForeignKey(
                        name: "FK_daily_records_activities_activity_id",
                        column: x => x.activity_id,
                        principalTable: "activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_daily_records_lots_lot_id",
                        column: x => x.lot_id,
                        principalTable: "lots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_daily_records_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_daily_records_users_updated_by_user_id",
                        column: x => x.updated_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_daily_records_weekly_closings_weekly_closing_id",
                        column: x => x.weekly_closing_id,
                        principalTable: "weekly_closings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_daily_records_workers_worker_id",
                        column: x => x.worker_id,
                        principalTable: "workers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "weekly_pay_stubs",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    weekly_closing_id = table.Column<int>(type: "INTEGER", nullable: false),
                    worker_id = table.Column<int>(type: "INTEGER", nullable: false),
                    total_hours = table.Column<int>(type: "INTEGER", nullable: false),
                    gross_earnings = table.Column<decimal>(type: "TEXT", nullable: false),
                    ss_deduction = table.Column<decimal>(type: "TEXT", nullable: false),
                    se_deduction = table.Column<decimal>(type: "TEXT", nullable: false),
                    union_dues = table.Column<decimal>(type: "TEXT", nullable: false),
                    total_deductions = table.Column<decimal>(type: "TEXT", nullable: false),
                    net_pay = table.Column<decimal>(type: "TEXT", nullable: false),
                    generated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    pdf_path = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weekly_pay_stubs", x => x.id);
                    table.ForeignKey(
                        name: "FK_weekly_pay_stubs_weekly_closings_weekly_closing_id",
                        column: x => x.weekly_closing_id,
                        principalTable: "weekly_closings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_weekly_pay_stubs_workers_worker_id",
                        column: x => x.worker_id,
                        principalTable: "workers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "record_voidings",
                columns: table => new
                {
                    record_id = table.Column<int>(type: "INTEGER", nullable: false),
                    voided_by_user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    void_reason = table.Column<string>(type: "TEXT", nullable: true),
                    voided_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_record_voidings", x => x.record_id);
                    table.ForeignKey(
                        name: "FK_record_voidings_daily_records_record_id",
                        column: x => x.record_id,
                        principalTable: "daily_records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_record_voidings_users_voided_by_user_id",
                        column: x => x.voided_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pay_stub_lines",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    pay_stub_id = table.Column<int>(type: "INTEGER", nullable: false),
                    daily_record_id = table.Column<int>(type: "INTEGER", nullable: false),
                    amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pay_stub_lines", x => x.id);
                    table.ForeignKey(
                        name: "FK_pay_stub_lines_daily_records_daily_record_id",
                        column: x => x.daily_record_id,
                        principalTable: "daily_records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pay_stub_lines_weekly_pay_stubs_pay_stub_id",
                        column: x => x.pay_stub_id,
                        principalTable: "weekly_pay_stubs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activities_name",
                table: "activities",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_entity_type_id",
                table: "audit_events",
                column: "entity_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_user_id",
                table: "audit_events",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_daily_records_activity_id",
                table: "daily_records",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "IX_daily_records_created_by_user_id",
                table: "daily_records",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_daily_records_lot_id",
                table: "daily_records",
                column: "lot_id");

            migrationBuilder.CreateIndex(
                name: "IX_daily_records_updated_by_user_id",
                table: "daily_records",
                column: "updated_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_daily_records_weekly_closing_id",
                table: "daily_records",
                column: "weekly_closing_id");

            migrationBuilder.CreateIndex(
                name: "IX_daily_records_worker_id_work_date",
                table: "daily_records",
                columns: new[] { "worker_id", "work_date" });

            migrationBuilder.CreateIndex(
                name: "IX_daily_records_worker_id_work_date_activity_id_lot_id",
                table: "daily_records",
                columns: new[] { "worker_id", "work_date", "activity_id", "lot_id" });

            migrationBuilder.CreateIndex(
                name: "IX_entity_types_name",
                table: "entity_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lots_code",
                table: "lots",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pay_stub_lines_daily_record_id",
                table: "pay_stub_lines",
                column: "daily_record_id");

            migrationBuilder.CreateIndex(
                name: "IX_pay_stub_lines_pay_stub_id",
                table: "pay_stub_lines",
                column: "pay_stub_id");

            migrationBuilder.CreateIndex(
                name: "IX_record_voidings_voided_by_user_id",
                table: "record_voidings",
                column: "voided_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weekly_closings_created_by_user_id",
                table: "weekly_closings",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_weekly_closings_week_start_week_end",
                table: "weekly_closings",
                columns: new[] { "week_start", "week_end" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weekly_pay_stubs_weekly_closing_id_worker_id",
                table: "weekly_pay_stubs",
                columns: new[] { "weekly_closing_id", "worker_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weekly_pay_stubs_worker_id",
                table: "weekly_pay_stubs",
                column: "worker_id");

            migrationBuilder.CreateIndex(
                name: "IX_workers_code",
                table: "workers",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workers_id_number",
                table: "workers",
                column: "id_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_events");

            migrationBuilder.DropTable(
                name: "pay_stub_lines");

            migrationBuilder.DropTable(
                name: "record_voidings");

            migrationBuilder.DropTable(
                name: "entity_types");

            migrationBuilder.DropTable(
                name: "weekly_pay_stubs");

            migrationBuilder.DropTable(
                name: "daily_records");

            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "lots");

            migrationBuilder.DropTable(
                name: "weekly_closings");

            migrationBuilder.DropTable(
                name: "workers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
