import { createContext, useContext, useEffect, useState } from 'react';
import authService from '../appServices/authService';

const AuthContext = createContext({
    isAuthenticated: authService.isAuthenticated(),
});

export const useAuth = () => useContext(AuthContext);

const AuthProvider = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const isAuth = async () => {
            const res = authService.isAuthenticated();
            setIsAuthenticated(res);
        };
        isAuth();
    });

    return (
        <AuthContext.Provider value={{ isAuthenticated, setIsAuthenticated }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthProvider;