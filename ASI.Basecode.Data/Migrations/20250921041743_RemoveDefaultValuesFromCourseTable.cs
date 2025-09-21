using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASI.Basecode.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDefaultValuesFromCourseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove default values from course table columns
            migrationBuilder.AlterColumn<short>(
                name: "available_semester",
                table: "courses",
                type: "SMALLINT",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldDefaultValue: (short)0);

            migrationBuilder.AlterColumn<string>(
                name: "course_code",
                table: "courses",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "courses",
                type: "BOOLEAN",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BOOLEAN",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<short>(
                name: "year_level",
                table: "courses",
                type: "SMALLINT",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldDefaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restore default values to course table columns
            migrationBuilder.AlterColumn<short>(
                name: "available_semester",
                table: "courses",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "SMALLINT");

            migrationBuilder.AlterColumn<string>(
                name: "course_code",
                table: "courses",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "courses",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "BOOLEAN");

            migrationBuilder.AlterColumn<short>(
                name: "year_level",
                table: "courses",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "SMALLINT");
        }
    }
}
