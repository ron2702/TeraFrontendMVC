let currentPage = '1';
function resultsSearch() {
    const codEdo = document.getElementById("selectEstado")?.value || null;
    const munId = document.getElementById("selectMunicipio")?.value || null;
    const codPar = document.getElementById("selectParroquia")?.value || null;
    const pageSize = document.getElementById("rowsSelect")?.value || 5;

    // Llama al controlador con pageSize actualizado
    fetch(`/Results/Buscar?codEdo=${codEdo}&munId=${munId}&codPar=${codPar}&pageSize=${pageSize}&pageNumber=${currentPage}`)
        .then(response => response.text())
        .then(data => {
            document.getElementById("resultadosParciales").innerHTML = data;
        })
        .catch(error => console.error('Error:', error));
}

document.addEventListener("DOMContentLoaded", function () {
    // Evento en el botón buscar
    const btnBuscar = document.getElementById("btnBuscar");
    if (btnBuscar) {
        btnBuscar.addEventListener("click", function () {
            currentPage = '1'
            resultsSearch(); // Llama a resultsSearch con los valores actuales
        });
    }

    // Evento en dropdown para actualizar pageSize y refrescar resultados automáticamente
    const rowsSelect = document.getElementById("rowsSelect");
    if (rowsSelect) {
        rowsSelect.addEventListener("change", function () {
            resultsSearch(); // Llama a resultsSearch cuando cambia el dropdown de filas
        });
    }
});

function changePage(direction) {
    if (direction === 'next') {
        currentPage++;
    } else if (direction === 'prev' && currentPage > 1) {
        currentPage--;
    }
    resultsSearch();
}

// Verificación en cada `addEventListener`
const selectEstado = document.getElementById("selectEstado");
if (selectEstado) {
    selectEstado.addEventListener("change", function () {
        const estadoCodigo = this.value;
        const selectMunicipio = document.getElementById("selectMunicipio");
        const selectParroquia = document.getElementById("selectParroquia");

        if (estadoCodigo) {
            fetch(`/Results/GetMunicipios?codEdo=${estadoCodigo}`)
                .then(response => response.json())
                .then(data => {
                    selectMunicipio.innerHTML = '<option value="">Seleccione un municipio</option>';
                    data.forEach(municipio => {
                        const cleanedName = cleanName(municipio.nombre); // Llamar a la función de limpieza
                        selectMunicipio.innerHTML += `<option value="${municipio.id}">${cleanedName}</option>`;
                    });
                    selectMunicipio.disabled = false;

                    selectParroquia.innerHTML = '<option value="">Seleccione una parroquia</option>';
                    selectParroquia.disabled = true;
                })
                .catch(error => console.error('Error:', error));
        } else {
            selectMunicipio.innerHTML = '<option value="">Seleccione un municipio</option>';
            selectMunicipio.disabled = true;
            selectParroquia.innerHTML = '<option value="">Seleccione una parroquia</option>';
            selectParroquia.disabled = true;
        }
    });
}

const selectMunicipio = document.getElementById("selectMunicipio");
if (selectMunicipio) {
    selectMunicipio.addEventListener("change", function () {
        const munCodigo = this.value;
        const selectParroquia = document.getElementById("selectParroquia");

        if (munCodigo) {
            fetch(`/Results/GetParroquias?codMun=${munCodigo}`)
                .then(response => response.json())
                .then(data => {
                    selectParroquia.innerHTML = '<option value="">Seleccione una parroquia</option>';
                    data.forEach(parroquia => {
                        const cleanedName = cleanName(parroquia.nombre);
                        selectParroquia.innerHTML += `<option value="${parroquia.codPar}">${cleanedName}</option>`;
                    });
                    selectParroquia.disabled = false;
                })
                .catch(error => console.error('Error:', error));
        } else {
            selectParroquia.innerHTML = '<option value="">Seleccione una parroquia</option>';
            selectParroquia.disabled = true;
        }
    });
}

function cleanName(name) {
    return name.replace(/[\uFFFD]/g, 'Ñ');
}
