//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace myWebApp.ViewModels.Placement
//{
//    public class UpdatePlacement
//    {
//        [Display(Name = "Period")]
//        [Required(ErrorMessage = "{0} is Required")]
//        public int PlacementId { get; set; }
//        [Display(Name = "Period")]
//        [Required(ErrorMessage = "{0} is Required")]
//        public int Period { get; set; }
//        [Display(Name = "Placement Date")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [DataType(DataType.Date)]
//        public DateTime PlacementDate { get; set; }
//        [Display(Name = "Location Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementLocationIdExist", "Validations", ErrorMessage = "Location does't exist")]
//        public int LocationId { get; set; }
//        [Display(Name = "Department Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementDepartmentIdExist", "Validations", ErrorMessage = "Department does't exist")]

//        public int DepartmentId { get; set; }
//        [Display(Name = "Designation Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementDesignationIdExist", "Validations", ErrorMessage = "Designation does't exist")]

//        public int DesignationId { get; set; }
//        [Display(Name = "Grade Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementGradeIdExist", "Validations", ErrorMessage = "Grade does't exist")]

//        public int GradeId { get; set; }
//        [Display(Name = "Employee Type Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementEmployeeTypeIdExist", "Validations", ErrorMessage = "Employee Type does't exist")]
//        public int EmployeeTypeId { get; set; }
//        [Display(Name = "Shift Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementShiftIdExist", "Validations", ErrorMessage = "Shift does't exist")]
//        public int ShiftId { get; set; }
//        [Display(Name = "Section Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementSectionIdExist", "Validations", ErrorMessage = "Section does't exist")]
//        public int SectionId { get; set; }
//        [Display(Name = "Project Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementProjectIdExist", "Validations", ErrorMessage = "Project does't exist")]
//        public int ProjectId { get; set; }
//        [Display(Name = "School Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementSchoolIdExist", "Validations", ErrorMessage = "School does't exist")]
//        public int SchoolId { get; set; }
//        [Display(Name = "Campus Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("UPlacementCampusIdExist", "Validations", ErrorMessage = "Location does't exist")]

//        public int CampusId { get; set; }
//        [Display(Name = "Contract Start Date")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [DataType(DataType.Date)]
//        public DateTime ContractStartDate { get; set; }
//        [Display(Name = "Contract End Date")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [DataType(DataType.Date)]
//        public DateTime ContractEndDate { get; set; }
//        public bool IsContractRenewable { get; set; }

//    }
//}
