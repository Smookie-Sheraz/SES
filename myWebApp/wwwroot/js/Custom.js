
//Function to retrieve the School Section if anyone has done planning in any SchoolSection books to the current user

//function GetSchoolSections(id) {
//    $.get("/AcademicCalendar/GetSchoolSections", { YearId: parseInt($(`#${id}`).val(), 10) }, function (data) {
//        $("#SchoolSectionId").empty();
//        $("#GradeId").empty();
//        $("#ClassId").empty();
//        $("#BookId").empty();
//        $("#SchoolSectionId").append("<Option value='0'>" + "---Select Section Please---" + "</Option>");
//        $.each(data, function (index, row) {
//            console.log(row);
//            $("#SchoolSectionId").append("<Option value='" + row.schoolSectionId + "'>" + row.sectionName + "</Option>")
//        });
//    });
//};

//function GetGrades(id) {
//    $.get("/AcademicCalendar/GetGrades", { SchoolSectionId: parseInt($(`#${id}`).val(), 10) }, function (data) {
//        $("#GradeId").empty();
//        $("#ClassId").empty();
//        $("#BookId").empty();
//        $("#GradeId").append("<Option value='0'>" + "---Select Grade Please---" + "</Option>");
//        $.each(data, function (index, row) {
//            console.log(row);
//            $("#GradeId").append("<Option value='" + row.gradeId + "'>" + row.gradeName + "</Option>")
//        });
//    });
//};

//function GetSubjects(id) {
//    $.get("/AcademicCalendar/GetSubjects", { GradeId: parseInt($(`#${id}`).val(), 10) }, function (data) {
//        $("#SubjectId").empty();
//        $("#BookId").empty();
//        $("#SubjectId").append("<Option value='0'>" + "---Select Subject Please---" + "</Option>");
//        $.each(data, function (index, row) {
//            console.log(row);
//            $("#SubjectId").append("<Option value='" + row.subjectId + "'>" + row.subjectName + "</Option>")
//        });
//    });
//};

//Allocations Dynamic Data

function GetBooks(id) {
    $.get("/academiccalendar/GetBooks", { SectionId: parseInt($(`#${id}`).val(), 10)}, function (data) {
        $("#BookId").empty();
        $("#BookId").append("<option value='0'>" + "---Select Book Please---" + "</option>");
        $.each(data, function (index, row) {
            $("#BookId").append("<option value='" + row.bookId + "'>" + row.bookName + "</option>")
        });
    });
};

function GetUnits(id) {
    $.get("/academiccalendar/GetUnits", { BookId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#UnitId").empty();
        $("#UnitId").append("<option value='0'>" + "---Select Unit Please---" + "</option>");
        $.each(data, function (index, row) {
            $("#UnitId").append("<option value='" + row.unitId + "'>" + row.unitName + "</option>")
        });
    });
};


function GetChapters(id) {
    $.get("/academiccalendar/GetChapters", { UnitId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#ChapterId").empty();
        $("#ChapterId").append("<option value='0'>" + "---Select Chapter Please---" + "</option>");
        $.each(data, function (index, row) {
            $("#ChapterId").append("<option value='" + row.chapterId + "'>" + row.chapterName + "</option>")
        });
    });
};

function GetTopics(id) {
    $.get("/academiccalendar/GetTopics", { ChapterId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#TopicId").empty();
        $("#TopicId").append("<option value='0'>" + "---Select Topic Please---" + "</option>");
        $.each(data, function (index, row) {
            $("#TopicId").append("<option value='" + row.topicId + "'>" + row.topicName + "</option>")
        });
    });
};

function SubjectChanged(id) {
    console.log("I'm Selected!");
    $.get("/AcademicCalendar/GetBooks", { SubjectId: parseInt($(`#${id}`).val(), 10), GradeId: $("#GradeId").val() }, function (data) {
        $("#BookId").empty();
        $("#BookId").append("<Option value='0'>" + "---Select Book Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(data);
            //console.log(index);
            console.log(row);
            $("#BookId").append("<Option value='" + row.bookId + "'>" + row.bookName + "</Option>")
        });
    });
};


function Gradchange(id) {
    //debugger;
    console.log($(`#${id}`).val());
    $("#SubjectId").val('0');
    $("#BookId").val('0');
    $.get("/AcademicCalendar/GetSubjects", { GradeId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#SubjectId").empty();
        $("#SubjectId").append("<Option value='0'>" + "---Select Subject Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(data);
            //console.log(index);
            console.log(row);
            $("#SubjectId").append("<Option value='" + row.subjectId + "'>" + row.subjectName + "</Option>")
        });
    });
};

