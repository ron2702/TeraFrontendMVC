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