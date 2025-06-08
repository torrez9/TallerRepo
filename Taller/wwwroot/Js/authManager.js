(function () {
    window.authManager = {
        // Almacena token y datos de usuario
        storeUserData: function (token, userData) {
            try {
                // Validar token
                if (typeof token !== 'string' || token.trim() === '') {
                    console.error('❌ Token inválido:', token);
                    return false;
                }

                // Validar y parsear datos de usuario
                let parsedData;
                try {
                    parsedData = JSON.parse(userData);
                    if (!parsedData || typeof parsedData !== 'object') {
                        console.error('❌ Datos de usuario no son un objeto válido');
                        return false;
                    }
                } catch (e) {
                    console.error('❌ Error al parsear datos de usuario:', e);
                    return false;
                }

                // Almacenar en localStorage
                localStorage.setItem('authToken', token);
                localStorage.setItem('userData', userData);
                console.log('✅ Datos almacenados correctamente');
                return true;
            } catch (error) {
                console.error('❌ Error en storeUserData:', error);
                return false;
            }
        },

        // Obtiene el token de autenticación
        getAuthToken: function () {
            try {
                return localStorage.getItem('authToken') || '';
            } catch (error) {
                console.error('❌ Error al obtener authToken:', error);
                return '';
            }
        },

        // Obtiene los datos del usuario actual
        getCurrentUser: function () {
            try {
                const userData = localStorage.getItem('userData');
                if (!userData) return null;
                return JSON.parse(userData);
            } catch (error) {
                console.error('❌ Error al obtener datos de usuario:', error);
                return null;
            }
        },

        // Limpia los datos de autenticación
        clearUserData: function () {
            try {
                localStorage.removeItem('authToken');
                localStorage.removeItem('userData');
                console.log('✅ Datos de usuario eliminados');
                return true;
            } catch (error) {
                console.error('❌ Error al limpiar datos:', error);
                return false;
            }
        },

        // Verifica si el usuario está autenticado
        isLoggedIn: function () {
            return !!this.getAuthToken();
        },

        // Actualiza los datos del usuario
        updateUserData: function (newUserData) {
            try {
                const currentToken = this.getAuthToken();
                if (!currentToken) return false;

                localStorage.setItem('userData', JSON.stringify(newUserData));
                console.log('✅ Datos de usuario actualizados');
                return true;
            } catch (error) {
                console.error('❌ Error al actualizar datos:', error);
                return false;
            }
        },

        // Verifica la autenticación y validez del token
        checkAuth: function () {
            try {
                const token = this.getAuthToken();
                if (!token) {
                    console.log('Usuario no autenticado');
                    return false;
                }

                // Verificación básica de estructura del token JWT
                const tokenParts = token.split('.');
                if (tokenParts.length !== 3) {
                    console.error('Token con formato inválido');
                    this.clearUserData();
                    return false;
                }

                return true;
            } catch (error) {
                console.error('Error en checkAuth:', error);
                return false;
            }
        }
    };
})();

// Función para obtener las citas del usuario autenticado
async function obtenerMisCitas() {
    try {
        // Verificar autenticación primero
        if (!authManager.checkAuth()) {
            throw new Error('No autenticado');
        }

        const token = authManager.getAuthToken();
        const response = await fetch('/api/Citas/MisCitas', {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            // Si la respuesta es 401 (No autorizado), limpiar datos de autenticación
            if (response.status === 401) {
                authManager.clearUserData();
            }
            throw new Error(`Error ${response.status}: ${response.statusText}`);
        }

        const citas = await response.json();
        console.log('Mis citas:', citas);
        return citas;
    } catch (error) {
        console.error('Error al obtener citas:', error);
        throw error;
    }
}