function SchoolSectionChange(id) {
    //debugger;
    //console.log(parseInt($("${'#id'}").val(),10));
    $("#GradeId").val('0');
    $("#SubjectId").val('0');
    $("#BookId").val('0');
    $.get("/AcademicCalendar/GetGrades", { SchoolSectionId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#GradeId").empty();
        $("#GradeId").append("<Option value='0'>" + "---Select Grade Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(data);
            //console.log(index);
            console.log(row);
            $("#GradeId").append("<Option value='" + row.gradeId + "'>" + row.gradeName + "</Option>")
        });
    });
};

//Function to retrieve the allocated books to the current user

function GetClassBooks(id) {
    //debugger;
    //console.log(parseInt($("${'#id'}").val(),10));
    //debugger;
    $.get("/AcademicCalendar/GetClassBooks", { CLassId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#BookId").empty();
        $("#BookId").append("<Option value='0'>" + "---Select Class Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(data);
            //console.log(index);
            console.log(row);
            $("#BookId").append("<Option value='" + row.bookId + "'>" + row.bookName + "</Option>")
        });
    });
};

//Function to choose the employee type used in Add and Update Employee

function SelectEmployeeType(id) {
    //debugger;
    var empType = $(`#${id}`).val();
    if (empType == 1) {
        var temporaries = document.getElementsByClassName("termporary");
        for (var i = 0; i < temporaries.length; i++) {
            temporaries[i].classList.add("d-none");
        }
        var perm = document.getElementsByClassName("permanent");
        for (var j = 0; j < perm.length; j++) {
            perm[j].classList.remove("d-none");
        }
    }
    else if (empType == 2) {
        var perm = document.getElementsByClassName("permanent");
        for (var j = 0; j < perm.length; j++) {
            perm[j].classList.add("d-none");
        }
        var temporaries = document.getElementsByClassName("termporary");
        for (var i = 0; i < temporaries.length; i++) {
            temporaries[i].classList.remove("d-none");
        }
    }
    else {
        var perm = document.getElementsByClassName("permanent");
        for (var j = 0; j < perm.length; j++) {
            perm[j].classList.add("d-none");
        }
        var temporaries = document.getElementsByClassName("termporary");
        for (var i = 0; i < temporaries.length; i++) {
            temporaries[i].classList.add("d-none");
        }
    }
}

//Book, Unit, Chapter, Topic, SubTopic and Planning Write Urdu
function WriteUrdu(id) {
    debugger;
    var cbox = document.getElementById("writeUrdu");
    console.log(cbox.checked)
    if (cbox.checked) {
        document.getElementById("BookName").setAttribute("lang", "ur");
        document.getElementById("Author").setAttribute("lang", "ur");
        document.getElementById("Publisher").setAttribute("lang", "ur");
        document.getElementById("ResourceBook").setAttribute("lang", "ur");
    }
    else {
        document.getElementById("BookName").removeAttribute("lang");
        document.getElementById("Author").removeAttribute("lang");
        document.getElementById("Publisher").removeAttribute("lang");
        document.getElementById("ResourceBook").removeAttribute("lang");
    }
}

//Function to choose the Question type used in Add and Update Chapter Question

function SelectQuestionType(id) {
    //debugger;
    var questionType = $(`#${id}`).val();
    if (questionType == "True False") {
        document.getElementById("MCQ").classList.add("d-none");
        document.getElementById("TrueFalse").classList.remove("d-none");
    }
    else if (questionType == "MCQ") {
        document.getElementById("TrueFalse").classList.add("d-none");
        document.getElementById("MCQ").classList.remove("d-none");
    }
    else {
        document.getElementById("TrueFalse").classList.add("d-none");
        document.getElementById("MCQ").classList.add("d-none");
    }
}

//Funtion to Generate Dynamic Rows for MCQ Quiz

//function AddNewRow(id) {
//    const table = document.getElementById('MCQChoiceTable');
//    const button = document.getElementById(`${id}`);

//    // Add a click event listener to the button
//    button.addEventListener('click', function () {
//        // Create a new table row
//        const newRow = table.insertRow();

//        // Add two cells to the row
//        const cell1 = newRow.insertCell(0);
//        const cell2 = newRow.insertCell(1);

//        // Add the input and select elements to the cells
//        cell1.innerHTML = '<div class="form-group"><label>Topic Name</label><input type="text" class="form-control" placeholder="Enter Choice Please"/></div>';
//        cell2.innerHTML = '<div class="form-group"><label>True or False?</label><select class="form-select" aria-label="Default select example"><option selected value="@null">Please Select Answer</option><option value="@true">True</option><option value="@false">False</option></select></div>';

