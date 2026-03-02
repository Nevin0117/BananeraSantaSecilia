using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantaSecilia.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_daily_records_activities_activity_id",
                table: "daily_records");

            migrationBuilder.DropForeignKey(
                name: "FK_daily_records_lots_lot_id",
                table: "daily_records");

            migrationBuilder.DropForeignKey(
                name: "FK_daily_records_users_created_by_user_id",
                table: "daily_records");

            migrationBuilder.DropForeignKey(
                name: "FK_daily_records_users_updated_by_user_id",
                table: "daily_records");

            migrationBuilder.DropForeignKey(
                name: "FK_daily_records_weekly_closings_weekly_closing_id",
                table: "daily_records");

            migrationBuilder.DropForeignKey(
                name: "FK_weekly_pay_stubs_weekly_closings_weekly_closing_id",
                table: "weekly_pay_stubs");

            migrationBuilder.DropTable(
                name: "audit_events");

            migrationBuilder.DropTable(
                name: "pay_stub_lines");

            migrationBuilder.DropTable(
                name: "record_voidings");

            migrationBuilder.DropTable(
                name: "weekly_closings");

            migrationBuilder.DropTable(
                name: "entity_types");

            migrationBuilder.DropIndex(
                name: "IX_workers_code",
                table: "workers");

            migrationBuilder.DropIndex(
                name: "IX_weekly_pay_stubs_weekly_closing_id_worker_id",
                table: "weekly_pay_stubs");

            migrationBuilder.DropIndex(
                name: "IX_daily_records_activity_id",
                table: "daily_records");

            migrationBuilder.DropIndex(
                name: "IX_daily_records_created_by_user_id",
                table: "daily_records");

            migrationBuilder.DropIndex(
                name: "IX_daily_records_lot_id",
                table: "daily_records");

            migrationBuilder.DropIndex(
                name: "IX_daily_records_updated_by_user_id",
                table: "daily_records");

            migrationBuilder.DropIndex(
                name: "IX_daily_records_weekly_closing_id",
                table: "daily_records");

            migrationBuilder.DropIndex(
                name: "IX_daily_records_worker_id_work_date_activity_id_lot_id",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "code",
                table: "workers");

            migrationBuilder.DropColumn(
                name: "weekly_closing_id",
                table: "weekly_pay_stubs");

            migrationBuilder.DropColumn(
                name: "activity_id",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "allow_duplicate",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "hours",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "is_locked",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "lot_id",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "rate_snapshot",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "subtotal_snapshot",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "daily_records");

            migrationBuilder.DropColumn(
                name: "weekly_closing_id",
                table: "daily_records");

            migrationBuilder.RenameColumn(
                name: "id_number",
                table: "workers",
                newName: "identification_number");

            migrationBuilder.RenameIndex(
                name: "IX_workers_id_number",
                table: "workers",
                newName: "IX_workers_identification_number");

            migrationBuilder.RenameColumn(
                name: "ss_deduction",
                table: "weekly_pay_stubs",
                newName: "week_start");

            migrationBuilder.RenameColumn(
                name: "se_deduction",
                table: "weekly_pay_stubs",
                newName: "week_end");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "weekly_pay_stubs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "education_insurance_deduction",
                table: "weekly_pay_stubs",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "social_security_deduction",
                table: "weekly_pay_stubs",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "daily_record_lines",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    daily_record_id = table.Column<int>(type: "INTEGER", nullable: false),
                    activity_id = table.Column<int>(type: "INTEGER", nullable: false),
                    lot_id = table.Column<int>(type: "INTEGER", nullable: false),
                    hours = table.Column<int>(type: "INTEGER", nullable: false),
                    rate_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    subtotal_snapshot = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_record_lines", x => x.id);
                    table.ForeignKey(
                        name: "FK_daily_record_lines_activities_activity_id",
                        column: x => x.activity_id,
                        principalTable: "activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_daily_record_lines_daily_records_daily_record_id",
                        column: x => x.daily_record_id,
                        principalTable: "daily_records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_daily_record_lines_lots_lot_id",
                        column: x => x.lot_id,
                        principalTable: "lots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_weekly_pay_stubs_week_start_week_end_worker_id",
                table: "weekly_pay_stubs",
                columns: new[] { "week_start", "week_end", "worker_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_daily_record_lines_activity_id",
                table: "daily_record_lines",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "IX_daily_record_lines_daily_record_id_activity_id_lot_id",
                table: "daily_record_lines",
                columns: new[] { "daily_record_id", "activity_id", "lot_id" });

            migrationBuilder.CreateIndex(
                name: "IX_daily_record_lines_lot_id",
                table: "daily_record_lines",
                column: "lot_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_record_lines");

            migrationBuilder.DropIndex(
                name: "IX_weekly_pay_stubs_week_start_week_end_worker_id",
                table: "weekly_pay_stubs");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "weekly_pay_stubs");

            migrationBuilder.DropColumn(
                name: "education_insurance_deduction",
                table: "weekly_pay_stubs");

            migrationBuilder.DropColumn(
                name: "social_security_deduction",
                table: "weekly_pay_stubs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "identification_number",
                table: "workers",
                newName: "id_number");

            migrationBuilder.RenameIndex(
                name: "IX_workers_identification_number",
                table: "workers",
                newName: "IX_workers_id_number");

            migrationBuilder.RenameColumn(
                name: "week_start",
                table: "weekly_pay_stubs",
                newName: "ss_deduction");

            migrationBuilder.RenameColumn(
                name: "week_end",
                table: "weekly_pay_stubs",
                newName: "se_deduction");

            migrationBuilder.AddColumn<int>(
                name: "code",
                table: "workers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "weekly_closing_id",
                table: "weekly_pay_stubs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "activity_id",
                table: "daily_records",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "allow_duplicate",
                table: "daily_records",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "daily_records",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hours",
                table: "daily_records",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_locked",
                table: "daily_records",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "lot_id",
                table: "daily_records",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "rate_snapshot",
                table: "daily_records",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "subtotal_snapshot",
                table: "daily_records",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "updated_by_user_id",
                table: "daily_records",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "weekly_closing_id",
                table: "daily_records",
                type: "INTEGER",
                nullable: true);

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
                name: "pay_stub_lines",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    daily_record_id = table.Column<int>(type: "INTEGER", nullable: false),
                    pay_stub_id = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "weekly_closings",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_by_user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    se_rate_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    ss_rate_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    union_dues_snapshot = table.Column<decimal>(type: "TEXT", nullable: false),
                    week_end = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    week_start = table.Column<DateOnly>(type: "TEXT", nullable: false)
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
                name: "audit_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    entity_type_id = table.Column<int>(type: "INTEGER", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    action = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    details = table.Column<string>(type: "TEXT", nullable: true),
                    entity_id = table.Column<int>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_workers_code",
                table: "workers",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weekly_pay_stubs_weekly_closing_id_worker_id",
                table: "weekly_pay_stubs",
                columns: new[] { "weekly_closing_id", "worker_id" },
                unique: true);

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
                name: "IX_daily_records_worker_id_work_date_activity_id_lot_id",
                table: "daily_records",
                columns: new[] { "worker_id", "work_date", "activity_id", "lot_id" });

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_entity_type_id",
                table: "audit_events",
                column: "entity_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_user_id",
                table: "audit_events",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_entity_types_name",
                table: "entity_types",
                column: "name",
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
                name: "IX_weekly_closings_created_by_user_id",
                table: "weekly_closings",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_weekly_closings_week_start_week_end",
                table: "weekly_closings",
                columns: new[] { "week_start", "week_end" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_daily_records_activities_activity_id",
                table: "daily_records",
                column: "activity_id",
                principalTable: "activities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_daily_records_lots_lot_id",
                table: "daily_records",
                column: "lot_id",
                principalTable: "lots",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_daily_records_users_created_by_user_id",
                table: "daily_records",
                column: "created_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_daily_records_users_updated_by_user_id",
                table: "daily_records",
                column: "updated_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_daily_records_weekly_closings_weekly_closing_id",
                table: "daily_records",
                column: "weekly_closing_id",
                principalTable: "weekly_closings",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_weekly_pay_stubs_weekly_closings_weekly_closing_id",
                table: "weekly_pay_stubs",
                column: "weekly_closing_id",
                principalTable: "weekly_closings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
