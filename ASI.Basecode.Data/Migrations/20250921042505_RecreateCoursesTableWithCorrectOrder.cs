using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ASI.Basecode.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecreateCoursesTableWithCorrectOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create temporary table with correct column order
            migrationBuilder.CreateTable(
                name: "courses_temp",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_code = table.Column<string>(type: "VARCHAR(20)", nullable: true),
                    course_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    units = table.Column<short>(type: "SMALLINT", nullable: false),
                    year_level = table.Column<short>(type: "SMALLINT", nullable: false),
                    available_semester = table.Column<short>(type: "SMALLINT", nullable: false),
                    is_active = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses_temp", x => x.id);
                });

            // Copy data from old table to new table
            migrationBuilder.Sql(@"
                INSERT INTO courses_temp (id, course_code, course_name, description, units, year_level, available_semester, is_active, created_at)
                SELECT id, course_code, course_name, description, units, year_level, available_semester, is_active, created_at
                FROM courses;
            ");

            // Drop foreign key constraints first
            migrationBuilder.DropForeignKey(
                name: "FK_classes_courses_course_id",
                table: "classes");

            // Drop the old table with CASCADE to handle dependencies
            migrationBuilder.DropTable(name: "courses");

            // Rename the temporary table to the original name
            migrationBuilder.RenameTable(name: "courses_temp", newName: "courses");

            // Recreate the unique index on course_code
            migrationBuilder.CreateIndex(
                name: "IX_courses_course_code",
                table: "courses",
                column: "course_code",
                unique: true);

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_classes_courses_course_id",
                table: "classes",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_classes_courses_course_id",
                table: "classes");

            // Drop the recreated table
            migrationBuilder.DropTable(name: "courses");

            // Recreate the original table structure (with the old column order)
            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    units = table.Column<short>(type: "SMALLINT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "NOW()"),
                    available_semester = table.Column<short>(type: "SMALLINT", nullable: false),
                    course_code = table.Column<string>(type: "VARCHAR(20)", nullable: true),
                    is_active = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    year_level = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                });

            // Recreate the unique index
            migrationBuilder.CreateIndex(
                name: "IX_courses_course_code",
                table: "courses",
                column: "course_code",
                unique: true);

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_classes_courses_course_id",
                table: "classes",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