//    }
//}


//function to select all the permissions on Add and Update roles page


function SelectAllPermissions() {
    //debugger;
    var checkboxes = document.getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox') {
            checkboxes[i].checked = true;
        }
    }
}

//Funtion to enable the WBStart and End Pages if any workbook selected during the planning

function EnterWBPages(id) {
    var stringId = id.toString();
    var splitId = stringId.split("__");
    if ($(`#${id}`).val() != "") {
        $(`#${splitId[0]}__WorkBookStartPage`).removeAttr("disabled");
        $(`#${splitId[0]}__WorkBookEndPage`).removeAttr("disabled");
    }
    else {
        $(`#${splitId[0]}__WorkBookStartPage`).attr("disabled", true);
        $(`#${splitId[0]}__WorkBookEndPage`).attr("disabled", true);
    }
}

//Function to retrieve the School Section if anyone has done planning in any SchoolSection books to the current user

function GetClassTeachers(id) {
    //debugger;
    $.get("/Grade/GetClassTeachers", { GradeId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#ClassTeacherId").empty();
        $("#ClassTeacherId").append("<Option value='0'>" + "---Select Class Teacher Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(row);
            $("#ClassTeacherId").append("<Option value='" + row.employeeId + "'>" + row.fName + " " + row.lName + "</Option>")
        });
    });
};

//Function to retrieve the Grade Managers according to the selected school section

function GetGradeManagers(id) {
    //debugger;
    $.get("/Grade/GetGradeManagers", { SectionId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#GradeManagerId").empty();
        $("#GradeManagerId").append("<Option value='0'>" + "---Select Grade Manager Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(row);
            $("#GradeManagerId").append("<Option value='" + row.employeeId + "'>" + row.fName + " " + row.lName + "</Option>")
        });
    });
};

//The following function/calls are used in AcademicActivity Controller
//Function to retrieve the Subjects of the diary

function GetDiarySubjects(id) {
    debugger;
    $.get("/AcademicActivities/GetDiarySubjects", { ClassId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#SubjectId").empty();
        $("#SubjectId").append("<Option value='0'>" + "---Select Subject Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(row);
            $("#SubjectId").append("<Option value='" + row.subjectId + "'>" + row.subjectName+ "</Option>")
        });
    });
};

//Function to retrieve the Subjects of the diary
function GetDiaryTests(id) {
    //debugger;
    $.get("/AcademicActivities/GetDiaryTests", { SubjectId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#TestId").empty();
        $("#TestId").append("<Option value='0'>" + "---Select Test Please---" + "</Option>");
        $.each(data, function (index, row) {
            console.log(row);
            $("#TestId").append("<Option value='" + row.testId + "'>" + row.testTitle + "</Option>")
        });
    });
};

//Function to retrieve the Subjects of the Test

function GetTestSubjects(id) {
    debugger;
    $.get("/AcademicActivities/GetTestSubjects", { ClassId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#SubjectId").empty();
        $("#SubjectId").append("<Option value='0'>" + "---Select Subject Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(row);
            $("#SubjectId").append("<Option value='" + row.subjectId + "'>" + row.subjectName + "</Option>")
        });
    });
};

//Function to retrieve the Books of the Test
function GetTestBooks(id) {
    //debugger;
    $.get("/AcademicActivities/GetTestBooks", { SubjectId: parseInt($(`#${id}`).val(), 10) }, function (data) {
        $("#BookId").empty();
        $("#BookId").append("<Option value='0'>" + "---Select Book Please---" + "</Option>");
        $.each(data, function (index, row) {
            //console.log(row);
            $("#BookId").append("<Option value='" + row.bookId + "'>" + row.bookName + "</Option>")
        });
    });
};

//---------------
function GetParentInfo(id){
    var cnicInput = document.getElementById(id);
    var parentData = document.getElementsByClassName("parent-info");

    //cnicInput.addEventListener("input", function () {
        if (cnicInput.value.length === 15) {
            $.get("/Student/GetParentInfo", { CNICNo: $(`#${id}`).val().toString() }, function (data) {
                if (data == null) {
                    for (var i = 0; i < parentData.length; i++) {
                        parentData[i].classList.remove("d-none");
                    }
                    $("#ParentId").val(null);
                }
                else {
                    for (var i = 0; i < parentData.length; i++) {
                        parentData[i].classList.add("d-none");
                    }
                    $("#ParentId").val(data.parentId);
                }
            });
        }
    /*});*/

}