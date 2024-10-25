function resultsSearch() {
    document.getElementById("btnBuscar").addEventListener("click", function () {
        var codEdo = document.getElementById("selectEstado").value;
        var codMun = document.getElementById("selectMunicipio").value;
        var codPar = document.getElementById("selectParroquia").value;

        // Llama al controlador
        fetch(`/Results/Buscar?codEdo=${codEdo}&codMun=${codMun}&codPar=${codPar}`)
            .then(response => response.text()) // Cambiado a 'text()' para manejar HTML
            .then(data => {
                document.getElementById("resultadosParciales").innerHTML = data;
            })
            .catch(error => console.error('Error:', error));
    });
}

document.getElementById('selectEstado').addEventListener('change', function () {
    const estadoCodigo = this.value;
    const selectMunicipio = document.getElementById("selectMunicipio");
    const selectParroquia = document.getElementById("selectParroquia");

    if (estadoCodigo) {
        fetch(`/Results/GetMunicipios?codEdo=${estadoCodigo}`)
            .then(response => response.json())
            .then(data => {
                selectMunicipio.innerHTML = '<option value="">Seleccione un municipio</option>';
                data.forEach(municipio => {
                    selectMunicipio.innerHTML += `<option value="${municipio.codMun}">${municipio.nombre}</option>`;
                });
                selectMunicipio.disabled = false;

                selectParroquia.innerHTML = '<option value="">Seleccione una parroquia</option>';
                selectParroquia.disabled = true;
            })
            .catch(error => console.error('Error:', error));
    } else {
        // Vaciar y deshabilitar los dropdowns si no se selecciona un estado
        selectMunicipio.innerHTML = '<option value="">Seleccione un municipio</option>';
        selectMunicipio.disabled = true;
        selectParroquia.innerHTML = '<option value="">Seleccione una parroquia</option>';
        selectParroquia.disabled = true;
    }
});

document.getElementById('selectMunicipio').addEventListener('change', function () {
    const munCodigo = this.value;
    const selectParroquia = document.getElementById("selectParroquia");

    if (munCodigo) {
        fetch(`/Results/GetParroquias?codMun=${munCodigo}`)
            .then(response => response.json())
            .then(data => {
                selectParroquia.innerHTML = '<option value="">Seleccione una parroquia</option>';
                data.forEach(parroquia => {
                    selectParroquia.innerHTML += `<option value="${parroquia.codPar}">${parroquia.nombre}</option>`;
                });
                selectParroquia.disabled = false;
            })
            .catch(error => console.error('Error:', error));
    } else {
        // Vaciar y deshabilitar el dropdown de parroquias si no se selecciona un municipio
        selectParroquia.innerHTML = '<option value="">Seleccione una parroquia</option>';
        selectParroquia.disabled = true;
    }
});
