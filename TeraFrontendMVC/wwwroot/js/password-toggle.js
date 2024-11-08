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

// Llama a esta función para cada campo de contraseña que necesite el toggle
// togglePasswordVisibility('passwordInput', 'togglePassword');
