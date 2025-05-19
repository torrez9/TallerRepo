// wwwroot/js/authManager.js
window.authManager = {
    storeUserData: function (token, userData) {
        try {
            localStorage.setItem('authToken', token);
            localStorage.setItem('userData', JSON.stringify(userData));
            return true;
        } catch (error) {
            console.error('Error storing user data:', error);
            return false;
        }
    },

    getCurrentUser: function () {
        try {
            const userData = localStorage.getItem('userData');
            return userData ? JSON.parse(userData) : null;
        } catch (error) {
            console.error('Error getting user data:', error);
            return null;
        }
    },

    updateUserData: function (updatedData) {
        try {
            const userData = this.getCurrentUser();
            if (!userData) return false;

            const newUserData = { ...userData, ...updatedData };
            localStorage.setItem('userData', JSON.stringify(newUserData));
            return true;
        } catch (error) {
            console.error('Error updating user data:', error);
            return false;
        }
    },

    clearUserData: function () {
        localStorage.removeItem('authToken');
        localStorage.removeItem('userData');
    }
};