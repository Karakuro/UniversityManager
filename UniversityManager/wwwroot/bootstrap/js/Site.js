
let templStud = (item) => `<tr>
                <td>
                    ${item.id}
                </td>
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
                    ${item.birthDate}
                </td>
            </tr>`

fetch('api/Student')
    .then(response => response.json())
    .then(json => {
        let rows = json.map(templStud).join('');
        document.getElementById("TableBody").innerHTML = rows;
    });