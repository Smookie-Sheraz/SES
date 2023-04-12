using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class StudentParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parent",
                columns: table => new
                {
                    ParentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistraionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ITSNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmissionEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisaNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportValidity = table.Column<DateTime>(type: "date", nullable: true),
                    VisaValidity = table.Column<DateTime>(type: "date", nullable: true),
                    ResidentCardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Employer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DegreeQualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationInstituion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstituionStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    InstituionEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    VaccinationFirstDose = table.Column<DateTime>(type: "date", nullable: true),
                    VaccinationSecondDose = table.Column<DateTime>(type: "date", nullable: true),
                    VaccinationThirdDose = table.Column<DateTime>(type: "date", nullable: true),
                    VaccinationFourthDose = table.Column<DateTime>(type: "date", nullable: true),
                    MotherFName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherLName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherRegistraionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotherGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherITSNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherSecondMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherSecondAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherSecondEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherAdmissionEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherPasswordRepeat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherStatus = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    MotherCNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherPassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherVisaNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherPassportValidity = table.Column<DateTime>(type: "date", nullable: true),
                    MotherVisaValidity = table.Column<DateTime>(type: "date", nullable: true),
                    MotherResidentCardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherFamilyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherMaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherOccupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherEmployer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherOfficeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherOfficeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherDegreeQualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherEducationInstituion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherInstituionStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    MotherInstituionEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    MotherVaccinationFirstDose = table.Column<DateTime>(type: "date", nullable: true),
                    MotherVaccinationSecondDose = table.Column<DateTime>(type: "date", nullable: true),
                    MotherVaccinationThirdDose = table.Column<DateTime>(type: "date", nullable: true),
                    MotherVaccinationFourthDose = table.Column<DateTime>(type: "date", nullable: true),
                    FirstContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstContactRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstContactOfficeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondContactRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondContactOfficeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdContactRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdContactOfficeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FourthContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FourthContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FourthContactRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FourthContactOfficeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FourthContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parent", x => x.ParentId);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistraionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRegistraionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidateNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PalceOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnlyRegisteredNoAdmitted = table.Column<bool>(type: "bit", nullable: true),
                    ITSNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModeOfTransport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmissionEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    RollNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassportValidity = table.Column<DateTime>(type: "date", nullable: true),
                    VisaValidity = table.Column<DateTime>(type: "date", nullable: true),
                    ResidentCardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectricityBillNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterBillNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmissionTestResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardOrEnrollmentNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardOrUniversityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeatNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmittedSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmittedClassOrSection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardMarksObtained = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IgnoreFeeDefaulterRestrictLogin = table.Column<bool>(type: "bit", nullable: true),
                    ScholarchipAmount = table.Column<int>(type: "int", nullable: true),
                    TaxPercentage = table.Column<int>(type: "int", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageSpken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraCurricularActivities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginFeeDefualterRestrictLogin = table.Column<bool>(type: "bit", nullable: true),
                    RestrictLogin = table.Column<bool>(type: "bit", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaccinationFirstDose = table.Column<DateTime>(type: "date", nullable: true),
                    VaccinationSecondDose = table.Column<DateTime>(type: "date", nullable: true),
                    VaccinationThirdDose = table.Column<DateTime>(type: "date", nullable: true),
                    VaccinationFourthDose = table.Column<DateTime>(type: "date", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Student_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Student_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parent",
                        principalColumn: "ParentId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_ClassId",
                table: "Student",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ParentId",
                table: "Student",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Parent");
        }
    }
}
