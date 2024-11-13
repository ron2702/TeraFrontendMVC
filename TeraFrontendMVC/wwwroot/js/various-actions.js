// password-toggle.js
function togglePasswordVisibility(inputId, iconId) {
    const passwordInput = document.getElementById(inputId);
    const toggleIcon = document.getElementById(iconId);

    if (passwordInput && toggleIcon) {
        toggleIcon.addEventListener('click', function () {
            const type = passwordInput.type === 'password' ? 'text' : 'password'; // Alterna entre 'password' y 'text'
            passwordInput.type = type;

            // Cambiar el ícono dependiendo del tipo
            this.classList.toggle('fa-eye-slash');
        });
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const successAlert = document.querySelector('.alert-info');
    if (successAlert) {
        setTimeout(() => {
            successAlert.style.display = 'none';
        }, 4000);
    }
});

document.addEventListener("DOMContentLoaded", function () {
    const dangerAlert = document.querySelector('.alert-danger');
    if (dangerAlert) {
        setTimeout(() => {
            dangerAlert.style.display = 'none';
        }, 4000);
    }
});

// Llama a esta función para cada campo de contraseña que necesite el toggle
// togglePasswordVisibility('passwordInput', 'togglePassword');
