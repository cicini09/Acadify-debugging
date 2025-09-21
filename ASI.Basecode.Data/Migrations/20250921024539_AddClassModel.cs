using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ASI.Basecode.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddClassModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_users_created_by",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "FK_courses_users_teacher_id",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "FK_enrollments_courses_course_id",
                table: "enrollments");

            migrationBuilder.DropIndex(
                name: "IX_courses_created_by",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_join_code",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_teacher_id",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "join_code",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "teacher_id",
                table: "courses");

            migrationBuilder.RenameColumn(
                name: "course_id",
                table: "enrollments",
                newName: "class_id");

            migrationBuilder.RenameIndex(
                name: "IX_enrollments_student_id_course_id",
                table: "enrollments",
                newName: "IX_enrollments_student_id_class_id");

            migrationBuilder.RenameIndex(
                name: "IX_enrollments_course_id",
                table: "enrollments",
                newName: "IX_enrollments_class_id");

            migrationBuilder.AddColumn<bool>(
                name: "is_approved",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "grades",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<decimal>(
                name: "midterm_grade",
                table: "grades",
                type: "numeric(5,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "final_grade",
                table: "grades",
                type: "numeric(5,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<short>(
                name: "available_semester",
                table: "courses",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "course_code",
                table: "courses",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "courses",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "year_level",
                table: "courses",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    semester = table.Column<short>(type: "SMALLINT", nullable: false),
                    year_level = table.Column<short>(type: "SMALLINT", nullable: false),
                    schedule = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    room = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    join_code = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    join_code_generated_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "NOW()"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                    table.ForeignKey(
                        name: "FK_classes_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classes_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_courses_course_code",
                table: "courses",
                column: "course_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_classes_course_id",
                table: "classes",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_join_code",
                table: "classes",
                column: "join_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_classes_teacher_id",
                table: "classes",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "FK_enrollments_classes_class_id",
                table: "enrollments",
                column: "class_id",
                principalTable: "classes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_enrollments_classes_class_id",
                table: "enrollments");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropIndex(
                name: "IX_courses_course_code",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "is_approved",
                table: "users");

            migrationBuilder.DropColumn(
                name: "available_semester",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "course_code",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "year_level",
                table: "courses");

            migrationBuilder.RenameColumn(
                name: "class_id",
                table: "enrollments",
                newName: "course_id");

            migrationBuilder.RenameIndex(
                name: "IX_enrollments_student_id_class_id",
                table: "enrollments",
                newName: "IX_enrollments_student_id_course_id");

            migrationBuilder.RenameIndex(
                name: "IX_enrollments_class_id",
                table: "enrollments",
                newName: "IX_enrollments_course_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "grades",
                type: "TIMESTAMP",
                nullable: true,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "midterm_grade",
                table: "grades",
                type: "numeric(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "final_grade",
                table: "grades",
                type: "numeric(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,1)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "created_by",
                table: "courses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "join_code",
                table: "courses",
                type: "VARCHAR(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "teacher_id",
                table: "courses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_created_by",
                table: "courses",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_courses_join_code",
                table: "courses",
                column: "join_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_teacher_id",
                table: "courses",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_users_created_by",
                table: "courses",
                column: "created_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_courses_users_teacher_id",
                table: "courses",
                column: "teacher_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_enrollments_courses_course_id",
                table: "enrollments",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
