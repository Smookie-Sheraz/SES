//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace myWebApp.ViewModels.Placement
//{
//    public class AddPlacement
//    {
//        [Display(Name = "Period")]
//        [Required(ErrorMessage = "{0} is Required")]
//        public int Period { get; set; }
//        [Display(Name = "Placement Date")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [DataType(DataType.Date)]
//        public DateTime PlacementDate { get; set; }
//        [Display(Name = "Location Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("LocationIdExist", "Validations", ErrorMessage = "Location does't exist")]
//        public int LocationId { get; set; }
//        [Display(Name = "Department Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("DepartmentIdExist", "Validations", ErrorMessage = "Department does't exist")]

//        public int DepartmentId { get; set; }
//        [Display(Name = "Designation Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("DesignationIdExist", "Validations", ErrorMessage = "Designation does't exist")]

//        public int DesignationId { get; set; }
//        [Display(Name = "Grade Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("GradeIdExist", "Validations", ErrorMessage = "Grade does't exist")]

//        public int GradeId { get; set; }
//        [Display(Name = "Employee Type Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("EmployeeTypeIdExist", "Validations", ErrorMessage = "Employee Type does't exist")]
//        public int EmployeeTypeId { get; set; }
//        [Display(Name = "Shift Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("ShiftIdExist", "Validations", ErrorMessage = "Shift does't exist")]
//        public int ShiftId { get; set; }
//        [Display(Name = "Section Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("SectionIdExist", "Validations", ErrorMessage = "Section does't exist")]
//        public int SectionId { get; set; }
//        [Display(Name = "Project Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("ProjectIdExist", "Validations", ErrorMessage = "Project does't exist")]

//        public int ProjectId { get; set; }
//        [Display(Name = "School Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("SchoolIdExist", "Validations", ErrorMessage = "School does't exist")]
//        public int SchoolId { get; set; }
//        [Display(Name = "Campus Id")]
//        [Required(ErrorMessage = "{0} is Required")]
//        [Remote("CampusIdExist", "Validations", ErrorMessage = "Location does't exist")]
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
