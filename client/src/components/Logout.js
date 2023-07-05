import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from '../appServices/authService';
import { useAuth } from '../providers/AuthProvider';

const Logout = () => {
    const navigate = useNavigate();
    const { setIsAuthenticated } = useAuth();

    useEffect(() => {
        authService.logout();
        setIsAuthenticated(false);
        navigate('/');
      });
    

    return (
        <></>
    );
};

export default Logout;
