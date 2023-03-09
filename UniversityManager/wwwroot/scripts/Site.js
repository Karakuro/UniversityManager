
let templStud = (item) => `<tr>
                <td>
                    ${item.name}
                </td>
                <td>
                    ${item.surname}
                </td>
                <td>
                    ${item.fiscalCode}
                </td>
                <td>
                    ${(new Date(item.birthDate)).toLocaleDateString('it-IT')}
                </td>
                <td>
                    <input type="button" class="btn btn-primary" value="Modifica" onclick="EditStudent('${item.id}')" />
                    <input type="button" class="btn btn-danger" value="Elimina" onclick="DeleteStudent('${item.id}')" />
                </td>
            </tr>`
let loadStudentData = () => {
    fetch('api/Student')
        .then(response => response.json())
        .then(json => {
            let rows = json.map(templStud).join('');
            document.getElementById("TableBody").innerHTML = rows;
        });
}

let PostStudent = () => {
    //alert("CHIAMATO!");
    let id = document.getElementById("studentId").value;
    let student = {
        id: id ? id : '00000000-0000-0000-0000-000000000000',
        name: document.getElementById("studentName").value,
        surname: document.getElementById("studentSurname").value,
        fiscalCode: document.getElementById("studentFiscalCode").value,
        birthDate: document.getElementById("studentBirthDate").value
    };
    //student.id = student.id ? student.id : '00000000-0000-0000-0000-000000000000';
    let promise = fetch("api/student", {
        method: id ? "PUT" : "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(student)
    })
        .then((response) => {
            console.log(response);
            if (response.ok) {
                ShowStudentForm();
                loadStudentData();
            }
        }).catch((err) => console.log(err));
}

let ShowStudentForm = () => {
    let form = document.getElementById("formSection");
    let table = document.getElementById("tableSection");
    form.toggleAttribute("hidden");
    table.toggleAttribute("hidden");
    document.getElementById("studentForm").classList.remove("was-validated");
    document.getElementById("studentId").value = "";
    document.getElementById("studentName").value = "";
    document.getElementById("studentSurname").value = "";
    document.getElementById("studentFiscalCode").value = "";
    document.getElementById("studentBirthDate").value = "";
}

let EditStudent = (id) => {
    fetch("api/student/" + id)
        .then((response) => response.json())
        .then((json) => {
            ShowStudentForm();
            document.getElementById("studentId").value = json.id;
            document.getElementById("studentName").value = json.name;
            document.getElementById("studentSurname").value = json.surname;
            document.getElementById("studentFiscalCode").value = json.fiscalCode;
            document.getElementById("studentBirthDate").value = new Date(json.birthDate).toLocaleDateString('fr-CA', {
                year: "numeric",
                month: "2-digit",
                day: "2-digit"
            });
        });
}

let DeleteStudent = (id) => {
    fetch("api/student/" + id, {
        method: "DELETE"
    }).then((response) => {
        if (response.ok) {
            loadStudentData();
        }
    }).catch((err) => console.log(err));
}

(() => {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    const form = document.getElementById('studentForm')

    form.addEventListener('submit', event => {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated')
        if (form.checkValidity()) {
            PostStudent();
        }
    }, false);

    loadStudentData();
})()
