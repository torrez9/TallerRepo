// wwwroot/js/authManager.js
(function () {
    window.authManager = {
        /**
         * Almacena el token y los datos del usuario en localStorage.
         * @param {string} token El JWT u otro token de autenticación.
         * @param {object} userData Objeto con los datos del usuario.
         * @returns {boolean} true si se almacenó correctamente, false en caso de error.
         */
        storeUserData: function (token, userData) {
            try {
                localStorage.setItem('authToken', token);
                localStorage.setItem('userData', JSON.stringify(userData));
                console.log('✅ authManager: datos de usuario almacenados.');
                return true;
            } catch (error) {
                console.error('❌ authManager: error al almacenar datos de usuario:', error);
                return false;
            }
        },

        /**
         * Recupera el token de autenticación.
         * @returns {string|null} El token almacenado, o null si no existe.
         */
        getAuthToken: function () {
            try {
                return localStorage.getItem('authToken');
            } catch (error) {
                console.error('❌ authManager: error al obtener authToken:', error);
                return null;
            }
        },

        /**
         * Recupera los datos del usuario.
         * @returns {object|null} El objeto de datos de usuario, o null si no existe o hay un error.
         */
        getCurrentUser: function () {
            try {
                const userData = localStorage.getItem('userData');
                return userData ? JSON.parse(userData) : null;
            } catch (error) {
                console.error('❌ authManager: error al obtener datos de usuario:', error);
                return null;
            }
        },

        /**
         * Actualiza los datos de usuario mezclando los existentes con updatedData.
         * @param {object} updatedData Campos a actualizar en el objeto de usuario.
         * @returns {boolean} true si se actualizó correctamente, false en caso de error.
         */
        updateUserData: function (updatedData) {
            try {
                const userData = window.authManager.getCurrentUser();
                if (!userData) return false;
                const newUserData = Object.assign({}, userData, updatedData);
                localStorage.setItem('userData', JSON.stringify(newUserData));
                console.log('✅ authManager: datos de usuario actualizados.');
                return true;
            } catch (error) {
                console.error('❌ authManager: error al actualizar datos de usuario:', error);
                return false;
            }
        },

        /**
         * Elimina token y datos de usuario de localStorage.
         */
        clearUserData: function () {
            try {
                localStorage.removeItem('authToken');
                localStorage.removeItem('userData');
                console.log('✅ authManager: datos de usuario eliminados.');
            } catch (error) {
                console.error('❌ authManager: error al limpiar datos de usuario:', error);
            }
        }
    };
})();
